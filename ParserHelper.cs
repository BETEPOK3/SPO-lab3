using System;
using System.Collections.Generic;
using System.Text;
using lexer;

namespace parser
{
    public class Node
    {
        public Node[] Children { get; private set; }

        public (TokenType, dynamic) Value { get; private set; }

        public Node((TokenType, dynamic) value, Node[] children = null)
        {
            Value = value;
            Children = children;
        }

        /// <summary>
        /// Вычислить значение выражения (дерева)
        /// </summary>
        /// <param name="vars">Возможные переменные</param>
        /// <returns>Вычисленное значение</returns>
        public double SolveTree(IDictionary<string, double> vars = null)
        {
            // Число или переменная
            if (Children == null)
            {
                if (Value.Item2.GetType() == typeof(double))
                {
                    return (double)Value.Item2;
                }
                double num = 0;
                if (vars != null && vars.TryGetValue(Value.Item2, out num))
                {
                    return num;
                }
                return ParserHelper.GetNumFromVar(Value.Item2);
            }
            else
            {
                // Функция
                if (Value.Item1 == TokenType.Tok_id)
                {
                    double[] values = new double[Children.Length];
                    for (int i = 0; i < Children.Length; ++i)
                    {
                        values[i] = Children[i].SolveTree(vars);
                    }
                    return ParserHelper.ActivateFunc(Value.Item2, values);
                }

                // Бинарная операция
                else
                {
                    double num1 = Children[0].SolveTree(vars);
                    double num2 = Children[1].SolveTree(vars);
                    switch (Value.Item1)
                    {
                        case TokenType.Tok_plus:
                            return num1 + num2;
                        case TokenType.Tok_minus:
                            return num1 - num2;
                        case TokenType.Tok_mult:
                            return num1 * num2;
                        case TokenType.Tok_divide:
                            return num1 / num2;
                        case TokenType.Tok_power:
                            return Math.Pow(num1, num2);
                    }
                }
            }
            throw new ApplicationException($"Unknown error while solving tree");
        }

        /// <summary>
        /// Создание строкового представления дерева
        /// </summary>
        /// <param name="vars">Возможные переменные</param>
        /// <returns></returns>
        public string GetTree(IDictionary<string, double> vars = null)
        {
            // Число или переменная
            if (Children == null)
            {
                return Value.Item2.ToString();
            }
            else
            {
                // Функция
                if (Value.Item1 == TokenType.Tok_id)
                {
                    int childrenLength = Children.Length;
                    StringBuilder result = new StringBuilder($"{Value.Item2}({Children[childrenLength - 1].GetTree(vars)}");
                    for (int i = childrenLength - 2; i >= 0; --i)
                    {
                        result.Append($", {Children[i].GetTree(vars)}");
                    }
                    result.Append(')');
                    return result.ToString();
                }

                // Бинарная операция
                else
                {
                    return $"{Value.Item2}({Children[0].GetTree(vars)}, {Children[1].GetTree(vars)})";
                }
            }
            throw new ApplicationException($"Unknown error while getting tree");
        }

        /// <summary>
        /// Проверить дерево на возможность подставить значения
        /// </summary>
        /// <param name="vars">Возможные названия переменных</param>
        public void CheckTree(string[] vars = null)
        {
            // Переменная
            if (Children == null)
            {
                if (Value.Item2.GetType() == typeof(string))
                {
                    if (vars != null && Array.IndexOf(vars, Value.Item2) == -1)
                    {
                        ParserHelper.GetNumFromVar(Value.Item2);
                    }
                }
            }
            else
            {
                // Функция
                if (Value.Item1 == TokenType.Tok_id)
                {
                    Func func = ParserHelper.GetFuncFromName(Value.Item2);
                    if (func.Vars.Length != Children.Length)
                    {
                        throw new ApplicationException($"Function {Value.Item2} has {func.Vars.Length} arguments, not {Children.Length}");
                    }
                    foreach (Node node in Children)
                    {
                        node.CheckTree(vars);
                    }
                }

                // Бинарная операция
                else
                {
                    Children[0].CheckTree(vars);
                    Children[1].CheckTree(vars);
                }
            }
        }
    }

    public class Func
    {
        public Node Root { get; private set; }

        public string[] Vars { get; private set; }

        public Func(Node root, List<string> vars)
        {
            Root = root;
            Vars = vars.ToArray();
        }
    }

    public static class ParserHelper
    {
        private static IDictionary<string, double> _vars = new Dictionary<string, double>()
        {
            { "pi", Math.PI },
            { "e", Math.E }
        };

        private static IDictionary<string, Func> _funcs = new Dictionary<string, Func>();

        /// <summary>
        /// Возвращает значение переменной
        /// </summary>
        /// <param name="id">Название переменной</param>
        /// <returns>Значение переменной</returns>
        public static double GetNumFromVar(string id)
        {
            double result;
            if (_vars.TryGetValue(id, out result))
            {
                return result;
            }
            throw new ApplicationException($"Unknown id {id}");
        }

        /// <summary>
        /// Возвращает объект функции по её наименованию
        /// </summary>
        /// <param name="id">Наименование</param>
        /// <returns>Объект функции</returns>
        public static Func GetFuncFromName(string id)
        {
            Func result;
            if (_funcs.TryGetValue(id, out result))
            {
                return result;
            }
            throw new ApplicationException($"Unknown function {id}");
        }

        /// <summary>
        /// Добавляет переменную в хранилище
        /// </summary>
        /// <param name="id">Наименование переменной</param>
        /// <param name="num">Значение переменной</param>
        /// <returns>Строка с результатом</returns>
        public static string AddVar(string id, double num)
        {
            if (!_vars.TryAdd(id, num))
            {
                _vars.Remove(id);
                _vars.Add(id, num);
                return $"Changed var {id} to {num}";
            }
            return $"Added new var {id} = {num}";
        }

        /// <summary>
        /// Добавляет функцию в хранилище
        /// </summary>
        /// <param name="id">Наименование функции</param>
        /// <param name="func">Объект функции</param>
        /// <returns>Строка с результатом</returns>
        public static string AddFunc(string id, Func func)
        {
            func.Root.CheckTree(func.Vars);
            if (!_funcs.TryAdd(id, func))
            {
                _funcs.Remove(id);
                _funcs.Add(id, func);
                return $"Changed function {id}";
            }
            return $"Added new function {id}";
        }

        /// <summary>
        /// Выполнить функцию
        /// </summary>
        /// <param name="funcName">Наименование функции</param>
        /// <param name="values">Аргументы</param>
        /// <returns>Возвращённое функцией значение</returns>
        public static double ActivateFunc(string funcName, double[] values)
        {
            Func func = GetFuncFromName(funcName);
            if (func.Vars.Length == values.Length)
            {
                Dictionary<string, double> varsInfo = new Dictionary<string, double>();
                for (int i = 0; i < func.Vars.Length; ++i)
                {
                    varsInfo.Add(func.Vars[i], values[i]);
                }
                return func.Root.SolveTree(varsInfo);
            }
            throw new ApplicationException($"Function {funcName} has {func.Vars.Length} arguments, not {values.Length}");
        }
    }
}