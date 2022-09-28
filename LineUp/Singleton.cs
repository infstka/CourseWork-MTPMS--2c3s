using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LineUp.Command
{
    public sealed class Singleton
    {
        private Singleton() { }
        private static Singleton sing;

        public static Singleton GetInstance()
        {
            if (sing == null)
            {
                sing = new Singleton();
            }
            return sing;
        }
    }
}
