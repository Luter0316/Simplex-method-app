using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simplex_v.nikitchuk
{
    public partial class DecisionStepByStep : Form
    {
        public bool firstStepComplete = false;
        public bool lastStepComplete = false;
        public bool IsCalculated = false;
        public bool firstFlag = false;

        public int CounterIteration = 0;
        public int BasisColumn; //Переменная текущего базисного столбца
        public int BasisRow; //Переменная текущей базисной строки

        public List<int> ValueRow = new List<int>(); //Список исключенных из поиска строк
        public List<int> columnMinLst = new List<int>(); //Список индексов столбцов опорных элементов
        public List<int> rowMinLst = new List<int>(); //Список индексов строк опорных элементов

        public List<List<int>> saveValueRow = new List<List<int>>(); //Список проверенных строк для реализации "шага назад"
        public List<List<int>> saveColumnMin = new List<List<int>>(); //Список базисных столбцов для реализации "шага назад"
        public List<List<int>> saveRowMin = new List<List<int>>(); //Список базисных строк для реализации "шага назад"

        public List<List<List<String>>> saveMatrixSBS = new List<List<List<String>>>(); //3-х мерный список основной мастрицы для реализации "шага назад" - десятичные дроби
        public List<List<List<Fraction>>> saveFractionMatrixSBS = new List<List<List<Fraction>>>(); //3-х мерный список основной мастрицы для реализации "шага назад" - натуральные дроби
        public List<List<List<String>>> saveIndexSBS = new List<List<List<String>>>(); //3-х мерный список матрицы индексов для реализации "шага назад"

        public DecisionStepByStep()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Стиль границ формы
            this.Size = new Size(1280, 445); //Размер формы
            this.Text = "Пошаговый метод искуственного базиса"; //Заголовок формы
            this.StartPosition = FormStartPosition.CenterScreen; //Открыть форму по центру экрана
            splitContainer.IsSplitterFixed = true; //Запрет на изменения размера "контейнеров"
            Button_StepForward_Click(this, null);
            this.ShowDialog();
        }

        //Кнопка шага вперёд
        private void Button_StepForward_Click(object sender, System.EventArgs e)
        {
            if (CounterIteration != 0)
            {
                if (Program.FormWithCondition.IsDecimal)
                {
                    if (!firstStepComplete)
                    {
                        if (Program.FormWithCondition.MainStepCheck())
                        {
                            SaveDataToLst(Program.FormWithCondition);
                            StepByStepCalculation(1, Program.FormWithCondition); //Расчёт "первого шага"
                        }
                        if (!Program.FormWithCondition.MainStepCheck())
                        {
                            firstStepComplete = true;
                            Button_StepForward_Click(this, null);
                            ReCalculation(Program.FormWithCondition); //Пересчёт перед "вторым шагом"
                            if (!Program.FormWithCondition.SecondStepCheck())
                            {
                                Answer(1000, 15, Program.FormWithCondition); //Вывод ответа
                            }
                        }
                        if (Program.FormWithCondition.ValueRow.Count == dataGridView.RowCount - 1)
                        {
                            Program.FormWithCondition.solvability();
                            this.Close();
                        }
                    }
                    else
                    {
                        if (Program.FormWithCondition.solvability()) //Если задача разрешима
                        {
                            if (Program.FormWithCondition.SecondStepCheck())
                            {
                                lastStepComplete = true;
                                SaveDataToLst(Program.FormWithCondition);
                                StepByStepCalculation(0, Program.FormWithCondition); //Расчёт "второго шага"
                            }
                            if (lastStepComplete && !Program.FormWithCondition.SecondStepCheck())
                            {
                                Answer(1000, 15, Program.FormWithCondition); //Вывод ответа
                            }
                        }
                        else
                        {
                            this.Close();
                        }
                    }

                }
                else if (!Program.FormWithCondition.IsDecimal)
                {
                    if (!firstStepComplete)
                    {
                        if (Program.FormWithCondition.MainStepCheck())
                        {
                            SaveDataToLst(Program.FormWithCondition);
                            FractionStepByStepCalculation(1, Program.FormWithCondition); //Расчёт "первого шага"
                        }
                        if (!Program.FormWithCondition.MainStepCheck())
                        {
                            firstStepComplete = true;
                            Button_StepForward_Click(this, null);
                            ReCalculation(Program.FormWithCondition); //Пересчёт перед "вторым шагом"
                            if (!Program.FormWithCondition.SecondStepCheck())
                            {
                                Fraction_Answer(1000, 15, Program.FormWithCondition); //Вывод ответа
                            }
                            MinFind(Program.FormWithCondition);
                        }
                        if (Program.FormWithCondition.ValueRow.Count == dataGridView.RowCount - 1)
                        {
                            Program.FormWithCondition.solvability();
                            this.Close();
                        }
                    }
                    else
                    {
                        if (Program.FormWithCondition.solvability()) //Если задача разрешима
                        {
                            if (Program.FormWithCondition.SecondStepCheck())
                            {
                                lastStepComplete = true;
                                SaveDataToLst(Program.FormWithCondition);
                                FractionStepByStepCalculation(0, Program.FormWithCondition); //Расчёт "второго шага"
                            }
                            if (lastStepComplete && !Program.FormWithCondition.SecondStepCheck())
                            {
                                Fraction_Answer(1000, 15, Program.FormWithCondition); //Вывод ответа
                            }
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
            }
            MinFind(Program.FormWithCondition);
            CounterIteration++;
            CheckSteps();

            if (IsCalculated)
            {
                Button_StepForward.Enabled = false;
                Button_StepBack.Enabled = false;
            }

            this.Update();
        }
        //Кнопка шага назад
        private void Button_StepBack_Click(object sender, EventArgs e)
        {
            Program.FormWithCondition.StartMatrix.Clear();
            Program.FormWithCondition.Fraction_StartMatrix.Clear();
            Program.FormWithCondition.ListOfIndex.Clear();
            ValueRow.Clear();
            columnMinLst.Clear();
            rowMinLst.Clear();

            if (Program.FormWithCondition.IsDecimal)
            {
                //Замена основной матрицы
                for (int r = 0; r < saveMatrixSBS[saveMatrixSBS.Count - 1].Count; r++)
                {
                    List<string> row = new List<string>();
                    for (int c = 0; c < saveMatrixSBS[saveMatrixSBS.Count - 1][0].Count; c++)
                    {
                        row.Add(saveMatrixSBS[saveMatrixSBS.Count - 1][r][c]);
                    }
                    Program.FormWithCondition.StartMatrix.Add(row);
                }
                saveMatrixSBS.RemoveAt(saveMatrixSBS.Count - 1);
            }
            else if (!Program.FormWithCondition.IsDecimal)
            {
                //Замена основной матрицы
                for (int r = 0; r < saveFractionMatrixSBS[saveFractionMatrixSBS.Count - 1].Count; r++)
                {
                    List<Fraction> row = new List<Fraction>();
                    for (int c = 0; c < saveFractionMatrixSBS[saveFractionMatrixSBS.Count - 1][0].Count; c++)
                    {
                        row.Add(saveFractionMatrixSBS[saveFractionMatrixSBS.Count - 1][r][c]);
                    }
                    Program.FormWithCondition.Fraction_StartMatrix.Add(row);
                }
                saveFractionMatrixSBS.RemoveAt(saveFractionMatrixSBS.Count - 1);
            }

            //Замена матрицы индексов
            for (int r = 0; r < saveIndexSBS[saveIndexSBS.Count - 1].Count; r++)
            {
                List<string> row = new List<string>();
                for (int c = 0; c < saveIndexSBS[saveIndexSBS.Count - 1][0].Count; c++)
                {
                    row.Add(saveIndexSBS[saveIndexSBS.Count - 1][r][c]);
                }
                Program.FormWithCondition.ListOfIndex.Add(row);
            }
            saveIndexSBS.RemoveAt(saveIndexSBS.Count - 1);

            //Замена списка проверенных строк
            for (int c = 0; c < saveValueRow[saveValueRow.Count - 1].Count; c++)
            {
                ValueRow.Add(saveValueRow[saveValueRow.Count - 1][c]);
            }
            saveValueRow.RemoveAt(saveValueRow.Count - 1);

            //Замена списка базисных строк
            for (int c = 0; c < saveRowMin[saveRowMin.Count - 1].Count; c++)
            {
                rowMinLst.Add(saveRowMin[saveRowMin.Count - 1][c]);
            }
            saveRowMin.RemoveAt(saveRowMin.Count - 1);

            //Замена списка базисных столбцов
            for (int c = 0; c < saveColumnMin[saveColumnMin.Count - 1].Count; c++)
            {
                columnMinLst.Add(saveColumnMin[saveColumnMin.Count - 1][c]);
            }
            saveColumnMin.RemoveAt(saveColumnMin.Count - 1);

            CounterIteration --;
            CheckSteps();

            if (CounterIteration <= Program.FormWithCondition.RestrictionsCounter)
            {
                firstStepComplete = false;
            }

            UpdateDGV(0, 0, 3, Program.FormWithCondition);
        }

        //
        //Функции расчета
        //
        //Функция, расчитывающая автоматический метод искуственного базиса для десятичных дробей
        public void StepByStepCalculation(int flag, Condition condition_form)
        {
            int columnMin = BasisColumn; //индекс опроного столбца
            int rowMin = BasisRow; //Индекс опорной строки
            //
            //Изменение индексов переменных
            //
            var tmp = condition_form.ListOfIndex[rowMin + 1][0];
            condition_form.ListOfIndex[rowMin + 1][0] = condition_form.ListOfIndex[0][columnMin + 1];
            condition_form.ListOfIndex[0][columnMin + 1] = tmp;

            condition_form.StartMatrix[rowMin][columnMin] = Math.Round(1 / double.Parse(condition_form.StartMatrix[rowMin][columnMin]), 4).ToString(); //Пересчет опорного элемента
            //
            //Пересчет опорной строки
            //
            for (int c = 0; c < condition_form.StartMatrix[0].Count; c++)
            {
                if (c != columnMin && c != condition_form.StartMatrix[0].Count - 2)
                {
                    condition_form.StartMatrix[rowMin][c] = Math.Round(double.Parse(condition_form.StartMatrix[rowMin][columnMin]) * double.Parse(condition_form.StartMatrix[rowMin][c]), 4).ToString(); //Элемент в опроной строке = опорный элемент * текущий элемент
                }
            }
            //
            //Пересчет не опорных элементов
            //
            for (int r = 0; r < condition_form.StartMatrix.Count; r++)
            {
                for (int c = 0; c < condition_form.StartMatrix[0].Count; c++)
                {
                    if (c != condition_form.StartMatrix[0].Count - 2 && c != columnMin && r != rowMin)
                    {
                        condition_form.StartMatrix[r][c] = Math.Round(double.Parse(condition_form.StartMatrix[r][c]) - (double.Parse(condition_form.StartMatrix[r][columnMin]) * double.Parse(condition_form.StartMatrix[rowMin][c])), 4).ToString(); //Новый элемент = старый элемент - (элемент этой строки в опорном столбце * элемент в этом столбце опорной строки)
                    }
                }
            }
            //
            //Пересчет опорного столбца
            //
            for (int r = 0; r < condition_form.StartMatrix.Count; r++)
            {
                if (r != rowMin)
                {
                    condition_form.StartMatrix[r][columnMin] = Math.Round(-1 * double.Parse(condition_form.StartMatrix[rowMin][columnMin]) * double.Parse(condition_form.StartMatrix[r][columnMin]), 4).ToString(); //Элемент в опорном столбце = -опорный элемент * текущий элемент
                }
            }
            if (flag == 1)
            {
                //
                //Удаление измененного индекса
                //
                foreach (var c in condition_form.ListOfIndex)
                {
                    c.RemoveAt(columnMin + 1);
                }
                //
                //Удаление столбца из матрицы
                //
                foreach (var c in condition_form.StartMatrix)
                {
                    c.RemoveAt(columnMin);
                }
            }
        }
        //Функция, расчитывающая автоматический метод искуственного базиса для натуральных дробей
        public void FractionStepByStepCalculation(int flag, Condition condition_form)
        {
            int columnMin = BasisColumn; //индекс опроного столбца
            int rowMin = BasisRow; //Индекс опорной строки
            //
            //Изменение индексов переменных
            //
            var tmp = condition_form.ListOfIndex[rowMin + 1][0];
            condition_form.ListOfIndex[rowMin + 1][0] = condition_form.ListOfIndex[0][columnMin + 1];
            condition_form.ListOfIndex[0][columnMin + 1] = tmp;

            condition_form.Fraction_StartMatrix[rowMin][columnMin] = condition_form.Fraction_StartMatrix[rowMin][columnMin].GetReverse(); //Пересчет опорного элемента
            //
            //Пересчет опорной строки
            //
            for (int c = 0; c < condition_form.Fraction_StartMatrix[0].Count; c++)
            {
                if (c != columnMin)
                {
                    condition_form.Fraction_StartMatrix[rowMin][c] *= condition_form.Fraction_StartMatrix[rowMin][columnMin]; //Элемент в опроной строке = опорный элемент * текущий элемент
                }
            }
            //
            //Пересчет не опорных элементов
            //
            for (int r = 0; r < condition_form.Fraction_StartMatrix.Count; r++)
            {
                for (int c = 0; c < condition_form.Fraction_StartMatrix[0].Count; c++)
                {
                    if (c != columnMin && r != rowMin)
                    {
                        condition_form.Fraction_StartMatrix[r][c] -= condition_form.Fraction_StartMatrix[r][columnMin] * condition_form.Fraction_StartMatrix[rowMin][c]; //Новый элемент = старый элемент - (элемент этой строки в опорном столбце * элемент в этом столбце опорной строки)
                    }
                }
            }
            //
            //Пересчет опорного столбца
            //
            for (int r = 0; r < condition_form.Fraction_StartMatrix.Count; r++)
            {
                if (r != rowMin)
                {
                    condition_form.Fraction_StartMatrix[r][columnMin] *= -condition_form.Fraction_StartMatrix[rowMin][columnMin]; //Элемент в опорном столбце = -опорный элемент * текущий элемент
                }
            }
            //
            //Проверка матрицы
            //

            if (flag == 1)
            {
                //
                //Удаление измененного индекса
                //
                foreach (var c in condition_form.ListOfIndex)
                {
                    c.RemoveAt(columnMin + 1);
                }
                //
                //Удаление столбца из матрицы
                //
                foreach (var c in condition_form.Fraction_StartMatrix)
                {
                    c.RemoveAt(columnMin);
                }
            }
        }

        //Обновление DGV
        private void UpdateDGV(int ColSupport, int RowSupport, int flag, Condition condition_form)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left; //Привязка к центральной части формы
            dataGridView.BorderStyle = BorderStyle.FixedSingle; //Установка стиля 
            dataGridView.Size = new Size(935, 379); //Установка размера 
            dataGridView.MinimumSize = new Size(935, 379); //Установка минимального размера
            dataGridView.MaximumSize = new Size(935, 379); //Установка максимального размера
            dataGridView.Location = new Point(0, 0); //Установка позиции

            dataGridView.AllowUserToResizeColumns = false; //Запрет на изменение пользователем размера столбцов
            dataGridView.ColumnHeadersDefaultCellStyle.Font = condition_form.HeaderFont; //Установка шрифта для заголовка столбцов
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в заголовках столбцов
            dataGridView.ColumnHeadersHeight = 30; //Высота заголовка столбцов

            dataGridView.RowHeadersVisible = true; //Видимость заголовка строк
            dataGridView.AllowUserToAddRows = false; //Запрет на добавление строк пользователем
            dataGridView.AllowUserToResizeRows = false; //Запрет на изменение пользователем размера строк
            dataGridView.AllowUserToDeleteRows = false; //Запрет на удаление строк пользователем
            dataGridView.RowHeadersDefaultCellStyle.Font = condition_form.HeaderFont; //Установка шрифта для заголовка строк
            dataGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в заголовках строк
            dataGridView.RowHeadersWidth = 85; //Ширина заголовка строк
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing; //Запрет на изменение ширины заголовка строк

            dataGridView.DefaultCellStyle.Font = condition_form.CellsFont; //Установка шрифта для ячеек
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в ячейках
            dataGridView.ReadOnly = true; //Запрет на изменение ячеек
            dataGridView.MultiSelect = false; //Запрет на выделение нескольких ячеек
            //
            //Убрать выделение ячеек
            //
            dataGridView.DefaultCellStyle.SelectionBackColor = dataGridView.DefaultCellStyle.BackColor;
            dataGridView.DefaultCellStyle.SelectionForeColor = dataGridView.DefaultCellStyle.ForeColor;

            if (condition_form.IsDecimal)
            {
                for (int c = 0; c < condition_form.StartMatrix[0].Count - 2; c++)
                {
                    //Добавление столбцов для переменных
                    var VariableColumn = new DataGridViewColumn(); //Создание нового столбца
                    VariableColumn.HeaderText = "X" + condition_form.Variables[int.Parse(condition_form.ListOfIndex[0][c + 1]) - 1].ToString(); //Текст в заголовке
                    VariableColumn.Width = 50; //Ширина ячейки
                    VariableColumn.Frozen = true; //Фиксация столбца на своём месте
                    VariableColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
                    dataGridView.Columns.Add(VariableColumn); //Добавление столбца в DGV
                }
            }
            else if(!condition_form.IsDecimal)
            {
                for (int c = 0; c < condition_form.Fraction_StartMatrix[0].Count - 1; c++)
                {
                    //Добавление столбцов для переменных
                    var VariableColumn = new DataGridViewColumn(); //Создание нового столбца
                    VariableColumn.HeaderText = "X" + condition_form.Variables[int.Parse(condition_form.ListOfIndex[0][c + 1]) - 1].ToString(); //Текст в заголовке
                    VariableColumn.Width = 50; //Ширина ячейки
                    VariableColumn.Frozen = true; //Фиксация столбца на своём месте
                    VariableColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
                    dataGridView.Columns.Add(VariableColumn); //Добавление столбца в DGV
                }
            }
            //
            //Добавление столбца со значением ограничений
            //
            var BColumn = new DataGridViewColumn(); //Создание нового стобца
            BColumn.HeaderText = "b"; //Текст в заголовке
            BColumn.Width = 50; //Ширина столбца
            BColumn.Frozen = true; //Фиксация столбца на своём месте
            BColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
            dataGridView.Columns.Add(BColumn); //Добавление столбца в DGV
            //
            //Добавление количества строк (ограничений)
            //
            for (int r = 0; r < condition_form.RestrictionsCounter + 1; r++)
            {
                dataGridView.Rows.Add();
                if (r != condition_form.RestrictionsCounter)
                {
                    dataGridView.Rows[r].HeaderCell.Value = "X" + condition_form.Variables[int.Parse(condition_form.ListOfIndex[r + 1][0]) - 1];
                }
            }
            dataGridView.ClearSelection(); //Очистить выделение ячейки
            //
            //Заполнение DGV новыми значениями
            //
            if (condition_form.IsDecimal)
            {
                for (int r = 0; r < condition_form.StartMatrix.Count; r++)
                {
                    for (int c = 0; c < condition_form.StartMatrix[0].Count - 1; c++)
                    {
                        if (c != condition_form.StartMatrix[0].Count - 2)
                        {
                            dataGridView[c, r].Value = Math.Round(double.Parse(condition_form.StartMatrix[r][c]), 2);
                        }
                        else
                        {
                            dataGridView[c, r].Value = Math.Round(double.Parse(condition_form.StartMatrix[r][c + 1]), 2);
                        }
                    }
                }
            }
            else if (!condition_form.IsDecimal)
            {
                for (int r = 0; r < condition_form.Fraction_StartMatrix.Count; r++)
                {
                    for (int c = 0; c < condition_form.Fraction_StartMatrix[0].Count; c++)
                    {
                        dataGridView[c, r].Value = condition_form.Fraction_StartMatrix[r][c];
                    }
                }
            }

            if (flag == 1) //Построение DGV с выделением удаляющегося столбца
            {
                dataGridView.Columns[ColSupport].DefaultCellStyle.BackColor = Color.OrangeRed;
            }
            else if (flag == 2) //Построение DGV с выделением опорного элементов
            {

                dataGridView[ColSupport, RowSupport].Style.BackColor = Color.LightGreen;
            }
            else if (flag == 3) //Построение DGV с выделением опорного(-ых) элементов
            {
                for (int e = 0; e < columnMinLst.Count; e++)
                {
                    dataGridView[columnMinLst[e], rowMinLst[e]].Style.BackColor = Color.LightGreen;
                }
            }
            dataGridView.Update();
        }
        //Выбор базисного элемента пользователем
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int CountSelected = ((DataGridView)sender).GetCellCount(DataGridViewElementStates.Selected); //Количество выбранных ячеек
            int isBasis = 0; //Флаг на нахождение опорного элемента

            for (int i = 0; i < CountSelected; i++)
            {
                if (isBasis == 0 && ((DataGridView)sender).SelectedCells[i].Style.BackColor == Color.LightGreen)
                {
                    BasisColumn = ((DataGridView)sender).SelectedCells[i].ColumnIndex; //текущий базис == выбранный пользователем элемент
                    BasisRow = ((DataGridView)sender).SelectedCells[i].RowIndex; //текущий базис == выбранный пользователем элемент
                    Button_StepForward.Enabled = true; //Включить кнопку если выбран опорный элемент
                    ((DataGridView)sender).DefaultCellStyle.SelectionBackColor = Color.Yellow; //Подсветить выбранный элемент
                    isBasis = 1;
                }
                else
                {
                    ((DataGridView)sender).DefaultCellStyle.SelectionBackColor = ((DataGridView)sender).DefaultCellStyle.BackColor;
                    Button_StepForward.Enabled = false; //Отключить кнопку, если выбран не опорный элемент
                }
            }
        }
        //Сохранение текущих данных
        private void SaveDataToLst(Condition condition_form)
        {
            if (condition_form.IsDecimal)
            {
                //Сохранение основной матрицы
                List<List<string>> addMatrix = new List<List<string>>();
                for (int r = 0; r < condition_form.StartMatrix.Count; r++)
                {
                    List<string> row = new List<string>();
                    for (int c = 0; c < condition_form.StartMatrix[r].Count; c++)
                    {
                        row.Add(condition_form.StartMatrix[r][c]);
                    }
                    addMatrix.Add(row);
                }
                saveMatrixSBS.Add(addMatrix);
            }else if (!condition_form.IsDecimal)
            {
                //Сохранение основной матрицы
                List<List<Fraction>> addMatrix = new List<List<Fraction>>();
                for (int r = 0; r < condition_form.Fraction_StartMatrix.Count; r++)
                {
                    List<Fraction> row = new List<Fraction>();
                    for (int c = 0; c < condition_form.Fraction_StartMatrix[r].Count; c++)
                    {
                        row.Add(condition_form.Fraction_StartMatrix[r][c]);
                    }
                    addMatrix.Add(row);
                }
                saveFractionMatrixSBS.Add(addMatrix);
            }

            //Сохранение матрицы индексов
            List<List<string>> addMatrixIndex = new List<List<string>>();
            for (int r = 0; r < condition_form.ListOfIndex.Count; r++)
            {
                List<string> row = new List<string>();
                for (int c = 0; c < condition_form.ListOfIndex[r].Count; c++)
                {
                    row.Add(condition_form.ListOfIndex[r][c]);
                }
                addMatrixIndex.Add(row);
            }
            saveIndexSBS.Add(addMatrixIndex);

            //Сохранение списка проверенных строк
            List<int> addRow = new List<int>();
            for (int e = 0; e < ValueRow.Count; e++)
            {
                addRow.Add(ValueRow[e]);
            }
            saveValueRow.Add(addRow);

            //Сохранение списка базисных строк
            List<int> addMinRow = new List<int>();
            for (int e = 0; e < rowMinLst.Count; e++)
            {
                addMinRow.Add(rowMinLst[e]);
            }
            saveRowMin.Add(addMinRow);

            //Сохранение списка базисных столбцов
            List<int> addMinColumn = new List<int>();
            for (int e = 0; e < columnMinLst.Count; e++)
            {
                addMinColumn.Add(columnMinLst[e]);
            }
            saveColumnMin.Add(addMinColumn);
        }

        //Функция, пересчитывающая переменные для "второго шага"
        public void ReCalculation(Condition condition_form)
        {
            List<string> Function = new List<string>(condition_form.Target_function); //Список для вычислений подобных слагаемых
            //
            //Отсеивание неиспользуемых переменных
            //
            for (int i = 1; i < condition_form.ListOfIndex[0].Count; i++)
            {
                for (int r = 0; r < Function.Count; r++)
                {
                    if (r == int.Parse(condition_form.ListOfIndex[0][i]) - 1)
                    {
                        Function[r] += "*x" + condition_form.ListOfIndex[0][i];
                    }
                }
            }
            //
            //Выражение базисных переменных и подстановка в начальную целевую функцию (x1 = b - 2x2...)
            //
            if (condition_form.IsDecimal)
            {
                for (int r = 0; r < condition_form.StartMatrix.Count - 1; r++)
                {
                    for (int c = 0; c < condition_form.Target_function.Count; c++)
                    {
                        if (c + 1 == int.Parse(condition_form.ListOfIndex[r + 1][0])) //с == индексу переменной
                        {
                            Function[c] = (double.Parse(condition_form.Target_function[c]) * double.Parse(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1])).ToString(); //Новый элемент = коэффициент элемента в целевой функции * элемент поледнего столбца текущей строки в матрице
                            for (int j = 0; j < condition_form.StartMatrix[0].Count - 1; j++)
                            {
                                if (j != condition_form.StartMatrix[0].Count - 2)
                                {
                                    Function[c] += " " + (-1 * double.Parse(condition_form.Target_function[c]) * double.Parse(condition_form.StartMatrix[r][j])).ToString() + "*x" + condition_form.ListOfIndex[0][j + 1]; //Из пересчитанной суммы - коэффициент целевой функции * текущий элемент (в конце указан индекс переменной)
                                }
                            }
                        }
                    }
                }
                //
                //Обнуление строки сумм в матрице для записи пересчитанных значений
                //
                for (int i = 0; i < condition_form.StartMatrix[0].Count; i++)
                {
                    if (i != condition_form.StartMatrix[0].Count - 2)
                    {
                        condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][i] = "0";
                    }
                }
                //
                //Приведение подобных слагаемых
                //
                for (int i = 0; i < Function.Count; i++)
                {
                    for (int j = 0; j < Function[i].Split().Count(); j++)
                    {
                        if (!Function[i].Split()[j].Contains("x")) //Если в элементе нету неизвестной переменной
                        {
                            condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][condition_form.StartMatrix[0].Count - 1] = (Math.Round(double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][condition_form.StartMatrix[0].Count - 1]) + double.Parse(Function[i].Split()[j]), 4)).ToString(); //Правый нижний элемент = правый нижний элемент + элемент без неизвестных переменных
                        }
                        else //Если в элементе есть неизвестная переменная
                        {
                            for (int c = 0; c < condition_form.ListOfIndex[0].Count; c++)
                            {
                                if (Function[i].Split()[j].Split('*')[1].Contains(condition_form.ListOfIndex[0][c].ToString())) //Ищем индекс переменной
                                {
                                    condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][c - 1] = (Math.Round(double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][c - 1]) + double.Parse(Function[i].Split()[j].Split('*')[0]), 4)).ToString(); //Значение в последней строке полученной переменной = элемент последней строки полученной переменной + элемент с полученной переменной
                                }
                            }
                        }
                    }
                }
                condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][condition_form.StartMatrix[0].Count - 1] = (double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][condition_form.StartMatrix[0].Count - 1]) * -1).ToString(); //Инверсия последнего 
            }
            else if (!condition_form.IsDecimal)
            {
                for (int r = 0; r < condition_form.Fraction_StartMatrix.Count - 1; r++)
                {
                    for (int c = 0; c < condition_form.Target_function.Count; c++)
                    {
                        if (c + 1 == int.Parse(condition_form.ListOfIndex[r + 1][0])) //с == индексу переменной
                        {
                            Function[c] = (int.Parse(condition_form.Target_function[c]) * condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1]).ToString(); //Новый элемент = коэффициент элемента в целевой функции * элемент поледнего столбца текущей строки в матрице
                            for (int j = 0; j < condition_form.Fraction_StartMatrix[0].Count - 1; j++)
                            {
                                Function[c] += " " + (-1 * int.Parse(condition_form.Target_function[c]) * condition_form.Fraction_StartMatrix[r][j]).ToString() + "*x" + condition_form.ListOfIndex[0][j + 1]; //Из пересчитанной суммы - коэффициент целевой функции * текущий элемент (в конце указан индекс переменной)
                            }
                        }
                    }
                }
                //
                //Обнуление строки сумм в матрице для записи пересчитанных значений
                //
                for (int i = 0; i < condition_form.Fraction_StartMatrix[0].Count; i++)
                {
                    condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][i] = new Fraction(0);
                }
                //
                //Приведение подобных слагаемых
                //
                for (int i = 0; i < Function.Count; i++)
                {
                    for (int j = 0; j < Function[i].Split().Count(); j++)
                    {
                        if (!Function[i].Split()[j].Contains("x")) //Если в элементе нету неизвестной переменной
                        {
                            if (Function[i].Split()[j].Split('/').Count() == 1)
                            {
                                condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][condition_form.Fraction_StartMatrix[0].Count - 1] += int.Parse(Function[i].Split()[j]); //Правый нижний элемент = правый нижний элемент + элемент без неизвестных переменных
                            }
                            else if (Function[i].Split()[j].Split('/').Count() == 2)
                            {
                                condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][condition_form.Fraction_StartMatrix[0].Count - 1] += new Fraction(int.Parse(Function[i].Split()[j].Split('/')[0]), int.Parse(Function[i].Split()[j].Split('/')[1])); //Правый нижний элемент = правый нижний элемент + элемент без неизвестных переменных
                            }
                        }
                        else //Если в элементе есть неизвестная переменная
                        {
                            for (int c = 0; c < condition_form.ListOfIndex[0].Count; c++)
                            {
                                if (Function[i].Split()[j].Split('*')[1].Contains(condition_form.ListOfIndex[0][c].ToString())) //Ищем индекс переменной
                                {
                                    if (Function[i].Split()[j].Split('*')[0].Split('/').Count() == 2)
                                    {
                                        condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][c - 1] += new Fraction(int.Parse(Function[i].Split()[j].Split('*')[0].Split('/')[0]), int.Parse(Function[i].Split()[j].Split('*')[0].Split('/')[1])); //Значение в последней строке полученной переменной = элемент последней строки полученной переменной + элемент с полученной переменной
                                    }
                                    else if (Function[i].Split()[j].Split('*')[0].Split('/').Count() == 1)
                                    {
                                        condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][c - 1] += new Fraction(int.Parse(Function[i].Split()[j].Split('*')[0])); //Значение в последней строке полученной переменной = элемент последней строки полученной переменной + элемент с полученной переменной
                                    }
                                }
                            }
                        }
                    }
                }
                condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][condition_form.Fraction_StartMatrix[0].Count - 1] = -condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][condition_form.Fraction_StartMatrix[0].Count - 1]; //Инверсия последнего элемента
            }
            ValueRow.Clear(); //Очистка списка строк исключения
        }
        //Поиск минимума
        private void MinFind(Condition condition_form)
        {
            //
            //Поиск опорного элемента (базисного)
            //
            int columnMin = 0; //индекс опроного столбца
            int rowMin = 0; //Индекс опорной строки
            columnMinLst.Clear(); //Очистка списка индесов опорных столбцов
            rowMinLst.Clear(); //Очистка списка индексов опорных строк
            if (CounterIteration == 0)
            {
                if (condition_form.IsDecimal)
                {
                    for (int l = 0; l < condition_form.StartMatrix[0].Count - 1; l++)
                    {
                        bool flag = false;
                        columnMin = 0;
                        rowMin = 0;
                        for (int c = 0; c < condition_form.StartMatrix[0].Count - 1; c++)
                        {
                            double min = 999999999.0; //Минимальный элемент

                            for (int r = 0; r < condition_form.StartMatrix.Count - 1; r++)
                            {
                                if (condition_form.StartMatrix[r][c] != "0" && c != condition_form.StartMatrix[0].Count - 2 && !ValueRow.Contains(r) && double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][c]) < 0.0) //Элемент не принаджлежит столбцу "знаки" && эта строка ранее не проверялась
                                {
                                    if (Convert.ToDouble(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1]) / Convert.ToDouble(condition_form.StartMatrix[r][c]) < min) //(Последний элемент / текущий элемент) < текущий минимум?
                                    {
                                        min = Math.Round(Convert.ToDouble(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1]) / Convert.ToDouble(condition_form.StartMatrix[r][c]), 4);
                                        rowMin = r;
                                        columnMin = c;
                                        flag = true;
                                    }
                                }
                            }
                            if (flag)
                            {
                                columnMinLst.Add(columnMin);
                                rowMinLst.Add(rowMin);
                            }
                        }
                    }
                }else if (!condition_form.IsDecimal)
                {
                    for (int l = 0; l < condition_form.Fraction_StartMatrix[0].Count - 1; l++)
                    {
                        bool flag = false;
                        columnMin = 0;
                        rowMin = 0;
                        for (int c = 0; c < condition_form.Fraction_StartMatrix[0].Count - 1; c++)
                        {
                            Fraction min = new Fraction(9999); //Минимальный элемент

                            for (int r = 0; r < condition_form.Fraction_StartMatrix.Count - 1; r++)
                            {
                                if (condition_form.Fraction_StartMatrix[r][c] != new Fraction(0) && !ValueRow.Contains(r) && condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][c] < 0) //эта строка ранее не проверялась
                                {
                                    if (condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1] / condition_form.Fraction_StartMatrix[r][c] < min) //(Последний элемент / текущий элемент) < текущий минимум?
                                    {
                                        min = condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1] / condition_form.Fraction_StartMatrix[r][c];
                                        rowMin = r;
                                        columnMin = c;
                                        flag= true;
                                    }
                                }
                            }
                            if (flag)
                            {
                                columnMinLst.Add(columnMin);
                                rowMinLst.Add(rowMin);
                            }
                        }
                    }
                }
            }
            else if (CounterIteration != 0)
            {
                if (condition_form.IsDecimal)
                {
                    columnMin = -1;
                    rowMin = -1;
                    for (int c = 0; c < condition_form.StartMatrix[0].Count - 1; c++)
                    {
                        double min = 999999999.0; //Минимальный элемент
                        for (int r = 0; r < condition_form.StartMatrix.Count - 1; r++)
                        {
                            if (condition_form.StartMatrix[r][c] != "0" && c != condition_form.StartMatrix[0].Count - 2 && !ValueRow.Contains(r) && double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][c]) < 0) //Элемент не принаджлежит столбцу "знаки" && эта строка ранее не проверялась
                            {
                                if (Convert.ToDouble(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1]) / Convert.ToDouble(condition_form.StartMatrix[r][c]) < min) //(Последний элемент / текущий элемент) < текущий минимум?
                                {
                                    min = Math.Round(Convert.ToDouble(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1]) / Convert.ToDouble(condition_form.StartMatrix[r][c]), 4);
                                    rowMin = r;
                                    columnMin = c;
                                }
                            }
                        }
                        if (columnMin != -1 && rowMin != -1)
                        {
                            columnMinLst.Add(columnMin);
                            rowMinLst.Add(rowMin);
                        }
                    }
                }
                else if (!condition_form.IsDecimal)
                {
                    columnMin = -1;
                    rowMin = -1;
                    for (int c = 0; c < condition_form.Fraction_StartMatrix[0].Count - 1; c++)
                    {
                        Fraction min = new Fraction(9999); //Минимальный элемент
                        for (int r = 0; r < condition_form.Fraction_StartMatrix.Count - 1; r++)
                        {
                            if (condition_form.Fraction_StartMatrix[r][c] != 0 && !ValueRow.Contains(r) && condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][c] < 0) //Элемент не принаджлежит столбцу "знаки" && эта строка ранее не проверялась
                            {
                                if (condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1] / condition_form.Fraction_StartMatrix[r][c] < min) //(Последний элемент / текущий элемент) < текущий минимум?
                                {
                                    min = condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1] / condition_form.Fraction_StartMatrix[r][c];
                                    rowMin = r;
                                    columnMin = c;
                                }
                            }
                        }
                        if (columnMin != -1 && rowMin != -1)
                        {
                            columnMinLst.Add(columnMin);
                            rowMinLst.Add(rowMin);
                        }
                    }
                }
            }
            if (columnMin != -1 && rowMin != -1)
            {
                BasisColumn = columnMinLst[0];
                BasisRow = rowMinLst[0];
                ValueRow.Add(BasisRow);
            }
            UpdateDGV(columnMin, rowMin, 3, condition_form);
        }

        //
        //Ответы
        //
        //Функция, выводящая ответ для десятичных дробей
        public void Answer(int x_cord, int y_cord, Condition condition_form)
        {
            //
            //Вывод ответа
            //
            string answer = ""; //Строка ответа
            string answerRound = ""; //Строка ответа с округлением
            for (int x = 1; x < condition_form.VariablesCounter + 1; x++)
            {
                int flag = 0; //Флаг для одноразовой записи
                for (int r = 0; r < condition_form.StartMatrix.Count - 1; r++)
                {
                    if (flag == 0)
                    {
                        if (int.Parse(condition_form.ListOfIndex[r + 1][0]) == x) //х == существующему индексу
                        {
                            if (x == condition_form.VariablesCounter)
                            {
                                answer += condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1];
                                answerRound += Math.Round(double.Parse(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1]), 0);
                            }
                            else
                            {
                                answer += condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1] + "; ";
                                answerRound += Math.Round(double.Parse(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1]), 0).ToString() + "; ";
                            }
                            flag = 1;
                        }
                    }
                }
                if (flag == 0) //Если значение переменной не было записано раньше
                {
                    if (x == condition_form.VariablesCounter)
                    {
                        answer += "0";
                        answerRound += "0";
                    }
                    else
                    {
                        answer += "0; ";
                        answerRound += "0; ";
                    }
                }
            }
            //
            //Создание Label с полученным ответом
            //
            for (int i = 0; i < 6; i++)
            {
                Label AnswerLabel = new Label(); //Создание нового Label
                AnswerLabel.AutoEllipsis = false;
                AnswerLabel.AutoSize = true; //Отображать весь текст Label
                AnswerLabel.Font = condition_form.HeaderFont; //Утановка шрифта
                AnswerLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left; //Привязка к центральной части формы
                if (i == 0) //Надпись "Ответ без округления"
                {
                    AnswerLabel.Text = "Ответ без округления:"; //Присваивание текста (ответа)
                    AnswerLabel.Location = new Point(1000, y_cord); //Точка отрисовка
                }
                else if (i == 1) //Надпись Х*
                {
                    AnswerLabel.Text = "X \u20F0 = (" + answer + ")"; //Присваивание текста (ответа)
                    AnswerLabel.Location = new Point(x_cord, y_cord); //Точка отрисовка
                }
                else if (i == 2) //Надпись F*
                {
                    if (condition_form.IsMin)
                    {
                        AnswerLabel.Text = "F \u20F0 = " + (-1 * double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][condition_form.StartMatrix[0].Count - 1])).ToString(); //Изменение знака, если задача на минимизацию
                    }
                    else
                    {
                        AnswerLabel.Text = "F \u20F0 = " + condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][condition_form.StartMatrix[0].Count - 1];
                    }
                    AnswerLabel.Location = new Point(x_cord, y_cord); //Точка отрисовка
                }
                else if (i == 3) //Надпись "Ответ с округлением"
                {
                    AnswerLabel.Text = "Округлённый ответ:"; //Присваивание текста (ответа)
                    AnswerLabel.Location = new Point(x_cord, y_cord); //Точка отрисовка
                }
                else if (i == 4) //Надпись X* (округлённая)
                {
                    AnswerLabel.Text = "X \u20F0 = (" + answerRound + ")"; //Присваивание текста (ответа)
                    AnswerLabel.Location = new Point(x_cord, y_cord); //Точка отрисовка
                }
                else if (i == 5) //Надпись F* (округлённая)
                {
                    if (condition_form.IsMin)
                    {
                        AnswerLabel.Text = "F \u20F0 = " + Math.Round(-1 * double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][condition_form.StartMatrix[0].Count - 1]), 0).ToString(); //Изменение знака, если задача на минимизацию
                    }
                    else
                    {
                        AnswerLabel.Text = "F \u20F0 = " + Math.Round(double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][condition_form.StartMatrix[0].Count - 1]), 0);
                    }
                    AnswerLabel.Location = new Point(x_cord, y_cord); //Точка отрисовка
                }
                y_cord += 20;
                splitContainer.Panel1.Controls.Add(AnswerLabel); //Добавление Label на форму
                IsCalculated= true;
            }
        }
        //Функция, выводящая ответ для натуральных дробей
        public void Fraction_Answer(int x_cord, int y_cord, Condition condition_form)
        {
            //
            //Вывод ответа
            //
            string answer = ""; //Строка ответа
            for (int x = 1; x < condition_form.VariablesCounter + 1; x++)
            {
                int flag = 0; //Флаг для одноразовой записи
                for (int r = 0; r < condition_form.Fraction_StartMatrix.Count - 1; r++)
                {
                    if (flag == 0)
                    {
                        if (int.Parse(condition_form.ListOfIndex[r + 1][0]) == x) //х == существующему индексу
                        {
                            if (x == condition_form.VariablesCounter)
                            {
                                answer += condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1].ToString();
                            }
                            else
                            {
                                answer += condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1].ToString() + "; ";
                            }
                            flag = 1;
                        }
                    }
                }
                if (flag == 0) //Если значение переменной не было записано раньше
                {
                    if (x == condition_form.VariablesCounter)
                    {
                        answer += "0";
                    }
                    else
                    {
                        answer += "0; ";
                    }
                }
            }
            //
            //Создание Label с полученным ответом
            //
            for (int i = 0; i < 3; i++)
            {
                Label AnswerLabel = new Label(); //Создание нового Label
                AnswerLabel.AutoEllipsis = false;
                AnswerLabel.AutoSize = true; //Отображать весь текст Label
                AnswerLabel.Font = condition_form.HeaderFont; //Утановка шрифта
                AnswerLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left; //Привязка к центральной части формы
                if (i == 0) //Надпись "Ответ"
                {
                    AnswerLabel.Text = "Ответ :"; //Присваивание текста (ответа)
                    AnswerLabel.Location = new Point(1000, y_cord); //Точка отрисовка
                }
                else if (i == 1) //Надпись Х*
                {
                    AnswerLabel.Text = "X \u20F0 = (" + answer + ")"; //Присваивание текста (ответа)
                    AnswerLabel.Location = new Point(x_cord, y_cord); //Точка отрисовка
                }
                else if (i == 2) //Надпись F*
                {
                    if (condition_form.IsMin)
                    {
                        AnswerLabel.Text = "F \u20F0 = " + (-condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][condition_form.Fraction_StartMatrix[0].Count - 1]).ToString(); //Изменение знака, если задача на минимизацию
                    }
                    else
                    {
                        AnswerLabel.Text = "F \u20F0 = " + condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][condition_form.Fraction_StartMatrix[0].Count - 1].ToString();
                    }
                    AnswerLabel.Location = new Point(x_cord, y_cord); //Точка отрисовка
                }
                y_cord += 20;
                splitContainer.Panel1.Controls.Add(AnswerLabel); //Добавление Label на форму
            }
            IsCalculated = true;
        }
        public void CheckSteps()
        {
            // Отключение кнопки шага назад если некуда возвращаться
            if (CounterIteration < 2)
            {
                Button_StepBack.Enabled = false;
            }
            else
            {
                Button_StepBack.Enabled = true;
            }
        }
    }
}
