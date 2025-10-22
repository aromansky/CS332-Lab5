namespace CS332_Lab5
{
    partial class Menu
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
            this.components = new System.ComponentModel.Container();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.task1ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.task2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.task3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.mountainPreview = new System.Windows.Forms.PictureBox();
            this.roughnessTrackBar = new System.Windows.Forms.TrackBar();
            this.roughnessLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.heightTrackBar = new System.Windows.Forms.TrackBar();
            this.iterationsLabel = new System.Windows.Forms.Label();
            this.iterationsTrackBar = new System.Windows.Forms.TrackBar();
            this.generateMountainBtn = new System.Windows.Forms.Button();
            this.stepByStepBtn = new System.Windows.Forms.Button();
            this.resetMountainBtn = new System.Windows.Forms.Button();
            this.currentIterationLabel = new System.Windows.Forms.Label();
            this.iterationViewTrackBar = new System.Windows.Forms.TrackBar();
            this.bezierPanel = new System.Windows.Forms.Panel();
            this.addPointBtn = new System.Windows.Forms.Button();
            this.removePointBtn = new System.Windows.Forms.Button();
            this.clearCurveBtn = new System.Windows.Forms.Button();
            this.showPolygonCheck = new System.Windows.Forms.CheckBox();
            this.showTangentsCheck = new System.Windows.Forms.CheckBox();
            this.bezierStatusLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mountainPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roughnessTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationsTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationViewTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tasksToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1026, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tasksToolStripMenuItem
            // 
            this.tasksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.task1ToolStripMenuItem1,
            this.task2ToolStripMenuItem,
            this.task3ToolStripMenuItem});
            this.tasksToolStripMenuItem.Name = "tasksToolStripMenuItem";
            this.tasksToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.tasksToolStripMenuItem.Text = "Tasks";
            // 
            // task1ToolStripMenuItem1
            // 
            this.task1ToolStripMenuItem1.Name = "task1ToolStripMenuItem1";
            this.task1ToolStripMenuItem1.Size = new System.Drawing.Size(102, 22);
            this.task1ToolStripMenuItem1.Text = "Task1";
            this.task1ToolStripMenuItem1.Click += new System.EventHandler(this.task1ToolStripMenuItem1_Click);
            // 
            // task2ToolStripMenuItem
            // 
            this.task2ToolStripMenuItem.Name = "task2ToolStripMenuItem";
            this.task2ToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.task2ToolStripMenuItem.Text = "Task2";
            this.task2ToolStripMenuItem.Click += new System.EventHandler(this.task2ToolStripMenuItem_Click);
            // 
            // task3ToolStripMenuItem
            // 
            this.task3ToolStripMenuItem.Name = "task3ToolStripMenuItem";
            this.task3ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.task3ToolStripMenuItem.Text = "Task3";
            this.task3ToolStripMenuItem.Click += new System.EventHandler(this.task3ToolStripMenuItem_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.BackColor = System.Drawing.Color.White;
            this.numericUpDown1.Enabled = false;
            this.numericUpDown1.Location = new System.Drawing.Point(848, 99);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Visible = false;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(845, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Поколение";
            this.label1.Visible = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.White;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(848, 125);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(165, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Использовать случайность";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(845, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Коэф. случайности";
            this.label2.Visible = false;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown2.BackColor = System.Drawing.Color.White;
            this.numericUpDown2.Enabled = false;
            this.numericUpDown2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(848, 165);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown2.TabIndex = 7;
            this.numericUpDown2.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown2.Visible = false;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.BackColor = System.Drawing.Color.White;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(848, 27);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(153, 17);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "Реалистичные L-деревья";
            this.checkBox2.UseVisualStyleBackColor = false;
            this.checkBox2.Visible = false;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(848, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Загрузить конфигурацию";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mountainPreview
            // 
            this.mountainPreview.Location = new System.Drawing.Point(12, 27);
            this.mountainPreview.Name = "mountainPreview";
            this.mountainPreview.Size = new System.Drawing.Size(788, 446);
            this.mountainPreview.TabIndex = 11;
            this.mountainPreview.TabStop = false;
            // 
            // roughnessTrackBar
            // 
            this.roughnessTrackBar.BackColor = System.Drawing.Color.White;
            this.roughnessTrackBar.Location = new System.Drawing.Point(848, 35);
            this.roughnessTrackBar.Maximum = 100;
            this.roughnessTrackBar.Name = "roughnessTrackBar";
            this.roughnessTrackBar.Size = new System.Drawing.Size(157, 45);
            this.roughnessTrackBar.TabIndex = 12;
            this.roughnessTrackBar.Scroll += new System.EventHandler(this.roughnessTrackBar_Scroll);
            // 
            // roughnessLabel
            // 
            this.roughnessLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.roughnessLabel.AutoSize = true;
            this.roughnessLabel.BackColor = System.Drawing.Color.White;
            this.roughnessLabel.Enabled = false;
            this.roughnessLabel.Location = new System.Drawing.Point(845, 19);
            this.roughnessLabel.Name = "roughnessLabel";
            this.roughnessLabel.Size = new System.Drawing.Size(85, 13);
            this.roughnessLabel.TabIndex = 13;
            this.roughnessLabel.Text = "Шероховатость";
            this.roughnessLabel.Visible = false;
            // 
            // heightLabel
            // 
            this.heightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.heightLabel.AutoSize = true;
            this.heightLabel.BackColor = System.Drawing.Color.White;
            this.heightLabel.Enabled = false;
            this.heightLabel.Location = new System.Drawing.Point(845, 79);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(45, 13);
            this.heightLabel.TabIndex = 15;
            this.heightLabel.Text = "Высота";
            this.heightLabel.Visible = false;
            // 
            // heightTrackBar
            // 
            this.heightTrackBar.BackColor = System.Drawing.Color.White;
            this.heightTrackBar.Location = new System.Drawing.Point(848, 95);
            this.heightTrackBar.Maximum = 100;
            this.heightTrackBar.Minimum = -100;
            this.heightTrackBar.Name = "heightTrackBar";
            this.heightTrackBar.Size = new System.Drawing.Size(157, 45);
            this.heightTrackBar.TabIndex = 14;
            this.heightTrackBar.Scroll += new System.EventHandler(this.roughnessTrackBar_Scroll);
            // 
            // iterationsLabel
            // 
            this.iterationsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iterationsLabel.AutoSize = true;
            this.iterationsLabel.BackColor = System.Drawing.Color.White;
            this.iterationsLabel.Enabled = false;
            this.iterationsLabel.Location = new System.Drawing.Point(845, 144);
            this.iterationsLabel.Name = "iterationsLabel";
            this.iterationsLabel.Size = new System.Drawing.Size(56, 13);
            this.iterationsLabel.TabIndex = 17;
            this.iterationsLabel.Text = "Итерации";
            this.iterationsLabel.Visible = false;
            // 
            // iterationsTrackBar
            // 
            this.iterationsTrackBar.BackColor = System.Drawing.Color.White;
            this.iterationsTrackBar.Location = new System.Drawing.Point(848, 160);
            this.iterationsTrackBar.Maximum = 12;
            this.iterationsTrackBar.Minimum = 1;
            this.iterationsTrackBar.Name = "iterationsTrackBar";
            this.iterationsTrackBar.Size = new System.Drawing.Size(157, 45);
            this.iterationsTrackBar.TabIndex = 16;
            this.iterationsTrackBar.Value = 1;
            this.iterationsTrackBar.Scroll += new System.EventHandler(this.roughnessTrackBar_Scroll);
            // 
            // generateMountainBtn
            // 
            this.generateMountainBtn.Location = new System.Drawing.Point(848, 275);
            this.generateMountainBtn.Name = "generateMountainBtn";
            this.generateMountainBtn.Size = new System.Drawing.Size(157, 26);
            this.generateMountainBtn.TabIndex = 18;
            this.generateMountainBtn.Text = "Создать";
            this.generateMountainBtn.UseVisualStyleBackColor = true;
            this.generateMountainBtn.Click += new System.EventHandler(this.generateMountainBtn_Click);
            // 
            // stepByStepBtn
            // 
            this.stepByStepBtn.Location = new System.Drawing.Point(848, 307);
            this.stepByStepBtn.Name = "stepByStepBtn";
            this.stepByStepBtn.Size = new System.Drawing.Size(157, 26);
            this.stepByStepBtn.TabIndex = 19;
            this.stepByStepBtn.Text = "Пошагово";
            this.stepByStepBtn.UseVisualStyleBackColor = true;
            this.stepByStepBtn.Click += new System.EventHandler(this.stepByStepBtn_Click);
            // 
            // resetMountainBtn
            // 
            this.resetMountainBtn.Location = new System.Drawing.Point(848, 339);
            this.resetMountainBtn.Name = "resetMountainBtn";
            this.resetMountainBtn.Size = new System.Drawing.Size(157, 26);
            this.resetMountainBtn.TabIndex = 20;
            this.resetMountainBtn.Text = "Сбросить";
            this.resetMountainBtn.UseVisualStyleBackColor = true;
            this.resetMountainBtn.Click += new System.EventHandler(this.resetMountainBtn_Click);
            // 
            // currentIterationLabel
            // 
            this.currentIterationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentIterationLabel.AutoSize = true;
            this.currentIterationLabel.BackColor = System.Drawing.Color.White;
            this.currentIterationLabel.Enabled = false;
            this.currentIterationLabel.Location = new System.Drawing.Point(845, 208);
            this.currentIterationLabel.Name = "currentIterationLabel";
            this.currentIterationLabel.Size = new System.Drawing.Size(79, 13);
            this.currentIterationLabel.TabIndex = 22;
            this.currentIterationLabel.Text = "Итерация: 0/8";
            this.currentIterationLabel.Visible = false;
            // 
            // iterationViewTrackBar
            // 
            this.iterationViewTrackBar.BackColor = System.Drawing.Color.White;
            this.iterationViewTrackBar.Location = new System.Drawing.Point(848, 224);
            this.iterationViewTrackBar.Maximum = 1;
            this.iterationViewTrackBar.Name = "iterationViewTrackBar";
            this.iterationViewTrackBar.Size = new System.Drawing.Size(157, 45);
            this.iterationViewTrackBar.TabIndex = 21;
            this.iterationViewTrackBar.Scroll += new System.EventHandler(this.iterationViewTrackBar_Scroll);
            // 
            // bezierPanel
            // 
            this.bezierPanel.BackColor = System.Drawing.Color.PaleTurquoise;
            this.bezierPanel.Location = new System.Drawing.Point(12, 27);
            this.bezierPanel.Name = "bezierPanel";
            this.bezierPanel.Size = new System.Drawing.Size(788, 446);
            this.bezierPanel.TabIndex = 23;
            this.bezierPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bezierPanel_Paint);
            this.bezierPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BezierPanel_MouseDown);
            this.bezierPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BezierPanel_MouseMove);
            this.bezierPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BezierPanel_MouseUp);
            // 
            // addPointBtn
            // 
            this.addPointBtn.Location = new System.Drawing.Point(492, 517);
            this.addPointBtn.Name = "addPointBtn";
            this.addPointBtn.Size = new System.Drawing.Size(157, 26);
            this.addPointBtn.TabIndex = 24;
            this.addPointBtn.Text = "Добавление точек";
            this.addPointBtn.UseVisualStyleBackColor = true;
            this.addPointBtn.Click += new System.EventHandler(this.AddPointBtn_Click);
            // 
            // removePointBtn
            // 
            this.removePointBtn.Location = new System.Drawing.Point(655, 517);
            this.removePointBtn.Name = "removePointBtn";
            this.removePointBtn.Size = new System.Drawing.Size(157, 26);
            this.removePointBtn.TabIndex = 25;
            this.removePointBtn.Text = "Удаление точек";
            this.removePointBtn.UseVisualStyleBackColor = true;
            this.removePointBtn.Click += new System.EventHandler(this.RemovePointBtn_Click);
            // 
            // clearCurveBtn
            // 
            this.clearCurveBtn.Location = new System.Drawing.Point(818, 517);
            this.clearCurveBtn.Name = "clearCurveBtn";
            this.clearCurveBtn.Size = new System.Drawing.Size(157, 26);
            this.clearCurveBtn.TabIndex = 26;
            this.clearCurveBtn.Text = "Очистка кривой";
            this.clearCurveBtn.UseVisualStyleBackColor = true;
            this.clearCurveBtn.Click += new System.EventHandler(this.ClearCurveBtn_Click);
            // 
            // showPolygonCheck
            // 
            this.showPolygonCheck.AutoSize = true;
            this.showPolygonCheck.Checked = true;
            this.showPolygonCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showPolygonCheck.Location = new System.Drawing.Point(329, 517);
            this.showPolygonCheck.Name = "showPolygonCheck";
            this.showPolygonCheck.Size = new System.Drawing.Size(119, 17);
            this.showPolygonCheck.TabIndex = 27;
            this.showPolygonCheck.Text = "Показать полигон";
            this.showPolygonCheck.UseVisualStyleBackColor = true;
            this.showPolygonCheck.CheckedChanged += new System.EventHandler(this.ShowPolygonCheck_CheckedChanged);
            // 
            // showTangentsCheck
            // 
            this.showTangentsCheck.AutoSize = true;
            this.showTangentsCheck.Checked = true;
            this.showTangentsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTangentsCheck.Location = new System.Drawing.Point(125, 516);
            this.showTangentsCheck.Name = "showTangentsCheck";
            this.showTangentsCheck.Size = new System.Drawing.Size(133, 17);
            this.showTangentsCheck.TabIndex = 28;
            this.showTangentsCheck.Text = "Плавное соединение";
            this.showTangentsCheck.UseVisualStyleBackColor = true;
            this.showTangentsCheck.CheckedChanged += new System.EventHandler(this.ShowTangentsCheck_CheckedChanged);
            // 
            // bezierStatusLabel
            // 
            this.bezierStatusLabel.AutoSize = true;
            this.bezierStatusLabel.Location = new System.Drawing.Point(12, 485);
            this.bezierStatusLabel.Name = "bezierStatusLabel";
            this.bezierStatusLabel.Size = new System.Drawing.Size(35, 13);
            this.bezierStatusLabel.TabIndex = 29;
            this.bezierStatusLabel.Text = "label3";
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1026, 545);
            this.Controls.Add(this.bezierStatusLabel);
            this.Controls.Add(this.showTangentsCheck);
            this.Controls.Add(this.showPolygonCheck);
            this.Controls.Add(this.clearCurveBtn);
            this.Controls.Add(this.removePointBtn);
            this.Controls.Add(this.addPointBtn);
            this.Controls.Add(this.bezierPanel);
            this.Controls.Add(this.currentIterationLabel);
            this.Controls.Add(this.iterationViewTrackBar);
            this.Controls.Add(this.resetMountainBtn);
            this.Controls.Add(this.stepByStepBtn);
            this.Controls.Add(this.generateMountainBtn);
            this.Controls.Add(this.iterationsLabel);
            this.Controls.Add(this.iterationsTrackBar);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.heightTrackBar);
            this.Controls.Add(this.roughnessLabel);
            this.Controls.Add(this.roughnessTrackBar);
            this.Controls.Add(this.mountainPreview);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mountainPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roughnessTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationsTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationViewTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ToolStripMenuItem tasksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem task1ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem task2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem task3ToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox mountainPreview;
        private System.Windows.Forms.TrackBar roughnessTrackBar;
        private System.Windows.Forms.Label roughnessLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.TrackBar heightTrackBar;
        private System.Windows.Forms.Label iterationsLabel;
        private System.Windows.Forms.TrackBar iterationsTrackBar;
        private System.Windows.Forms.Button generateMountainBtn;
        private System.Windows.Forms.Button stepByStepBtn;
        private System.Windows.Forms.Button resetMountainBtn;
        private System.Windows.Forms.Label currentIterationLabel;
        private System.Windows.Forms.TrackBar iterationViewTrackBar;
        private System.Windows.Forms.Panel bezierPanel;
        private System.Windows.Forms.Button addPointBtn;
        private System.Windows.Forms.Button removePointBtn;
        private System.Windows.Forms.Button clearCurveBtn;
        private System.Windows.Forms.CheckBox showPolygonCheck;
        private System.Windows.Forms.CheckBox showTangentsCheck;
        private System.Windows.Forms.Label bezierStatusLabel;
    }
}

