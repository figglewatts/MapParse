using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MapParse.Types;
using MapParse.Util;

namespace MapParse
{
    public static class MapParser
    {
        private enum FaceParseState
        {
            PLANE,
            TEXTURE,
            TEXAXIS,
            ROTATION,
            SCALEX,
            SCALEY
        }

        private enum Vec3ParseState
        {
            X,
            Y,
            Z
        }

        private enum PlaneParseState
        {
            N_X, // x, y, and z values of the normal
            N_Y,
            N_Z,
            DISTANCE
        }

        private static StringBuilder sb = new StringBuilder();

        private static int i = 0;

        public static MapFile ParseMap(string pathToMapFile)
        {
            string contents = File.ReadAllText(pathToMapFile);
            return ParseMapString(contents);
        }

        public static MapFile ParseMapString(string content)
        {
            MapFile map = new MapFile();

            int index = 0;
            char token;
            bool done = false;
            while (!done)
            {
                token = content[index];
                if (token == Constants.LEFT_BRACE)
                {
                    Entity entity = parseEntity(content, ref index);
                    map.Entities.Add(entity);
                }

                if (index == content.Length - 1)
                {
                    done = true;
                    break;
                }

                index++;
            }

            return map;
        }

        private static Entity parseEntity(string content, ref int index)
        {
            Entity entity = new Entity();

            bool done = false;
            char token;
            while (!done)
            {
                token = content[index];

                if (LookAhead(content, index) == Constants.QUOTATION_MARK)
                {
                    Property p = parseProperty(content, ref index);
                    entity.AddProperty(p);
                    index++;
                    continue;
                }

                if (LookAhead(content, index) == Constants.LEFT_BRACE)
                {
                    Brush b = parseBrush(content, ref index);
                    entity.Brushes.Add(b);
                    index++;
                    continue;
                }

                if (LookAhead(content, index) == Constants.RIGHT_BRACE)
                {
                    done = true;
                    break;
                }

                index++;
            }

            return entity;
        }

        private static Property parseProperty(string content, ref int index)
        {
            Property p = new Property();

            bool done = false;
            bool parsedKey = false;
            bool parsing = false;
            char token;
            sb.Length = 0;
            while (!done)
            {
                token = content[index];

                if (!parsing)
                {
                    if (LookAhead(content, index) == Constants.QUOTATION_MARK)
                    {
                        index++;
                        parsing = true;
                        sb.Length = 0;
                    }
                }
                else
                {
                    sb.Append(token);
                    if (LookAhead(content, index) == Constants.QUOTATION_MARK)
                    {
                        parsing = false;
                        if (!parsedKey)
                        {
                            parsedKey = true;
                            p.Key = sb.ToString();
                            sb.Length = 0;
                        }
                        else
                        {
                            p.Value = sb.ToString();
                            done = true;
                            break;
                        }
                    }
                }

                index++;
            }

            return p;
        }

        private static Brush parseBrush(string content, ref int index)
        {
            Brush b = new Brush();

            bool done = false;
            bool parsing = false;
            char token;
            while (!done)
            {
                token = content[index];
                char lookAhead = LookAhead(content, index);

                if (!parsing)
                {
                    if (LookAhead(content, index) == Constants.LEFT_BRACE || token == Constants.SPACE)
                    {
                        parsing = true;
                        continue;
                    }

                    if (LookAhead(content, index) == Constants.RIGHT_BRACE)
                    {
                        done = true;
                        break;
                    }
                }
                else
                {
                    Face f = parseFace(content, ref index);
                    b.Faces.Add(f);
                    parsing = false;
                }

                index++;
            }

            b.GeneratePolys();
            for (int i = 0; i < b.NumberOfFaces; i++)
            {
                for (int j = 0; j < b.Faces[i].Polys.Length; j++)
                {
                    Poly poly = b.Faces[i].Polys[j];
                    poly.P = b.Faces[i].P;
                    poly.SortVerticesCW();
                    b.Faces[i].Polys[j] = poly;
                }
            }

            //b.CalculateAABB();
            return b;
        }

