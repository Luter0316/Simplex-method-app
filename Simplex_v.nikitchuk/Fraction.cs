using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simplex_v.nikitchuk
{
    public class Fraction
    {
        public int numerator;              // Числитель
        public int denominator;            // Знаменатель
        public int sign;					// Знак

        //
        // Конструктор
        //
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new DivideByZeroException("В знаменателе не может быть нуля");
            }
            this.numerator = Math.Abs(numerator);
            this.denominator = Math.Abs(denominator);
            if (numerator * denominator < 0)
            {
                this.sign = -1;
            }
            else
            {
                this.sign = 1;
            }
        }

        // Вызов первого конструктора со знаменателем равным единице
        public Fraction(int number) : this(number, 1) { }

        //
        // Перегрузка "+"
        //
        // Сложение дробей
        public static Fraction operator +(Fraction a, Fraction b)
        {
            return performOperation(a, b, (int x, int y) => x + y);
        }
        // Сложение дроби и числа
        public static Fraction operator +(Fraction a, int b)
        {
            return a + new Fraction(b);
        }

        //
        // Перегрузка "-"
        //
        // Вычитание дробей
        public static Fraction operator -(Fraction a, Fraction b)
        {
            return performOperation(a, b, (int x, int y) => x - y).Reduce();
        }
        // Вычитание из дроби числа
        public static Fraction operator -(Fraction a, int b)
        {
            return (a - new Fraction(b)).Reduce();
        }

        //
        // Перегрузка "*"
        //
        // Произведение двух дробей
        public static Fraction operator *(Fraction a, Fraction b)
        {
            if (a.numerator == b.denominator)
            {
                return new Fraction(b.numerator * 1 * a.sign * b.sign, a.denominator * 1).Reduce();
            }
            else if (a.denominator == b.numerator)
            {
                return new Fraction(1 * a.numerator * a.sign * b.sign, b.denominator * 1).Reduce();
            }
            else
            {
                return new Fraction(a.numerator * a.sign * b.numerator * b.sign, a.denominator * b.denominator).Reduce();
            }
        }
        // Произведения числа и дроби
        public static Fraction operator *(int a, Fraction b)
        {
            return new Fraction(a * b.numerator * b.sign, b.denominator).Reduce();
        }

        //
        // Перегрузка "/"
        //
        // Деление двух дробей
        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (a.denominator == b.denominator)
            {
                return new Fraction(a.numerator * 1 * a.sign * b.sign, b.numerator * 1).Reduce();
            }
            else if (a.numerator == b.numerator)
            {
                return new Fraction(1 * b.denominator * a.sign * b.sign, a.denominator * 1).Reduce();
            }
            else return new Fraction(a.numerator * b.denominator * a.sign * b.sign, a.denominator * b.numerator).Reduce();
        }

        // Перегрузка оператора "унарный минус"
        public static Fraction operator -(Fraction a)
        {
            return a.GetWithChangedSign();
        }

        // Обратная дробь
        public Fraction GetReverse()
        {
            return new Fraction(this.denominator * this.sign, this.numerator);
        }

        // Изменение знака
        public Fraction GetWithChangedSign()
        {
            return new Fraction(-this.numerator * this.sign, this.denominator);
        }

        // Равенство дробей
        public bool Equals(Fraction that)
        {
            Fraction a = this.Reduce();
            Fraction b = that.Reduce();
            return a.numerator == b.numerator &&
            a.denominator == b.denominator &&
            a.sign == b.sign;
        }

        //
        // Перегрузка "=="
        //
        // для дробей
        public static bool operator ==(Fraction a, Fraction b)
        {
            Object aAsObj = a as Object;
            Object bAsObj = b as Object;
            if (aAsObj == null || bAsObj == null)
            {
                return aAsObj == bAsObj;
            }
            return a.Equals(b);
        }
        // для дроби и числа
        public static bool operator ==(Fraction a, int b)
        {
            return a == new Fraction(b);
        }
        
        //
        // Перегрузка "!="
        //
        // для двух дробей
        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }
        // для дроби и числа
        public static bool operator !=(Fraction a, int b)
        {
            return a != new Fraction(b);
        }

        // Сравнение дробей (0, если дроби равны;
        //				     1, если this больше that;
        //				     -1, если this меньше that)
        public int CompareTo(Fraction that)
        {
            if (this.Equals(that))
            {
                return 0;
            }
            Fraction a = this.Reduce();
            Fraction b = that.Reduce();
            if (a.numerator * a.sign * b.denominator > b.numerator * b.sign * a.denominator)
            {
                return 1;
            }
            return -1;
        }

        //
        // Перегрузка ">"
        //
        // для двух дробей
        public static bool operator >(Fraction a, Fraction b)
        {
            return a.CompareTo(b) > 0;
        }
        // для двух дробей
        public static bool operator <(Fraction a, Fraction b)
        {
            return a.CompareTo(b) < 0;
        }
        // для дроби и числа
        public static bool operator >(Fraction a, int b)
        {
            return a > new Fraction(b);
        }

        //
        // Перегрузка "<"
        //
        // для дроби и числа
        public static bool operator <(Fraction a, int b)
        {
            return a < new Fraction(b);
        }

        //
        // Перегрузка ">="
        //
        // для двух дробей
        public static bool operator >=(Fraction a, Fraction b)
        {
            return a.CompareTo(b) >= 0;
        }

        //
        //Перегрузка "<="
        //
        // для двух дробей
        public static bool operator <=(Fraction a, Fraction b)
        {
            return a.CompareTo(b) <= 0;
        }

        // Сократить дробь
        public Fraction Reduce()
        {
            Fraction result = this;
            int greatestCommonDivisor = getGreatestCommonDivisor(this.numerator, this.denominator);
            result.numerator /= greatestCommonDivisor;
            result.denominator /= greatestCommonDivisor;
            return result;
        }

        // Переопределение метода ToString
        public override string ToString()
        {
            if (this.numerator == 0)
            {
                return "0";
            }
            string result;
            if (this.sign < 0)
            {
                result = "-";
            }
            else
            {
                result = "";
            }
            if (this.numerator == this.denominator)
            {
                return result + "1";
            }
            if (this.denominator == 1)
            {
                return result + this.numerator;
            }
            return result + this.numerator + "/" + this.denominator;
        }

        // НОД
        public static int getGreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // НОК
        public static int getLeastCommonMultiple(int a, int b)
        {
            return a * b / getGreatestCommonDivisor(a, b);
        }

        // Метод создан для устранения повторяющегося кода в методах сложения и вычитания дробей.
        // Возвращает дробь, которая является результатом сложения или вычитания дробей a и b,
        // В зависимости от того, какая операция передана в параметр operation.
        // P.S. использовать только для сложения и вычитания
        public static Fraction performOperation(Fraction a, Fraction b, Func<int, int, int> operation)
        {
            // Наименьшее общее кратное знаменателей
            int leastCommonMultiple = getLeastCommonMultiple(a.denominator, b.denominator);
            // Дополнительный множитель к первой дроби
            int additionalMultiplierFirst = leastCommonMultiple / a.denominator;
            // Дополнительный множитель ко второй дроби
            int additionalMultiplierSecond = leastCommonMultiple / b.denominator;
            // Результат операции
            int operationResult = operation(a.numerator * additionalMultiplierFirst * a.sign,
            b.numerator * additionalMultiplierSecond * b.sign);
            return new Fraction(operationResult, a.denominator * additionalMultiplierFirst);
        }
    }
}
