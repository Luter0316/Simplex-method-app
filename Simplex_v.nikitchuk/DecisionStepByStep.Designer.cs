using System;
using System.Threading;
using System.Windows.Forms;

namespace Simplex_v.nikitchuk
{
    partial class DecisionStepByStep
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Button_StepForward = new System.Windows.Forms.Button();
            this.Button_StepBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.AutoScroll = true;
            this.splitContainer.Panel1.Controls.Add(this.dataGridView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.Button_StepForward);
            this.splitContainer.Panel2.Controls.Add(this.Button_StepBack);
            this.splitContainer.Size = new System.Drawing.Size(1264, 410);
            this.splitContainer.SplitterDistance = 381;
            this.splitContainer.TabIndex = 0;
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(935, 379);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // Button_StepForward
            // 
            this.Button_StepForward.Dock = System.Windows.Forms.DockStyle.Right;
            this.Button_StepForward.Location = new System.Drawing.Point(644, 0);
            this.Button_StepForward.Name = "Button_StepForward";
            this.Button_StepForward.Size = new System.Drawing.Size(620, 25);
            this.Button_StepForward.TabIndex = 1;
            this.Button_StepForward.Text = "Вперёд";
            this.Button_StepForward.UseVisualStyleBackColor = true;
            this.Button_StepForward.Click += new System.EventHandler(this.Button_StepForward_Click);
            // 
            // Button_StepBack
            // 
            this.Button_StepBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.Button_StepBack.Location = new System.Drawing.Point(0, 0);
            this.Button_StepBack.Name = "Button_StepBack";
            this.Button_StepBack.Size = new System.Drawing.Size(620, 25);
            this.Button_StepBack.TabIndex = 0;
            this.Button_StepBack.Text = "Назад";
            this.Button_StepBack.UseVisualStyleBackColor = true;
            this.Button_StepBack.Click += new System.EventHandler(this.Button_StepBack_Click);
            // 
            // DecisionStepByStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 410);
            this.Controls.Add(this.splitContainer);
            this.Name = "DecisionStepByStep";
            this.Text = "DecisionStepByStep";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button Button_StepForward;
        private System.Windows.Forms.Button Button_StepBack;

        public DecisionStepByStep(DecisionStepByStep decisionStepByStep)
        {
        }
        private DataGridView dataGridView;
    }
}