        private static Face parseFace(string content, ref int index)
        {
            Face f = new Face();
            Vec3[] plane = new Vec3[3];
            string texture = "";
            Plane[] texAxis = new Plane[2];
            float rotation = 0F;
            float[] scale = new float[2];

            FaceParseState state = FaceParseState.PLANE;
            bool done = false;
            char token;
            while (!done)
            {
                token = content[index];
                char lookAhead = LookAhead(content, index);

                if (state == FaceParseState.PLANE)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        plane[i] = parseVec3(content, ref index);
                        if (i == 2)
                        {
                            // we're at the end of the plane definition, so get ready to parse the texture
                            state = FaceParseState.TEXTURE;
                            sb.Length = 0;
                        }
                    }
                }
                else if (state == FaceParseState.TEXTURE)
                {
                    if (token == Constants.SPACE)
                    {
                        index++;
                        continue;
                    }

                    if (LookAhead(content, index) == Constants.SPACE)
                    {
                        sb.Append(token);
                        // get ready to parse the texture axis
                        texture = sb.ToString();
                        sb.Length = 0;
                        state = FaceParseState.TEXAXIS;
                    }
                    else
                    {
                        sb.Append(token);
                    }
                }
                else if (state == FaceParseState.TEXAXIS)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        texAxis[i] = parsePlane(content, ref index);
                        if (i == 1)
                        {
                            // get ready to parse texture rotation
                            state = FaceParseState.ROTATION;
                            sb.Length = 0;
                        }
                    }
                }
                else if (state == FaceParseState.ROTATION)
                {
                    if (token == Constants.RIGHT_BRACKET)
                    {
                        index++;
                        continue;
                    }

                    if (LookAhead(content, index) != Constants.SPACE)
                    {
                        sb.Append(token);
                    }
                    else
                    {
                        rotation = float.Parse(sb.ToString());
                        sb.Length = 0;
                        state = FaceParseState.SCALEX;
                    }
                }
                else if (state == FaceParseState.SCALEX)
                {
                    if (LookAhead(content, index) != Constants.SPACE)
                    {
                        sb.Append(token);
                    }
                    else
                    {
                        scale[0] = float.Parse(sb.ToString());
                        sb.Length = 0;
                        state = FaceParseState.SCALEY;
                    }
                }
                else if (state == FaceParseState.SCALEY)
                {
                    if (LookAhead(content, index) != Constants.SPACE &&
                        LookAhead(content, index) != Constants.CARRAIGE_RETURN &&
                        LookAhead(content, index) != Constants.NEWLINE)
                    {
                        sb.Append(token);
                    }
                    else
                    {
                        scale[1] = float.Parse(sb.ToString());
                        sb.Length = 0;
                        done = true;
                        break;
                    }
                }

                index++;
            }

            f.P = new Plane(plane[0], plane[1], plane[2]);
            f.Texture = texture;
            f.TexAxis = texAxis;
            f.Rotation = rotation;
            f.TexScale = scale;
            return f;
        }

        private static Plane parsePlane(string content, ref int index)
        {
            Plane p = new Plane();

            sb.Length = 0;

            PlaneParseState state = PlaneParseState.N_X;
            bool done = false;
            bool parsing = false;
            char token;
            index++;
            while (!done)
            {
                token = content[index];
                char lookAhead = LookAhead(content, index);

                if (!parsing)
                {
                    if (token == Constants.LEFT_BRACKET || LookAhead(content, index) == Constants.LEFT_BRACKET)
                    {
                        index++;
                        continue;
                    }

                    if (LookAhead(content, index) != Constants.SPACE)
                    {
                        parsing = true;
                    }

                    if (LookAhead(content, index) == Constants.RIGHT_BRACKET)
                    {
                        done = true;
                        break;
                    }
                }
                else
                {
                    if (LookAhead(content, index) != Constants.SPACE)
                    {
                        sb.Append(token);
                    }
                    else
                    {
                        switch (state)
                        {
                            case PlaneParseState.N_X:
                                p.Normal.X = float.Parse(sb.ToString());
                                state = PlaneParseState.N_Y;
                                break;
                            case PlaneParseState.N_Y:
                                p.Normal.Z = float.Parse(sb.ToString());
                                state = PlaneParseState.N_Z;
                                break;
                            case PlaneParseState.N_Z:
                                p.Normal.Y = float.Parse(sb.ToString());
                                state = PlaneParseState.DISTANCE;
                                break;
                            case PlaneParseState.DISTANCE:
                                p.Distance = float.Parse(sb.ToString());
                                parsing = false;
                                break;
                        }

                        sb.Length = 0;
                    }
                }

                index++;
            }

            return p;
        }

        private static Vec3 parseVec3(string content, ref int index)
        {
            Vec3 vec3 = new Vec3();

            sb.Length = 0;

            Vec3ParseState state = Vec3ParseState.X;
            bool done = false;
            bool parsing = false;
            char token;

            while (!done)
            {
                token = content[index];
                char lookAhead = LookAhead(content, index);

                if (!parsing)
                {
                    if (token == Constants.LEFT_PARENTHESIS || token == Constants.NEWLINE ||
                        token == Constants.CARRAIGE_RETURN || lookAhead == Constants.SPACE
                        || lookAhead == Constants.LEFT_PARENTHESIS || token == Constants.LEFT_BRACE)
                    {
                        index++;
                        continue;
                    }

                    if (LookAhead(content, index) != Constants.SPACE)
                    {
                        parsing = true;
                    }

                    if (LookAhead(content, index) == Constants.RIGHT_PARENTHESIS)
                    {
                        done = true;
                    }
                }
                else
                {
                    if (LookAhead(content, index) != Constants.SPACE)
                    {
                        sb.Append(token);
                    }
                    else
                    {
                        switch (state)
                        {
                            case Vec3ParseState.X:
                                vec3.X = float.Parse(sb.ToString());
                                state = Vec3ParseState.Y;
                                break;
                            case Vec3ParseState.Y:
                                vec3.Z = float.Parse(sb.ToString());
                                state = Vec3ParseState.Z;
                                break;
                            case Vec3ParseState.Z:
                                vec3.Y = float.Parse(sb.ToString());
                                parsing = false;
                                break;
                        }

                        sb.Length = 0;
                    }
                }

                index++;
            }

            return vec3;
        }

        private static char LookAhead(string content, int index) { return content[index + 1]; }
    }
}
