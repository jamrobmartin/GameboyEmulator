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
            tableLayoutPanel1.BackColor = Color.Black;

            Button corner = new Button();
            corner.Dock = DockStyle.Fill;
            corner.FlatStyle = FlatStyle.Flat;
            corner.FlatAppearance.BorderSize = 0;
            corner.TabStop = false;
            corner.BackColor = Color.Gray;
            tableLayoutPanel1.Controls.Add(corner, 0,0);

            for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
            {
                Button button = new Button();
                Byte value = ((i - 1) << 4);
                button.Text = value.ToHexString();
                button.Dock = DockStyle.Fill;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.TabStop = false;
                button.BackColor = Color.Gray;
                tableLayoutPanel1.Controls.Add(button, 0, i);
            }

            for (int i = 1; i < tableLayoutPanel1.ColumnCount; i++)
            {
                Button button = new Button();
                Byte value = (i - 1);
                button.Text = value.ToHexString();
                button.Dock = DockStyle.Fill;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.TabStop = false;
                button.BackColor = Color.Gray;
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
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.TabStop = false;
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
                case eInstructionType.None: 
                    color = Color.Red; 
                    break;

                case eInstructionType.LD: 
                    color = Color.FromArgb(255, 204, 204, 255);
                    
                    if(inst.AddressingMode == eAddressingMode.Register_D16)
                        color = Color.FromArgb(255, 204, 255, 204);
                    if (inst.Register1 == eRegisterType.SP)
                        color = Color.FromArgb(255, 204, 255, 204);
                    if (inst.Register2 == eRegisterType.SP)
                        color = Color.FromArgb(255, 204, 255, 204);

                    break;

                case eInstructionType.LDH: 
                    color = Color.FromArgb(255, 204, 204, 255); 
                    break;

                case eInstructionType.JP:
                case eInstructionType.JR:
                case eInstructionType.CALL:
                case eInstructionType.RET:
                    color = Color.FromArgb(255, 255, 204, 153);
                    break;
            }

            return color;
        }


    }
}
