using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Simplex_v.nikitchuk
{
    public partial class Condition : Form
    {
        //Объявление переменных

        public Font HeaderFont = new Font("Montserrat", 10, FontStyle.Bold); //Шрифт заголовка для DGV
        public Font CellsFont = new Font("Montserrat", 10); //Шрифт текста для DGV
        public Font LabelFont = new Font("Montserrat ExtraBold;", 14, FontStyle.Bold); //Шрифт для Label
        public Font SelectFont = new Font("Montserrat", 12); //Шрифт для селекторов переменных и ограничений
        public Font RBFont = new Font("Montserrat", 10); //Шрифт для RadioButton

        public List<List<String>> StartMatrix = new List<List<String>>(); //Матрица для записи начальных значений в виде десятичных дробей
        public List<List<Fraction>> Fraction_StartMatrix = new List<List<Fraction>>(); //Матрица для записи начальных заначений в виде обыкновенных дробей
        public List<List<String>> ListOfIndex = new List<List<String>>(); //Список индексов матрицы

        public List<int> ValueRow = new List<int>(); //Список исключенных из поиска строк
        public List<int> columnMinLst = new List<int>(); //Список индексов столбцов опорных элементов
        public List<int> rowMinLst = new List<int>(); //Список индексов строк опорных элементов
        public List<string> Target_function = new List<string>(); //Список коэффициентов целевой функции
        public List<string> Variables = new List<string> //Список нижних индексов от 1 до 32
        {
                "\u2081", "\u2082", "\u2083", "\u2084", "\u2085", "\u2086", "\u2087", "\u2088", "\u2089", "\u2081\u2080", "\u2081\u2081", "\u2081\u2082", "\u2081\u2083", "\u2081\u2084", "\u2081\u2085", "\u2081\u2086", "\u2081\u2087", "\u2081\u2088", "\u2081\u2089", "\u2082\u2080", "\u2082\u2081", "\u2082\u2082", "\u2082\u2083", "\u2082\u2084", "\u2082\u2085", "\u2082\u2086", "\u2082\u2087", "\u2082\u2088", "\u2082\u2089", "\u2083\u2080", "\u2083\u2081", "\u2083\u2082"
        };

        public int CounterIteration; //Счётчик количества итераций при решении задачи
        public int RestrictionsCounter; //Количество ограничений для использования в другой форме
        public int VariablesCounter; //Количество переменных для использования в другой форме
        public int BasisColumn; //Переменная текущего базисного столбца
        public int BasisRow; //Переменная текущей базисной строки

        public bool IsDecimal; //Тип дробей
        public bool IsMin; //Минимизация или максимизация
        public bool Solution; //Разрешимость задачи

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Стиль границ формы
            this.Size = new Size(1330, 505); //Размер формы
            this.Text = "Метод искуственного базиса"; //Заголовок офрмы
            Program.FormWithCondition = this;
        }

        public Condition()
        {
            InitializeComponent();
            LoadComponents();
            StartPosition = FormStartPosition.CenterScreen; //Открыть форму по центру экрана
        }

        //Присваивание точек привязки и значений компонентам
        private void LoadComponents()
        {
            //Label "Количество переменных"
            LabelVariableCount.Anchor = AnchorStyles.Top | AnchorStyles.Left; //Привязка к левой-верхней части формы
            LabelVariableCount.AutoSize = true;  //Отображение всего текста полностью
            LabelVariableCount.Font = LabelFont;  //Установка шрифта
            LabelVariableCount.Location = new Point(12, 30); //Установка позиции

            //NumericUpDown выбора количесва переменных
            VariableCount.Anchor = AnchorStyles.Top | AnchorStyles.Left; //Привязка к левой-верхней части формы
            VariableCount.AutoSize = true; //Отображение всего текста полностью
            VariableCount.BorderStyle = BorderStyle.FixedSingle; //Установка стиля границ
            VariableCount.Font = SelectFont; //Установка шрифта
            VariableCount.Location = new Point(277, 30); //Установка позиции
            VariableCount.Size = new Size(41, 31); //Установка размера
            VariableCount.Minimum = 1; //Установка минимального значения
            VariableCount.Maximum = 16; //Установка максимального значения
            VariableCount.Increment = 1; //Установка шага изменения
            VariableCount.Value = 3; //Установка стартового значения

            //Label "Количество ограничений"
            LabelRestrictionsCount.Anchor = AnchorStyles.Top | AnchorStyles.Left; //Привязка к левой-верхней части формы
            LabelRestrictionsCount.AutoSize = true; //Отображение всего текста полностью
            LabelRestrictionsCount.Font = LabelFont; //Установка шрифта
            LabelRestrictionsCount.Location = new Point(12, 60); //Установка позиции

            //NumericUpDown выбора количесва ограничений
            RestrictionsCount.Anchor = AnchorStyles.Top | AnchorStyles.Left; //Привязка к левой-верхней части формы
            RestrictionsCount.AutoSize = true; //Отображение всего текста полностью
            RestrictionsCount.BorderStyle = BorderStyle.FixedSingle; //Установка стиля границ
            RestrictionsCount.Font = SelectFont; //Установка шрифта
            RestrictionsCount.Location = new Point(277, 60); //Установка позиции
            RestrictionsCount.Size = new Size(41, 31); //Установка размера
            RestrictionsCount.Minimum = 1; //Установка минимального значения
            RestrictionsCount.Maximum = 16; //Установка максимального значения
            RestrictionsCount.Increment = 1; //Установка шага изменения
            RestrictionsCount.Value = 3; //Установка стартового значения

            //Значения radioButton "десятичные дроби"
            DecimalRB.Text = "Десятичные дроби"; //Кнопка ввода десятичных чисел
            DecimalRB.Font = RBFont; //Шрифт кнопки
            DecimalRB.Checked = true; //Отметить кнопку
            DecimalRB.Location = new Point(15, 85); //Установка позиции

            //Значения radioButton "Обыкновенные дроби"
            NaturalRB.Text = "Обыкновенные дроби"; //Кнопка ввода обыкновенных дробей
            NaturalRB.Font = RBFont; //Шрифт кнопки
            NaturalRB.Checked = false; //Не отметчать кнопку
            NaturalRB.Location = new Point(15, 105); //Установка позиции

            //Combo box выбора метода
            string[] type = { "Автоматический", "Пошаговый" };
            CBMethod.Items.AddRange(type); //Добавление списков элементов
            CBMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; //Только выбор из списка
            CBMethod.SelectedIndex = 0; //Стартовое значение
            CBMethod.Font = CellsFont; //Шрфит
        }

        //Создание dataGridView для целевой функции
        private void dataGridViewTargetFunctionLoad()
        {
            dataGridViewTargetFunction.Rows.Clear(); //Очистка всех строк
            dataGridViewTargetFunction.Columns.Clear(); //Очистка всех столбцов

            dataGridViewTargetFunction.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left; //Привязка к центральной части формы
            dataGridViewTargetFunction.BorderStyle = BorderStyle.None; //Установка стиля границ

            dataGridViewTargetFunction.Size = new Size(965, 49); //Установка размера
            dataGridViewTargetFunction.MinimumSize = new Size(965, 49); //Установка минимального размера
            dataGridViewTargetFunction.MaximumSize = new Size(965, 49); //Установка максимального размера
            dataGridViewTargetFunction.Location = new Point(338, 30); //Установка позиции

            dataGridViewTargetFunction.AllowUserToResizeColumns = false; //Запрет на изменение пользователем размера столбцов
            dataGridViewTargetFunction.ColumnHeadersDefaultCellStyle.Font = HeaderFont; //Установка шрифта для заголовка столбцов
            dataGridViewTargetFunction.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в заголовках столбцов
            dataGridViewTargetFunction.ColumnHeadersHeight = 30; //Высота заголовка столбцов

            dataGridViewTargetFunction.RowHeadersVisible = true; //Видимость заголовка строк
            dataGridViewTargetFunction.AllowUserToAddRows = false; //Запрет на добавление строк пользователем
            dataGridViewTargetFunction.AllowUserToResizeRows = false; //Запрет на изменение пользователем размера строк
            dataGridViewTargetFunction.AllowUserToDeleteRows = false; //Запрет на удаление строк пользователем
            dataGridViewTargetFunction.RowHeadersDefaultCellStyle.Font = HeaderFont; //Установка шрифта для заголовка строк
            dataGridViewTargetFunction.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в заголовках строк
            dataGridViewTargetFunction.RowHeadersWidth = 85; //Ширина заголовка строк
            dataGridViewTargetFunction.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing; //Запрет на изменение ширины заголовка строк

            dataGridViewTargetFunction.DefaultCellStyle.Font = CellsFont; //Установка шрифта для ячеек
            dataGridViewTargetFunction.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в ячейках
            dataGridViewTargetFunction.EditMode = DataGridViewEditMode.EditOnKeystroke; //Редактирование ячейки при нажатии на любую клавишу
            dataGridViewTargetFunction.MultiSelect = false; //Запрет на выделение нескольких ячеек

            //Добавление количества стобцов (переменных)
            for (int x = 0; x < VariableCount.Value; x++)
            {
                var RatioColumn = new DataGridViewColumn(); //Создание нового столбца
                RatioColumn.HeaderText = "C" + Variables[x].ToString(); //Текст в заголовке
                RatioColumn.Width = 50; //Ширина ячейки
                RatioColumn.ReadOnly = false; //Возможность редактирования пользователем
                RatioColumn.Frozen = true; //Фиксация столбца на своём месте
                RatioColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
                dataGridViewTargetFunction.Columns.Add(RatioColumn); //Добавление столбца в DGV
            }

            dataGridViewTargetFunction.Rows.Add(); //Добавление строки
            dataGridViewTargetFunction.Rows[0].HeaderCell.Value = "ƒ(x) ="; //Запись значения в заголовок строки

            //Добавление столбца "->"
            var ArrowColumn = new DataGridViewColumn(); //Создание нового стобца
            ArrowColumn.Width = 30; //Ширина столбца
            ArrowColumn.ReadOnly = true; //Запрет на редактирование пользователем
            ArrowColumn.Frozen = true; //Фиксация столбца на своём месте
            ArrowColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
            dataGridViewTargetFunction.Columns.Add(ArrowColumn); //Добавление столбца в DGV
            dataGridViewTargetFunction[((int)VariableCount.Value), 0].Value = "->"; //Запись значения в ячейку
            dataGridViewTargetFunction.Columns[(int)VariableCount.Value].DefaultCellStyle.Font = HeaderFont; //Шрифт столбца

            //Добавление столбца "min"/"max"
            var EndColumn = new DataGridViewColumn(); //Создание нового стобца
            EndColumn.Width = 50; //Ширина столбца
            EndColumn.ReadOnly = true; //Запрет на редактирование пользователем
            EndColumn.Frozen = true; //Фиксация столбца на своём месте
            EndColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
            dataGridViewTargetFunction.Columns.Add(EndColumn); //Добавление столбца в DGV
            dataGridViewTargetFunction[((int)VariableCount.Value) + 1, 0].Value = "min"; //Запись значения в ячейку
            dataGridViewTargetFunction.Columns[(int)VariableCount.Value + 1].DefaultCellStyle.Font = HeaderFont; //Шрифт столбца
        }

        //Создание dataGridView для ограничений
        private void dataGridViewRestrictionsLoad()
        {
            dataGridViewRestrictions.Rows.Clear(); //Очистка всех строк
            dataGridViewRestrictions.Columns.Clear(); //Очистка всех столбцов

            dataGridViewRestrictions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left; //Привязка к центральной части формы
            dataGridViewRestrictions.BorderStyle = BorderStyle.None; //Установка стиля границ

            dataGridViewRestrictions.Size = new Size(965, 380); //Установка размера
            dataGridViewRestrictions.MinimumSize = new Size(965, 380); //Установка минимального размера
            dataGridViewRestrictions.MaximumSize = new Size(965, 380); //Установка максимального размера
            dataGridViewRestrictions.Location = new Point(338, 78); //Установка позиции

            dataGridViewRestrictions.AllowUserToResizeColumns = false; //Запрет на изменение пользователем размера столбцов
            dataGridViewRestrictions.ColumnHeadersDefaultCellStyle.Font = HeaderFont; //Установка шрифта для заголовка столбцов
            dataGridViewRestrictions.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в заголовках столбцов
            dataGridViewRestrictions.ColumnHeadersHeight = 30; //Высота заголовка столбцов

            dataGridViewRestrictions.RowHeadersVisible = true; //Видимость заголовка строк
            dataGridViewRestrictions.AllowUserToAddRows = false; //Запрет на добавление строк пользователем
            dataGridViewRestrictions.AllowUserToResizeRows = false; //Запрет на изменение пользователем размера строк
            dataGridViewRestrictions.AllowUserToDeleteRows = false; //Запрет на удаление строк пользователем
            dataGridViewRestrictions.RowHeadersDefaultCellStyle.Font = HeaderFont; //Установка шрифта для заголовка строк
            dataGridViewRestrictions.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в заголовках строк
            dataGridViewRestrictions.RowHeadersWidth = 85; //Ширина заголовка строк
            dataGridViewRestrictions.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing; //Запрет на изменение ширины заголовка строк

            dataGridViewRestrictions.DefaultCellStyle.Font = CellsFont; //Установка шрифта для ячеек
            dataGridViewRestrictions.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Выравнивание по центру в ячейках
            dataGridViewRestrictions.EditMode = DataGridViewEditMode.EditOnKeystroke; //Редактирование ячейки при нажатии на любую клавишу
            dataGridViewRestrictions.MultiSelect = false; //Запрет на выделение нескольких ячеек

            //Добавление количества стобцов (переменных)
            for (int x = 0; x < VariableCount.Value; x++)
            {
                var RatioColumn = new DataGridViewColumn(); //Создание нового столбца
                RatioColumn.HeaderText = "C" + Variables[x].ToString(); //Текст в заголовке
                RatioColumn.Width = 50; //Ширина ячейки
                RatioColumn.ReadOnly = false; //Возможность редактирования пользователем
                RatioColumn.Frozen = true; //Фиксация столбца на своём месте
                RatioColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
                dataGridViewRestrictions.Columns.Add(RatioColumn); //Добавление столбца в DGV
            }

            //Добавление столбца "<="/"="/"=>"
            var ArrowColumn = new DataGridViewColumn(); //Создание нового стобца
            ArrowColumn.Width = 30; //Ширина столбца
            ArrowColumn.ReadOnly = true; //Запрет на редактирование пользователем
            ArrowColumn.Frozen = true; //Фиксация столбца на своём месте
            ArrowColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
            dataGridViewRestrictions.Columns.Add(ArrowColumn); //Добавление столбца в DGV
            dataGridViewRestrictions.Columns[(int)VariableCount.Value].DefaultCellStyle.Font = HeaderFont; //Шрифт столбца

            //Добавление столбца со значением ограничений
            var BColumn = new DataGridViewColumn(); //Создание нового стобца
            BColumn.Width = 50; //Ширина столбца
            BColumn.ReadOnly = false; //Запрет на редактирование пользователем
            BColumn.Frozen = true; //Фиксация столбца на своём месте
            BColumn.CellTemplate = new DataGridViewTextBoxCell(); //Стиль столбца
            dataGridViewRestrictions.Columns.Add(BColumn); //Добавление столбца в DGV

            //Добавление количества строк (ограничений)
            for (int y = 0; y < RestrictionsCount.Value; y++)
            {
                dataGridViewRestrictions.Rows.Add();
                dataGridViewRestrictions.Rows[y].HeaderCell.Value = "ƒ" + Variables[y] + "(x) =";
                dataGridViewRestrictions[((int)VariableCount.Value), y].Value = "=";
            }
        }

        //Селектор количества переменных
        private void VariableCount_ValueChanged(object sender, EventArgs e)
        {
            //Вызывает обновление DGV Целевой функции и ограничений
            dataGridViewTargetFunctionLoad();
            dataGridViewRestrictionsLoad();
        }

        //Селектор количества ограничений
        private void RestrictionsCount_ValueChanged(object sender, EventArgs e)
        {
            dataGridViewRestrictionsLoad(); //Вызывает обновление DGV ограничений
        }

        //Отмена визуального выделения ячеек в DGV целевой функции
        private void dataGridViewTargetFunction_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewTargetFunction.ClearSelection();
        }

        //Отмена визуального выделения ячеек в DGV ограничений
        private void dataGridViewRestrictions_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewRestrictions.ClearSelection();
        }

        //Изменение min на max и наоборот по нажатию на ячейку
        private void dataGridViewTargetFunction_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex + 1 == dataGridViewTargetFunction.ColumnCount && dataGridViewTargetFunction[((int)VariableCount.Value) + 1, 0].Value.ToString() == "min")
            {
                dataGridViewTargetFunction[((int)VariableCount.Value) + 1, 0].Value = "max";
            }
            else
            {
                if (e.ColumnIndex + 1 == dataGridViewTargetFunction.ColumnCount && dataGridViewTargetFunction[((int)VariableCount.Value) + 1, 0].Value.ToString() == "max")
                {
                    dataGridViewTargetFunction[((int)VariableCount.Value) + 1, 0].Value = "min";
                }
            }
        }

        //Изменения знаков у ограничений
        private void dataGridViewRestrictions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex + 2 == dataGridViewRestrictions.ColumnCount && dataGridViewRestrictions[((int)VariableCount.Value), e.RowIndex].Value.ToString() == "=")
            {
                dataGridViewRestrictions[((int)VariableCount.Value), e.RowIndex].Value = ">=";
            }
            else
            {
                if (e.ColumnIndex + 2 == dataGridViewRestrictions.ColumnCount && dataGridViewRestrictions[((int)VariableCount.Value), e.RowIndex].Value.ToString() == ">=")
                {
                    dataGridViewRestrictions[((int)VariableCount.Value), e.RowIndex].Value = "<=";
                }
                else
                {
                    if (e.ColumnIndex + 2 == dataGridViewRestrictions.ColumnCount && dataGridViewRestrictions[((int)VariableCount.Value), e.RowIndex].Value.ToString() == "<=")
                    {
                        dataGridViewRestrictions[((int)VariableCount.Value), e.RowIndex].Value = "=";
                    }
                }
            }
        }

        //Проверка на ввод числа в ячейку DGV целевой функции
        private void dataGridViewTargetFunction_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tx = e.Control as TextBox;
            tx.KeyPress += new KeyPressEventHandler(tx_KeyPress_int);
        }

        //Проверка на ввод числа в ячейку DGV ограничений
        private void dataGridViewRestrictions_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tx = e.Control as TextBox;
            tx.KeyPress += new KeyPressEventHandler(tx_KeyPress_int);
        }

        private void tx_KeyPress_int(object sender, KeyPressEventArgs e)
        {
            if (NaturalRB.Checked) //Натуральные дроби
            {
                if (!(char.IsNumber(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '-' || e.KeyChar == '/')) //Разрешён ввод цифр, "-", "/", и удаление
                {
                    e.Handled = true;
                }
            }
            else if (DecimalRB.Checked) //Десятичные дроби
            {
                if (!(char.IsNumber(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '-' || e.KeyChar == '.')) //Разрешён ввод цифр, "-", "." и удаление
                {
                    e.Handled = true;
                }
            }
        }

        //Чтение файла
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int RowCounterInFile = 0; //Счетчик количества строк в файле
                int ColumnCounterInFile = 0; //Счетчик количества столбцов в файле
                OpenFileDialog openFileDialog = new OpenFileDialog(); //Открытие файла
                openFileDialog.Filter = "Текстовый документ (*.txt)|*.txt"; //Фильтр текстовых файлов

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName; //Полный путь к файлу
                    RowCounterInFile = System.IO.File.ReadAllLines(filePath).Length;
                    RestrictionsCount.Value = RowCounterInFile - 1; //Установка ограничений

                    var fileStream = openFileDialog.OpenFile(); //Считываение содержимое файла в поток
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string fileContent = reader.ReadLine(); //Строка из файла
                        ColumnCounterInFile = fileContent.Substring(0, fileContent.Length - 1).Split('x').Length; //Количество коэффициентов в файле
                        VariableCount.Value = ColumnCounterInFile - 1; //Установка количества переменных
                        string[,] Ratio = new string[RowCounterInFile, ColumnCounterInFile + 1]; //Локальная матрица коэффициентов
                        for (int i = 0; i < RowCounterInFile; i++)
                        {
                            int Lsplit = 0; //Левая граница поиска коээффициентов в строке
                            int Rsplit = 0; //Правая граница поиска коэффициентов в строке
                            int flag = 0; //Флаг - был ли обнаружен Lsplit
                            int flag_find = 0; //Флаг - следующий элемент является последним в строке

                            //Чтение и преобразование коэффициентов
                            for (int j = 0; j < fileContent.Length - 1; j++)
                            {
                                if (flag == 0) //Если Lsplit не был обнаружен
                                {
                                    if (fileContent[j] == '-' && (fileContent[j + 1] != '>' || fileContent[j + 1] != '<') || (char.IsDigit(fileContent[0]) && j == 0)) //(Начинается ли коэффициент с "-") или (символ, стоящий в начале это число) и при этом НЕ является "->"
                                    {
                                        Lsplit = j;
                                        flag = 1;
                                    }
                                    else
                                    {
                                        if (fileContent[j] == '+') //Начинается ли коэффициент с "+"
                                        {
                                            Lsplit = j + 1;
                                            flag = 1;
                                        }
                                    }
                                }
                                else //Если Lsplit найден, то ищем Rsplit
                                {
                                    if (fileContent[j] == 'x') //Элемент в строке это x
                                    {
                                        Rsplit = j;
                                        //CellId только для x={1,...,9}
                                        int CellId = int.Parse(fileContent.Substring(Rsplit + 1, 1)) - 1; //Расчет индекса ячейки переменной x
                                        int check; //Переменная для попытки перевода строки в int
                                        if (int.TryParse(fileContent.Substring(Lsplit, Rsplit - Lsplit), out check) || fileContent.Substring(Lsplit, Rsplit - Lsplit) == "-" || fileContent.Substring(Lsplit, Rsplit - Lsplit).Length == 0) //(Удалось выделить из строки число) или (выделили "-") или (длина найденой подстроки == 0)
                                        {
                                            Ratio[i, CellId] = fileContent.Substring(Lsplit, Rsplit - Lsplit); //Присваиваем в вычисленную ячейку полученную подстроку
                                        }
                                        else  //(Не удалось выделить из строки число) или (не выделили "-") или (длина найденой подстроки != 0)
                                        {
                                            throw new Exception();
                                        }

                                        if ((fileContent.Substring(Lsplit, Rsplit - Lsplit) == "-")) //Найденая подстрока это "-"
                                        {
                                            Ratio[i, CellId] = "-1"; //Присваиваем в вычисленную ячейку "-1"
                                        }
                                        else //Найденая подстрока это НЕ "-"
                                        {
                                            if (fileContent.Substring(Lsplit, Rsplit - Lsplit) == "") //Найденная подстрока пустая
                                            {
                                                Ratio[i, CellId] = "1"; //Присваиваем в вычисленную ячейку "1"
                                            }
                                        }

                                        if (fileContent[0].ToString().StartsWith("x")) //Первый элемент начинается с x
                                        {
                                            Ratio[i, 0] = "1"; //Присваиваем в первую ячейку "1"
                                        }
                                        flag = 0;
                                    }
                                }
                                if (fileContent[j] == '-' && fileContent[j + 1] == '>' && flag_find == 0) //(Рядом стоящие символы образуют "->") и (последний элемент не был обнаружен)
                                {
                                    Ratio[i, ColumnCounterInFile - 1] = "->"; //Присваиваем в предпоследнюю ячейку "->"
                                    Ratio[i, ColumnCounterInFile] = fileContent.Substring(j + 2, 3); //Присваиваем в последнюю ячейку "к чему стремится"
                                    flag_find = 1;
                                }
                                else
                                {
                                    if (fileContent[j] == '<' && fileContent[j + 1] == '=' && flag_find == 0) //(Рядом стоящие символы образуют "<=") и (последний элемент не был обнаружен)
                                    {
                                        Ratio[i, ColumnCounterInFile - 1] = "<="; //Присваиваем в предпоследнюю ячейку "<="
                                        Ratio[i, ColumnCounterInFile] = fileContent.Substring(j + 2, fileContent.Length - 2 - j); //Присваиваем в последнюю ячейку "значение ограничения"
                                        flag_find = 1;
                                    }
                                    else
                                    {
                                        if (fileContent[j] == '>' && fileContent[j + 1] == '=' && flag_find == 0) //(Рядом стоящие символы образуют ">=") и (последний элемент не был обнаружен)
                                        {
                                            Ratio[i, ColumnCounterInFile - 1] = ">="; //Присваиваем в предпоследнюю ячейку ">="
                                            Ratio[i, ColumnCounterInFile] = fileContent.Substring(j + 2, fileContent.Length - 2 - j); //Присваиваем в последнюю ячейку "значение ограничения"
                                            flag_find = 1;
                                        }
                                        else
                                        {
                                            if (fileContent[j] == '=' && flag_find == 0) //(Символ строки образует "=") и (последний элемент не был обнаружен)
                                            {

                                                Ratio[i, ColumnCounterInFile - 1] = "="; //Присваиваем в предпоследнюю ячейку "="
                                                Ratio[i, ColumnCounterInFile] = fileContent.Substring(j + 1, fileContent.Length - 1 - j); //Присваиваем в последнюю ячейку "значение ограничения"
                                                flag_find = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            fileContent = reader.ReadLine(); //Считываем новую строку
                        }
                        //Вывод данных в DGV
                        for (int r = 0; r < Ratio.GetLength(0); r++)
                        {
                            for (int c = 0; c < Ratio.GetLength(1); c++)
                            {
                                if (r == 0) //Первая итерация - заполнение DGV целевой функции
                                {
                                    if (string.IsNullOrEmpty(Ratio[r, c])) //Элемент в матрице оказался пустым
                                    {
                                        dataGridViewTargetFunction[c, r].Value = "0";
                                    }
                                    else
                                    {
                                        dataGridViewTargetFunction[c, r].Value = Ratio[r, c];
                                    }
                                }
                                else //Последующие итерации - заполнение DGV ограничений
                                {
                                    if (string.IsNullOrEmpty(Ratio[r, c])) //Элемент в матрице оказался пустым
                                    {
                                        dataGridViewRestrictions[c, r - 1].Value = "0";
                                    }
                                    else
                                    {
                                        dataGridViewRestrictions[c, r - 1].Value = Ratio[r, c];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Неправильный формат данных!", "Ошибка чтения из файла"); //Вывод о возможной ошибке в файле
            }
        }

        //Сохранение в файл
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog(); //Создание файла сохранения
            saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt"; //Фильтр текстовых файлов

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName); //Полный путь к файлу
                for (int i = 0; i < dataGridViewRestrictions.Rows.Count + 1; i++)
                {
                    for (int j = 0; j < dataGridViewRestrictions.Columns.Count; j++)
                    {
                        if (i == 0) //Первая итерация - запись целевой функции
                        {
                            if (dataGridViewTargetFunction[j, i].Value.ToString().StartsWith("-") && j < dataGridViewTargetFunction.ColumnCount - 2) //(Элемент начинается с "-") и (элемент находится левее "->")
                            {
                                if (dataGridViewTargetFunction[j, i].Value.ToString()[1] == '1') //Элемент является "-1"
                                {
                                    streamWriter.Write("-x" + (j + 1));
                                }
                                else //Элемент НЕ является "-1"
                                {
                                    streamWriter.Write(dataGridViewTargetFunction[j, i].Value.ToString() + "x" + (j + 1));
                                }
                            }
                            else //(Элемент не начинается с "-") и (элемент не находится левее "->")
                            {
                                if (char.IsDigit(dataGridViewTargetFunction[j, i].Value.ToString()[0]) && j < dataGridViewTargetFunction.ColumnCount - 2) //(Символ числа действительно является числом) и (элемент находится левее "->")
                                {
                                    if (dataGridViewTargetFunction[j, i].Value.ToString() == "0") //Элемент == 0
                                    {
                                        continue;
                                    }
                                    else //Элемент != 0
                                    {
                                        if (dataGridViewTargetFunction[j, i].Value.ToString() == "1") //Элемент == 1
                                        {
                                            if (j == 0) //Элемент является первым в строке
                                            {
                                                streamWriter.Write("x" + (j + 1));
                                            }
                                            else //Элемент является НЕ первым в строке
                                            {
                                                streamWriter.Write("+x" + (j + 1));
                                            }
                                        }
                                        else //Элемент != 1
                                        {
                                            if (j != 0) //Элемент является не первым в строке
                                            {
                                                streamWriter.Write("+" + dataGridViewTargetFunction[j, i].Value.ToString() + "x" + (j + 1));
                                            }
                                            else //Элемент является первым в строке
                                            {
                                                streamWriter.Write(dataGridViewTargetFunction[j, i].Value.ToString() + "x" + (j + 1));
                                            }
                                        }
                                    }
                                }
                                else //(Символ числа не является числом) и (элемент находится НЕ левее "->")
                                {
                                    streamWriter.Write(dataGridViewTargetFunction[j, i].Value.ToString());
                                }
                            }
                        }
                        else //Последующие итерации - запись ограничений
                        {
                            if (dataGridViewRestrictions[j, i - 1].Value.ToString().StartsWith("-") && j < dataGridViewRestrictions.ColumnCount - 2) //(Элемент начинается с "-") и (элемент находится левее "->")
                            {
                                if (dataGridViewRestrictions[j, i - 1].Value.ToString()[1] == '1') //Элемент является "-1"
                                {
                                    streamWriter.Write("-x" + (j + 1));
                                }
                                else //Элемент НЕ является "-1"
                                {
                                    streamWriter.Write(dataGridViewRestrictions[j, i - 1].Value.ToString() + "x" + (j + 1));
                                }
                            }
                            else //(Элемент не начинается с "-") и (элемент не находится левее "->")
                            {
                                if (char.IsDigit(dataGridViewRestrictions[j, i - 1].Value.ToString()[0]) && j < dataGridViewRestrictions.ColumnCount - 2) //(Символ числа действительно является числом) и (элемент находится левее "->")
                                {
                                    if (dataGridViewRestrictions[j, i - 1].Value.ToString() == "0") //Элемент == 0
                                    {
                                        continue;
                                    }
                                    else //Элемент != 0
                                    {
                                        if (dataGridViewRestrictions[j, i - 1].Value.ToString() == "1") //Элемент == 1
                                        {
                                            if (j == 0) //Элемент является первым в строке
                                            {
                                                streamWriter.Write("x" + (j + 1));
                                            }
                                            else //Элемент является НЕ первым в строке
                                            {
                                                streamWriter.Write("+x" + (j + 1));
                                            }
                                        }
                                        else //Элемент != 1
                                        {
                                            if (j != 0) //Элемент является не первым в строке
                                            {
                                                streamWriter.Write("+" + dataGridViewRestrictions[j, i - 1].Value.ToString() + "x" + (j + 1));
                                            }
                                            else //Элемент является первым в строке
                                            {
                                                streamWriter.Write(dataGridViewRestrictions[j, i - 1].Value.ToString() + "x" + (j + 1));
                                            }
                                        }
                                    }
                                }
                                else //(Символ числа не является числом) и (элемент находится НЕ левее "->")
                                {
                                    streamWriter.Write(dataGridViewRestrictions[j, i - 1].Value.ToString());
                                }
                            }
                        }
                    }
                    streamWriter.WriteLine(); //Последующая запись будет осуществляться в нвоую строку
                }
                streamWriter.Close(); //Запись окончена
            }
        }


        //
        //Кнопки
        //
        //Кнопка начала рассчётов
        public void CalculateButton_Click(object sender, EventArgs e)
        {
            //Проверка на запуск пустой программы
            try
            {
                //Целевая функция
                for (int c = 0; c < dataGridViewTargetFunction.ColumnCount; c++)
                {
                    if (dataGridViewTargetFunction[c, 0].Value == null)
                    {
                        throw new Exception();
                    }
                }
                //Функция ограничений
                for (int c = 0; c < dataGridViewRestrictions.ColumnCount; c++)
                {
                    for (int r = 0; r < dataGridViewRestrictions.RowCount; r++)
                    {
                        if (dataGridViewRestrictions[c, r].Value == null)
                        {
                            throw new Exception();
                        }
                    }
                }

                //Очистка всех необходимых переменных
                Solution = false;
                CounterIteration = 0;
                ListOfIndex.Clear();
                ValueRow.Clear();
                Target_function.Clear();
                columnMinLst.Clear();
                rowMinLst.Clear();
                BasisColumn = 0;
                BasisRow = 0;
                FirstFillMatrix(); //Заполнение начальных матриц перед вычислениями

                //Выбор типа дробей
                if (DecimalRB.Checked)
                {
                    IsDecimal = true;
                }
                else if (NaturalRB.Checked)
                {
                    IsDecimal = false;
                }

                RestrictionsCounter = (int)RestrictionsCount.Value;
                VariablesCounter = (int)VariableCount.Value;

                if (dataGridViewTargetFunction[dataGridViewTargetFunction.ColumnCount - 1, 0].Value.ToString() == "min")
                {
                    IsMin = true;
                }
                else if (dataGridViewTargetFunction[dataGridViewTargetFunction.ColumnCount - 1, 0].Value.ToString() == "max")
                {
                    IsMin = false;
                }

                if (CBMethod.SelectedIndex == 0) //Выбор автоматического метода
                {
                    Decision newForm = new Decision(this);
                }
                else if (CBMethod.SelectedIndex == 1) //Выбор пошагового метода
                {
                    DecisionStepByStep newform = new DecisionStepByStep();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка ввода коэффициентов", "Неправильный формат данных!");

            }
        }
        //Кнопка очистки полей
        private void ClearButton_Click(object sender, EventArgs e)
        {
            dataGridViewTargetFunctionLoad();
            dataGridViewRestrictionsLoad();
        }

        //
        //Функции, использующиеся при расчёте
        //
        //Функция, заполняющая необходимы для расчёта матрицы
        private void FirstFillMatrix()
        {
            ListOfIndex.Clear(); //Очистка матрицы индексов для новой задачи
            CounterIteration = 0; //Очистка количества итераций решения для новой задачи
            ValueRow.Clear(); //Очистка списка исключенных из поиска строк для новой задачи

            if (DecimalRB.Checked) //Выбраны десятичные дроби
            {
                StartMatrix.Clear(); //Очистка стартовой матрицы для новой задачи
                //
                //Заполнение начальной матрицы ограничениями
                //
                for (int r = 0; r < dataGridViewRestrictions.Rows.Count; r++)
                {
                    List<string> row = new List<string>();
                    for (int c = 0; c < dataGridViewRestrictions.Columns.Count; c++)
                    {
                        row.Add(dataGridViewRestrictions[c, r].Value.ToString());
                    }
                    StartMatrix.Add(row);
                }
                //
                //Заполнение начальной матрицы строкой с суммой
                //
                List<string> row_Sum = new List<string>(); //Строка для суммы
                for (int c = 0; c < StartMatrix[0].Count; c++)
                {
                    double counter_sum = 0; //Счётчик суммы
                    for (int r = 0; r < StartMatrix.Count; r++)
                    {
                        if (c != StartMatrix[0].Count - 2)
                        {
                            counter_sum += double.Parse(StartMatrix[r][c]) * -1;
                        }
                    }
                    if (c == StartMatrix[0].Count - 2) //Если текущий столбец - столбец со знаками
                    {
                        row_Sum.Add("=");
                    }
                    else //Если текущий столбец - не столбец со знаками
                    {
                        row_Sum.Add(counter_sum.ToString());
                    }
                }
                StartMatrix.Add(row_Sum); //Добавление строки в начальную матрицу
            }


            else if (NaturalRB.Checked) //Выбраны натуральные дроби
            {
                Fraction_StartMatrix.Clear(); //Очистка стартовой матрицы для новой задачи
                //
                //Заполнение начальной матрицы ограничениями
                //
                for (int r = 0; r < dataGridViewRestrictions.Rows.Count; r++)
                {
                    List<Fraction> FractionRow = new List<Fraction>();
                    for (int c = 0; c < dataGridViewRestrictions.Columns.Count; c++)
                    {
                        if (c != dataGridViewRestrictions.ColumnCount - 2)
                        {
                            if (dataGridViewRestrictions[c, r].Value.ToString().Split('/').Count() == 2) //Если элемент - дробь
                            {
                                FractionRow.Add(new Fraction(Convert.ToInt32(dataGridViewRestrictions[c, r].Value.ToString().Split('/')[0]), Convert.ToInt32(dataGridViewRestrictions[c, r].Value.ToString().Split('/')[1])));
                            }
                            else if (dataGridViewRestrictions[c, r].Value.ToString().Split('/').Count() == 1) //Если элемент - число
                            {
                                FractionRow.Add(new Fraction(Convert.ToInt32(dataGridViewRestrictions[c, r].Value)));
                            }
                        }
                    }
                    Fraction_StartMatrix.Add(FractionRow);
                }
                //
                //Заполнение начальной матрицы строкой с суммой
                //
                List<Fraction> FractionRow_Sum = new List<Fraction>(); //Строка для суммы
                for (int c = 0; c < Fraction_StartMatrix[0].Count; c++)
                {
                    Fraction FractionCounter_sum = new Fraction(0); //Счётчик суммы
                    for (int r = 0; r < Fraction_StartMatrix.Count; r++)
                    {
                        FractionCounter_sum += Fraction_StartMatrix[r][c];
                    }
                    FractionCounter_sum = -FractionCounter_sum;
                    FractionRow_Sum.Add(FractionCounter_sum);
                }
                Fraction_StartMatrix.Add(FractionRow_Sum); //Добавление строки в начальную матрицу
            }
            //
            //Первое заполнение массива индексов
            //
            List<string> row_index = new List<string>(); //Горизонтальные индексы
            row_index.Add("0");
            int first_row = 0; //Флаг на первую строку
            for (int c = 0; c < RestrictionsCount.Value; c++)
            {
                int one_include = 0; //Флаг на первый элемент
                List<string> column_index = new List<string>(); //Вертикальные индексы
                if (one_include == 0)
                {
                    column_index.Add((c + 1 + (int)VariableCount.Value).ToString());
                    one_include = 1;
                }
                for (int r = 1; r < VariableCount.Value + 1; r++)
                {
                    if (first_row == 0) //Вернхяя строка индексов
                    {
                        row_index.Add(r.ToString()); //Добавление горизонтального индекса
                    }
                    column_index.Add("0");
                }
                if (first_row == 0)
                {
                    ListOfIndex.Add(row_index);
                }
                first_row = 1;
                ListOfIndex.Add(column_index);
            }

            //
            //Заполнение матрицы с коэффициентами целевой функции
            //
            for (int c = 0; c < dataGridViewTargetFunction.Columns.Count - 2; c++)
            {
                Target_function.Add(dataGridViewTargetFunction[c, 0].Value.ToString());
            }
        }

        //
        //Функции проверки
        //
        //Проверка на наличие 0 в суммирующей строчке для остановки первого шага симплекс-метода
        public bool MainStepCheck()
        {
            int ZeroCount = 0;
            if (DecimalRB.Checked)
            {
                for (int c = 0; c < StartMatrix[0].Count - 1; c++)
                {
                    if (c != StartMatrix[0].Count - 2 && Math.Round(double.Parse(StartMatrix[StartMatrix.Count - 1][c]), 1) == 0)
                    {
                        ZeroCount++;
                    }
                }
                if (ZeroCount == StartMatrix[StartMatrix.Count - 1].Count - 2)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (NaturalRB.Checked)
            {
                for (int c = 0; c < Fraction_StartMatrix[0].Count - 1; c++)
                {
                    if (Fraction_StartMatrix[Fraction_StartMatrix.Count - 1][c] == 0)
                    {
                        ZeroCount++;
                    }
                }
                if (Fraction_StartMatrix[Fraction_StartMatrix.Count - 1].Count - 1 == ZeroCount)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
        //Проверка задачи на разрешимость
        public bool solvability()
        {
            if (!Solution)
            {
                if (DecimalRB.Checked)
                {
                    if (Math.Round(double.Parse(StartMatrix[StartMatrix.Count - 1][StartMatrix[0].Count - 1]), 0) > 0) //Значение функции > 0
                    {
                        MessageBox.Show("Система ограничений несовместна");
                        Solution = true;
                        return false;
                    }
                    else if (Math.Round(double.Parse(StartMatrix[StartMatrix.Count - 1][StartMatrix[0].Count - 1]), 0) < 0) //Значение функции < 0
                    {
                        MessageBox.Show("Задача неразрешима");
                        Solution = true;
                        return false;
                    }
                    else //Задача разрешима
                    {
                        Solution = true;
                        return true;
                    }
                } else if (NaturalRB.Checked)
                {
                    if (Fraction_StartMatrix[Fraction_StartMatrix.Count - 1][Fraction_StartMatrix[0].Count - 1] > 0) //Значение функции > 0
                    {
                        MessageBox.Show("Система ограничений несовместна");
                        Solution = true;
                        return false;
                    }
                    else if (Fraction_StartMatrix[Fraction_StartMatrix.Count - 1][Fraction_StartMatrix[0].Count - 1] < 0) //Значение функции < 0
                    {
                        MessageBox.Show("Задача неразрешима");
                        Solution = true;
                        return false;
                    }
                    else //Задача разрешима
                    {
                        Solution = true;
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        //Проверка на наличие положительных столбцов
        public bool SecondStepCheck()
        {
            int PositiveCount = 0;
            if (DecimalRB.Checked)
            {
                for (int c = 0; c < StartMatrix[0].Count - 1; c++)
                {
                    if (c != StartMatrix[0].Count - 2 && Math.Round(double.Parse(StartMatrix[StartMatrix.Count - 1][c]), 2) >= 0)
                    {
                        PositiveCount++;
                    }
                }
                if (PositiveCount == StartMatrix[StartMatrix.Count - 1].Count - 2)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (NaturalRB.Checked)
            {
                for (int c = 0; c < Fraction_StartMatrix[0].Count - 1; c++)
                {
                    if (Fraction_StartMatrix[Fraction_StartMatrix.Count - 1][c] > 0)
                    //if (Fraction_StartMatrix[Fraction_StartMatrix.Count - 1][c] >= 0)
                    {
                        PositiveCount++;
                    }
                }
                if (PositiveCount == Fraction_StartMatrix[Fraction_StartMatrix.Count - 1].Count - 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
    }
}
