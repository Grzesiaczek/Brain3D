
namespace Brain3D
{
    class Sound
    {
        int code;
        int length;

        public Sound(string data)
        {
            int k = 1;
            code = convert(data[0]);

            if(data[k] == '#')
            {
                k++;
                code++;
            }
            else if(data[k] == 'b')
            {
                k++;
                code--;
            }

            code += (data[k++] - 47) * 12;
            bool dot = true;

            if (data[k++] == 'N')
            {
                dot = false;
            }

            int n = int.Parse(data.Substring(k));
            length = 32 / n;

            if (dot)
            {
                length += length / 2;
            }
        }

        int convert(char c)
        {
            switch(c)
            {
                case 'C':
                    return 0;
                case 'D':
                    return 2;
                case 'E':
                    return 4;
                case 'F':
                    return 5;
                case 'G':
                    return 7;
                case 'A':
                    return 9;
                case 'H':
                    return 11;
            }

            return 0;
        }

        public int Code
        {
            get { return code ;}
        }

        public int Length
        {
            get { return length; }
        }
    }
}
