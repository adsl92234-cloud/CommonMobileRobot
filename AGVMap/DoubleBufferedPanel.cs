using System.ComponentModel;
using System.Windows.Forms;

namespace AGVMap
{
    [DesignerCategory("Code")]  // ← 加這行，告訴 VS 這是純程式碼
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
    }
}