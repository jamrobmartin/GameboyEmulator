using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameboyEmulator
{
    public partial class ImplementedInstructionsForm : Form
    {
        public ImplementedInstructionsForm()
        {
            InitializeComponent();

            //tableLayoutPanel1.RowCount = 17;
            //tableLayoutPanel1.ColumnCount = 17;

            Populatate();

        }

        private void Populatate()
        {
            tableLayoutPanel1.Controls.Clear();

            for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
            {
                Button button = new Button();
                Byte value = ((i - 1) << 4);
                button.Text = value.ToHexString();
                //button.Text = HexString.Convert((byte)((i - 1) << 4));
                button.Dock = DockStyle.Fill;
                tableLayoutPanel1.Controls.Add(button, 0, i);
            }

            for (int i = 1; i < tableLayoutPanel1.ColumnCount; i++)
            {
                Button button = new Button();
                Byte value = (i - 1);
                button.Text = value.ToHexString();
                button.Dock = DockStyle.Fill;
                tableLayoutPanel1.Controls.Add(button, i, 0);
            }

            for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
            {
                for (int j = 1; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    int value = ((i - 1) << 4) + (j - 1);

                    Instruction inst = new Instruction(value);

                    Button button = new Button();
                    button.Text = inst.OpCode.ToHexString() + Environment.NewLine + inst.Mnemonic();
                    button.Dock = DockStyle.Fill;
                    tableLayoutPanel1.Controls.Add(button, j, i);

                    button.BackColor = GetColorFromInstruction(inst);

                }
            }
        }

        private Color GetColorFromInstruction(Instruction inst)
        {
            Color color = Color.Red;

            switch (inst.InstructionType)
            {
                case eInstructionType.None: color = Color.Red; break;

                case eInstructionType.LD: color = Color.FromArgb(255,199,181,235); break;

                case eInstructionType.LDH: color = Color.FromArgb(255, 199, 181, 235); break;
            }

            return color;
        }


    }
}
