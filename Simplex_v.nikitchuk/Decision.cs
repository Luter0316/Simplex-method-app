using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Simplex_v.nikitchuk
{
    public partial class Decision : Form
    {
        private void Decision_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Стиль границ формы
            this.Size = new Size(1280, 525); //Размер формы
            this.Text = "Автоматический метод искуственного базиса"; //Заголовок формы
        }

        public Decision(Condition f)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; //Открыть форму по центру экрана
            artificialbasis(f); //Расчеты
        }

        //Цикл автоматического метода искуственного базиса для десятичных дробей + вывод ответа
        private void artificialbasis(Condition s)
        {
            if (s.IsDecimal)
            {
                while (s.MainStepCheck()) //Расчет "первого шага", пока в суммирущей строке начальной матрицы не все элементы == 0
                {
                    Calculation(1, s);
                }
                if (s.solvability())
                {
                    ReCalculation(s);
                    while (s.SecondStepCheck())
                    {
                        Calculation(0, s);
                    }
                    Answer(1000, 15, s);
                }
            }
            else if (!s.IsDecimal)
            {
                while (s.MainStepCheck()) //Расчет "первого шага", пока в суммирущей строке начальной матрицы не все элементы == 0
                {
                    Fraction_Calculation(1, s);
                }
                if (s.solvability())
                {
                    ReCalculation(s);
                    while (s.SecondStepCheck())
                    {
                        Fraction_Calculation(0, s);
                    }
                    Fraction_Answer(1000, 15, s);
                }
            }
        }

        //
        //Функции расчета
        //
        //Функция, расчитывающая автоматический метод искуственного базиса для десятичных дробей
        public void Calculation(int flag, Condition condition_form)
        {
            //
            //Поиск опорного элемента (базисного)
            //
            int columnMin = 0; //индекс опроного столбца
            int rowMin = 0; //Индекс опорной строки
            int stopper = 0; //Поиск опорного элемента только в одном столбце
            double min = 999999999.0; //Минимальный элемент
            for (int c = 0; c < condition_form.StartMatrix[0].Count - 1; c++)
            {
                if (stopper == 0)
                {
                    for (int r = 0; r < condition_form.StartMatrix.Count - 1; r++)
                    {
                        if (condition_form.StartMatrix[r][c] != "0" && c != condition_form.StartMatrix[0].Count - 2 && !condition_form.ValueRow.Contains(r) && double.Parse(condition_form.StartMatrix[condition_form.StartMatrix.Count - 1][c]) < 0) //Элемент не принаджлежит столбцу "знаки" && эта строка ранее не проверялась
                        {
                            if (Convert.ToDouble(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1]) / Convert.ToDouble(condition_form.StartMatrix[r][c]) < min) //(Последний элемент / текущий элемент) < текущий минимум?
                            {
                                min = Math.Round(Convert.ToDouble(condition_form.StartMatrix[r][condition_form.StartMatrix[0].Count - 1]) / Convert.ToDouble(condition_form.StartMatrix[r][c]), 4);
                                rowMin = r;
                                columnMin = c;
                                stopper = 1;
                                condition_form.ValueRow.Add(rowMin);
                            }
                        }
                    }
                }
            }

            CreateDGV(columnMin, rowMin, 2, condition_form); //Создать DataGridView со стартовыми значениями и выделить базисный элемент
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
                CreateDGV(columnMin, rowMin, 1, condition_form); //Создать DataGridView с пересчитанными опроными строками и столбцами. Выделить столбец для удаления
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
            else
            {
                CreateDGV(columnMin, rowMin, 0, condition_form); //Создать DataGridView с пересчитанными опроными строками и столбцами. Не выделять столбец для удаления
            }
        }
        //Функция, расчитывающая автоматический метод искуственного базиса для натрульных дробей
        private void Fraction_Calculation(int flag, Condition condition_form)
        {
            //
            //Поиск опорного элемента (базисного)
            //
            int columnMin = 0; //индекс опроного столбца
            int rowMin = 0; //Индекс опорной строки
            int stopper = 0; //Поиск опорного элемента только в одном столбце
            Fraction min = new Fraction(9999); //Минимальный элемент
            for (int c = 0; c < condition_form.Fraction_StartMatrix[0].Count; c++)
            {
                if (stopper == 0)
                {
                    for (int r = 0; r < condition_form.Fraction_StartMatrix.Count - 1; r++)
                    {
                        if (condition_form.Fraction_StartMatrix[r][c] != 0 && !condition_form.ValueRow.Contains(r) && condition_form.Fraction_StartMatrix[condition_form.Fraction_StartMatrix.Count - 1][c] < 0) //Эта строка ранее не проверялась
                        {
                            if (condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1] / condition_form.Fraction_StartMatrix[r][c] < min) //(Последний элемент / текущий элемент) < текущий минимум?
                            {
                                min = condition_form.Fraction_StartMatrix[r][condition_form.Fraction_StartMatrix[0].Count - 1] / condition_form.Fraction_StartMatrix[r][c];
                                rowMin = r;
                                columnMin = c;
                                stopper = 1;
                                condition_form.ValueRow.Add(rowMin);
                            }
                        }
                    }
                }
            }

            CreateDGV(columnMin, rowMin, 2, condition_form); //Создать DataGridView со стартовыми значениями и выделить базисный элемент
            //
            //Изменение индексов переменных
            //
            var tmp = condition_form.ListOfIndex[rowMin + 1][0];
            condition_form.ListOfIndex[rowMin + 1][0] = condition_form.ListOfIndex[0][columnMin + 1];
            condition_form.ListOfIndex[0][columnMin + 1] = tmp;

            if (condition_form.Fraction_StartMatrix[rowMin][columnMin].numerator != 0)
            {
                condition_form.Fraction_StartMatrix[rowMin][columnMin] = condition_form.Fraction_StartMatrix[rowMin][columnMin].GetReverse(); //Пересчет опорного элемента
            }
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

            if (flag == 1)
            {
                CreateDGV(columnMin, rowMin, 1, condition_form); //Создать DataGridView с пересчитанными опроными строками и столбцами. Выделить столбец для удаления
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
            else
            {
                CreateDGV(columnMin, rowMin, 0, condition_form); //Создать DataGridView с пересчитанными опроными строками и столбцами. Не выделять столбец для удаления
            }
        }
        

        //Создание нового DGV
        private void CreateDGV(int ColSupport, int RowSupport, int flag, Condition condition_form)
        {
            //
            //Создание DGV
            //
            int DGVShift = 379 * condition_form.CounterIteration; //Переменная сдвига DGV
            DataGridView dataGridView = new DataGridView(); //Создание новго DGV

            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left; //Привязка к центральной части формы
            dataGridView.BorderStyle = BorderStyle.FixedSingle; //Установка стиля границ

            dataGridView.Size = new Size(935, 379); //Установка размера 
            dataGridView.MinimumSize = new Size(935, 379); //Установка минимального размера
            dataGridView.MaximumSize = new Size(935, 379); //Установка максимального размера
            dataGridView.Location = new Point(0, DGVShift); //Установка позиции

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
            //
            //Убрать выделение ячеек
            //
            dataGridView.DefaultCellStyle.SelectionBackColor = dataGridView.DefaultCellStyle.BackColor;
            dataGridView.DefaultCellStyle.SelectionForeColor = dataGridView.DefaultCellStyle.ForeColor;

            //
            //Добавление количества стобцов (переменных)
            //
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
            else if (!condition_form.IsDecimal)
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
            this.Controls.Add(dataGridView); //Добавление DGV на форму
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
            dataGridView.Focus();
            if (flag == 0) //Построение DGV
            {

            }
            else if (flag == 1) //Построение DGV с выделением удаляющегося столбца
            {
                dataGridView.Columns[ColSupport].DefaultCellStyle.BackColor = Color.OrangeRed;
            }
            else if (flag == 2) //Построение DGV с выделением опорного элементов
            {

                dataGridView[ColSupport, RowSupport].Style.BackColor = Color.LightGreen;
            }
            else if (flag == 3) //Построение DGV с выделением опорного(-ых) элементов
            {
                for (int e = 0; e < condition_form.columnMinLst.Count; e++)
                {
                    dataGridView[condition_form.columnMinLst[e], condition_form.rowMinLst[e]].Style.BackColor = Color.LightGreen;
                }
            }
            else if (flag == 4) //Построение DGV с выделением удаляющегося столбца и опроных элементов
            {

            }
            condition_form.CounterIteration++; //Увеличение счётчика итераций
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
            CreateDGV(-2, -2, 0, condition_form); //Создание DGV с пересчитанной суммой строк
            condition_form.ValueRow.Clear(); //Очистка списка строк исключения
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
                this.Controls.Add(AnswerLabel); //Добавление Label на форму
                this.AutoScrollMargin = new System.Drawing.Size(0, 379 * (condition_form.CounterIteration - 0) - 62); //Высота прокрутки
            }
            this.ShowDialog();
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
                this.Controls.Add(AnswerLabel); //Добавление Label на форму
                this.AutoScrollMargin = new System.Drawing.Size(0, 379 * (condition_form.CounterIteration - 0) - 62); //Высота прокрутки
            }
            this.ShowDialog(); //Форма автоматического метода
        }
    }
}
