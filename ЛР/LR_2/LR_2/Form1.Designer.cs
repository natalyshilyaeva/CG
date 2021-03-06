namespace LR_2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox1_invisible_lines = new System.Windows.Forms.CheckBox();
            this.checkBox_isometric_view = new System.Windows.Forms.CheckBox();
            this.button_Plus = new System.Windows.Forms.Button();
            this.button_Minus = new System.Windows.Forms.Button();
            this.label_Mashtab = new System.Windows.Forms.Label();
            this.panel_parameters = new System.Windows.Forms.Panel();
            this.button_apply = new System.Windows.Forms.Button();
            this.textBox_radius = new System.Windows.Forms.TextBox();
            this.label_height = new System.Windows.Forms.Label();
            this.textBox_height = new System.Windows.Forms.TextBox();
            this.label_radius = new System.Windows.Forms.Label();
            this.panel_mashtab = new System.Windows.Forms.Panel();
            this.label_name = new System.Windows.Forms.Label();
            this.panel_parameters.SuspendLayout();
            this.panel_mashtab.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1_invisible_lines
            // 
            this.checkBox1_invisible_lines.AutoSize = true;
            this.checkBox1_invisible_lines.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1_invisible_lines.Location = new System.Drawing.Point(565, 30);
            this.checkBox1_invisible_lines.Name = "checkBox1_invisible_lines";
            this.checkBox1_invisible_lines.Size = new System.Drawing.Size(205, 29);
            this.checkBox1_invisible_lines.TabIndex = 0;
            this.checkBox1_invisible_lines.Text = "Невидимые линии";
            this.checkBox1_invisible_lines.UseVisualStyleBackColor = true;
            this.checkBox1_invisible_lines.CheckedChanged += new System.EventHandler(this.CheckBox1_invisible_lines_CheckedChanged);
            // 
            // checkBox_isometric_view
            // 
            this.checkBox_isometric_view.AutoSize = true;
            this.checkBox_isometric_view.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_isometric_view.Location = new System.Drawing.Point(565, 70);
            this.checkBox_isometric_view.Name = "checkBox_isometric_view";
            this.checkBox_isometric_view.Size = new System.Drawing.Size(288, 29);
            this.checkBox_isometric_view.TabIndex = 1;
            this.checkBox_isometric_view.Text = "Изометрическая проекция";
            this.checkBox_isometric_view.UseVisualStyleBackColor = true;
            this.checkBox_isometric_view.CheckedChanged += new System.EventHandler(this.CheckBox_isometric_view_CheckedChanged);
            // 
            // button_Plus
            // 
            this.button_Plus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Plus.Location = new System.Drawing.Point(133, 9);
            this.button_Plus.Name = "button_Plus";
            this.button_Plus.Size = new System.Drawing.Size(54, 43);
            this.button_Plus.TabIndex = 2;
            this.button_Plus.Text = "+";
            this.button_Plus.UseVisualStyleBackColor = true;
            this.button_Plus.Click += new System.EventHandler(this.Button_Plus_Click);
            // 
            // button_Minus
            // 
            this.button_Minus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Minus.Location = new System.Drawing.Point(133, 58);
            this.button_Minus.Name = "button_Minus";
            this.button_Minus.Size = new System.Drawing.Size(54, 45);
            this.button_Minus.TabIndex = 3;
            this.button_Minus.Text = "-";
            this.button_Minus.UseVisualStyleBackColor = true;
            this.button_Minus.Click += new System.EventHandler(this.Button_Minus_Click);
            // 
            // label_Mashtab
            // 
            this.label_Mashtab.AutoSize = true;
            this.label_Mashtab.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Mashtab.Location = new System.Drawing.Point(3, 38);
            this.label_Mashtab.Name = "label_Mashtab";
            this.label_Mashtab.Size = new System.Drawing.Size(95, 25);
            this.label_Mashtab.TabIndex = 4;
            this.label_Mashtab.Text = "Mashtab";
            // 
            // panel_parameters
            // 
            this.panel_parameters.Controls.Add(this.button_apply);
            this.panel_parameters.Controls.Add(this.textBox_radius);
            this.panel_parameters.Controls.Add(this.label_height);
            this.panel_parameters.Controls.Add(this.textBox_height);
            this.panel_parameters.Controls.Add(this.label_radius);
            this.panel_parameters.Location = new System.Drawing.Point(12, 12);
            this.panel_parameters.Name = "panel_parameters";
            this.panel_parameters.Size = new System.Drawing.Size(291, 165);
            this.panel_parameters.TabIndex = 5;
            // 
            // button_apply
            // 
            this.button_apply.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_apply.Location = new System.Drawing.Point(87, 113);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(131, 42);
            this.button_apply.TabIndex = 9;
            this.button_apply.Text = "Применить";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.Button_apply_Click);
            // 
            // textBox_radius
            // 
            this.textBox_radius.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_radius.Location = new System.Drawing.Point(194, 62);
            this.textBox_radius.Name = "textBox_radius";
            this.textBox_radius.Size = new System.Drawing.Size(94, 26);
            this.textBox_radius.TabIndex = 11;
            this.textBox_radius.Text = "3";
            this.textBox_radius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_height
            // 
            this.label_height.AutoSize = true;
            this.label_height.Font = new System.Drawing.Font("Arial Narrow", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_height.Location = new System.Drawing.Point(5, 11);
            this.label_height.Name = "label_height";
            this.label_height.Size = new System.Drawing.Size(169, 24);
            this.label_height.TabIndex = 7;
            this.label_height.Text = "Высота пирамиды";
            // 
            // textBox_height
            // 
            this.textBox_height.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_height.Location = new System.Drawing.Point(195, 9);
            this.textBox_height.Name = "textBox_height";
            this.textBox_height.Size = new System.Drawing.Size(93, 26);
            this.textBox_height.TabIndex = 10;
            this.textBox_height.Text = "3";
            this.textBox_height.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_radius
            // 
            this.label_radius.AutoSize = true;
            this.label_radius.Font = new System.Drawing.Font("Arial Narrow", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_radius.Location = new System.Drawing.Point(5, 62);
            this.label_radius.Name = "label_radius";
            this.label_radius.Size = new System.Drawing.Size(158, 24);
            this.label_radius.TabIndex = 8;
            this.label_radius.Text = "Радиус пирамиды";
            // 
            // panel_mashtab
            // 
            this.panel_mashtab.Controls.Add(this.label_Mashtab);
            this.panel_mashtab.Controls.Add(this.button_Plus);
            this.panel_mashtab.Controls.Add(this.button_Minus);
            this.panel_mashtab.Location = new System.Drawing.Point(337, 12);
            this.panel_mashtab.Name = "panel_mashtab";
            this.panel_mashtab.Size = new System.Drawing.Size(202, 116);
            this.panel_mashtab.TabIndex = 6;
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_name.Location = new System.Drawing.Point(920, 23);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(152, 48);
            this.label_name.TabIndex = 7;
            this.label_name.Text = "Шиляева Н. С.\r\n16 Вариант\r\n";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1147, 775);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.panel_mashtab);
            this.Controls.Add(this.panel_parameters);
            this.Controls.Add(this.checkBox_isometric_view);
            this.Controls.Add(this.checkBox1_invisible_lines);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.panel_parameters.ResumeLayout(false);
            this.panel_parameters.PerformLayout();
            this.panel_mashtab.ResumeLayout(false);
            this.panel_mashtab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1_invisible_lines;
        private System.Windows.Forms.CheckBox checkBox_isometric_view;
        private System.Windows.Forms.Button button_Plus;
        private System.Windows.Forms.Button button_Minus;
        private System.Windows.Forms.Label label_Mashtab;
        private System.Windows.Forms.Panel panel_parameters;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.TextBox textBox_radius;
        private System.Windows.Forms.Label label_height;
        private System.Windows.Forms.TextBox textBox_height;
        private System.Windows.Forms.Label label_radius;
        private System.Windows.Forms.Panel panel_mashtab;
        private System.Windows.Forms.Label label_name;
    }
}

