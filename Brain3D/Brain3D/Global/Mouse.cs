
namespace Brain3D
{
    interface Mouse
    {
        void Hover();

        void Idle();

        void Click(int x, int y);

        void Move(int x, int y);

        void Push(int x, int y);

        bool Cursor(int x, int y);

        bool Moved(int x, int y);
    }
}
