/*
 * 表达式分析及计算辅助类
 * 版权所有(C) Richard Bao
 * 
 * 此源代码可免费用于各类软件（包括商业软件）
 * 允许对此代码的进一步修改与开发
 * 但必须完整保留此版权与授权声明
 * 
 * 实际使用只需要 Calculate() 函数即可，其它函数仅用于演示其算法和调试
 * 例如：
 * 
 * string exp = "3.5 * (1 + 3 And 5) / ~2 - 2 ^ 3";
 * double result = ExpressionCalculator.Expressions.Calculate(exp);
 * 
 */

using System;
using System.Text;
using System.Collections.Generic;

namespace ASync.eTermPlugIn
{
    /// <summary>
    /// 提供表达式计算功能
    /// </summary>
    public class Expressions
    {
        /// <summary>
        /// 将表达式中的操作数和运算符分割出来
        /// </summary>
        /// <param name="expression">文本表达式</param>
        /// <returns>操作数与运算符表</returns>
        internal static List<IOperatorOrOperand> SplitExpression(string expression)
        {
            List<IOperatorOrOperand> output = new List<IOperatorOrOperand>();
            StringBuilder operandbuf = new StringBuilder();
            StringBuilder operatorbuf = new StringBuilder();

            // 记录刚才最后输出的表达式项
            IOperatorOrOperand lastItem = null;

            // 在尾部添加一个空格，帮助分离最后一个操作数或运算符
            expression = expression + " ";

            double result = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsDigit(expression[i]) == true || expression[i] == '.')
                {
                    // 如果是数字或小数点（操作数成份）

                    // 结束前一个运算符
                    if (operatorbuf.Length > 0)
                    {
                        // 尝试获取运算符
                        OperatorBase opr = TryGetOperator(operatorbuf.ToString(), lastItem);
                        if (opr != null)
                        {
                            output.Add(opr);
                            lastItem = opr;
                            operatorbuf.Length = 0;
                        }
                        else
                        {
                            throw new InvalidCastException(operatorbuf.ToString() + " 无法解析为合法的运算符。");
                        }
                    }
                    
                    // 合并入当前操作数项
                    operandbuf.Append(expression[i]);
                }
                else 
                {
                    // 不是数字或小数点（运算符成份）

                    // 结束前一个操作数
                    if (operandbuf.Length > 0)
                    {
                        if (double.TryParse(operandbuf.ToString(), out result) == false)
                        {
                            throw new FormatException(operandbuf.ToString() + " 无法解析为合法的操作数。");
                        }

                        // 输出操作数
                        OperandInfo operand = new OperandInfo(double.Parse(operandbuf.ToString()));
                        output.Add(operand);
                        lastItem = operand;
                        operandbuf.Length = 0;
                    }

                    // 合并非空白字符到当前运算符项
                    if(char.IsWhiteSpace(expression[i]) == false)
                    {
                        operatorbuf.Append(expression[i]);
                    }

                    // 分析并输出运算符
                    if (operatorbuf.Length > 0)
                    {
                        // 尝试获取运算符
                        OperatorBase opr = TryGetOperator(operatorbuf.ToString(), lastItem);
                        if (opr != null)
                        {
                            output.Add(opr);
                            lastItem = opr;
                            operatorbuf.Length = 0;
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// 将表达式转换为后缀表达式
        /// </summary>
        /// <param name="expression">文本表达式</param>
        /// <returns>转换后的后缀表达式</returns>
        internal static List<IOperatorOrOperand> ConvertInfixToPostfix(string expression)
        {
            // 预处理中缀表达式
            List<IOperatorOrOperand> infix = SplitExpression(expression);
            // 运算符栈
            System.Collections.Generic.Stack<OperatorBase> opr = new System.Collections.Generic.Stack<OperatorBase>();
            // 后缀表达式输出
            List<IOperatorOrOperand> output = new List<IOperatorOrOperand>();

            // 遍历
            foreach (IOperatorOrOperand item in infix)
            {
                if (item.IsOperator)
                {
                    // 是运算符
                    if (item.GetType() == typeof(OperatorCloseBracket))
                    {
                        // 闭括号

                        // 弹出运算符，直至遇到左括号为止
                        while (opr.Peek().GetType() != typeof(OperatorOpenBracket))
                        {
                            output.Add(opr.Pop());
                            if (opr.Count == 0)
                            {
                                // 括号不配对
                                throw new InvalidCastException("左右括号不匹配。");
                            }
                        }

                        // 弹出左括号
                        opr.Pop();
                    }
                    else
                    {
                        // 其它运算符
                        OperatorBase thisopr = item as OperatorBase;

                        // 弹出优先级高或相等的运算符
                        int thisPriority = thisopr.Priority;
                        while (opr.Count > 0)
                        {
                            OperatorBase topopr = opr.Peek();
                            if(topopr.GetType() != typeof(OperatorOpenBracket))
                            {
                                // 如果栈顶运算符不为左括号
                                if (topopr.Priority > thisopr.Priority)
                                {
                                    // 如果栈顶中的运算符优先级高于当前运算符，则输出并弹出栈
                                    output.Add(opr.Pop());
                                }
                                else if (topopr.Priority == thisopr.Priority)
                                {
                                    // 如果栈顶中的运算符优先级与当前运算符相等
                                    if (topopr.Direction == OperatingDirection.LeftToRight)
                                    {
                                        // 如果栈顶运算符结合性方向为从左至右，则输出并弹出栈
                                        output.Add(opr.Pop());
                                    }
                                    else
                                    {
                                        // 如果是从右至左，终止弹栈
                                        break;
                                    }
                                }
                                else
                                {
                                    // 终止弹栈
                                    break;
                                }
                            }
                            else
                            {
                                // 终止弹栈
                                break;
                            }
                        }

                        // 将当前运算符压入栈中
                        opr.Push(thisopr);
                    }
                }
                else
                {
                    // 是操作数
                    // 直接输出
                    output.Add(item);
                }
            }

            // 遍历结束，输出栈中全部剩余
            while (opr.Count > 0)
            {
                output.Add(opr.Pop());
            }

            return output;
        }

        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <param name="expression">文本表达式</param>
        /// <returns>计算结果</returns>
        public static double Calculate(string expression)
        {
            // 预处理后缀表达式
            List<IOperatorOrOperand> postfix = Expressions.ConvertInfixToPostfix(expression);
            // 操作数栈
            System.Collections.Generic.Stack<double> data = new System.Collections.Generic.Stack<double>();

            // 遍历
            foreach (IOperatorOrOperand item in postfix)
            {
                if (item.IsOperator)
                {
                    // 运算符
                    OperatorBase opr = item as OperatorBase;

                    // 从操作数栈中取出操作数
                    if (data.Count < opr.OperandCount)
                    {
                        throw new InvalidCastException("无效的表达式。缺少运算符或出现多余的操作数。");
                    }
                    double[] operands = new double[opr.OperandCount];
                    for (int i = opr.OperandCount - 1; i >= 0; i--)
                    {
                        operands[i] = data.Pop();
                    }

                    // 计算并将结果压回栈中
                    data.Push(opr.Calculate(operands));
                }
                else
                {
                    // 操作数
                    // 压入操作数栈
                    data.Push(((OperandInfo)item).Value);
                }
            }

            // 取最后结果
            if (data.Count != 1)
            {
                throw new InvalidCastException("无效的表达式。缺少运算符或出现多余的操作数。");
            }
            return data.Pop();
        }

        #region 运算符与操作数信息

        /// <summary>
        /// 指出运算符的结合性方向
        /// </summary>
        private enum OperatingDirection
        {
            /// <summary>
            /// 表示从左至右的结合性方向
            /// </summary>
            LeftToRight,
            /// <summary>
            /// 表示从右至左的结合性方向
            /// </summary>
            RightToLeft,
            /// <summary>
            /// 无结合性
            /// </summary>
            None
        }

        /// <summary>
        /// 表示运算符或者操作数
        /// </summary>
        internal interface IOperatorOrOperand
        {
            /// <summary>
            /// 是否为运算符
            /// </summary>
            bool IsOperator
            {
                get;
            }

            /// <summary>
            /// 是否为操作数
            /// </summary>
            bool IsOperand
            {
                get;
            }
        }

        /// <summary>
        /// 表示一个操作数所包含的信息
        /// </summary>
        private struct OperandInfo : IOperatorOrOperand
        {
            public readonly double Value;

            public OperandInfo(double value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return "操作数：" + Value.ToString();
            }

            #region IOperatorOrOperand 成员

            public bool IsOperator
            {
                get { return false; }
            }

            public bool IsOperand
            {
                get { return true; }
            }

            #endregion
        }

        /// <summary>
        /// 表示一个运算符所包含的信息
        /// </summary>
        private abstract class OperatorBase : IOperatorOrOperand
        {
            /// <summary>
            /// 运算符符号
            /// </summary>
            public abstract string OperatorSymbol { get; }
            /// <summary>
            /// 运算符名称
            /// </summary>
            public abstract string OperatorName { get; }
            /// <summary>
            /// 优先级
            /// </summary>
            public abstract int Priority { get; }
            /// <summary>
            /// 结合性方向
            /// </summary>
            public abstract OperatingDirection Direction { get; }
            /// <summary>
            /// 需要的操作数个数
            /// </summary>
            public abstract int OperandCount { get; }

            /// <summary>
            /// 计算结果
            /// </summary>
            /// <param name="operands">需要的操作数</param>
            /// <returns>返回计算结果</returns>
            public double Calculate(double[] operands)
            {
                if (operands == null)
                {
                    throw new ArgumentNullException("找不到操作数。");
                }

                if (operands.Length != OperandCount)
                {
                    throw new ArgumentException(OperatorSymbol + " 运算符需要 " + OperandCount.ToString() +
                        " 个操作数，但只找到 " + operands.Length + " 个。");
                }

                return OnCalculate(operands);
            }

            /// <summary>
            /// 计算结果（参数已检查）
            /// </summary>
            /// <param name="operands">需要的操作数（已检查）</param>
            /// <returns>返回计算结果</returns>
            protected abstract double OnCalculate(double[] operands);

            public override string ToString()
            {
                return "运算符：" + OperatorName + " [" + OperatorSymbol + "]";
            }

            #region IOperatorOrOperand 成员

            public bool IsOperator
            {
                get { return true; }
            }

            public bool IsOperand
            {
                get { return false; }
            }

            #endregion
        }

        /// <summary>
        /// 表示开括号运算符
        /// </summary>
        private class OperatorOpenBracket : OperatorBase
        {
            public override string OperatorSymbol { get { return "("; } }
            public override string OperatorName { get { return "左括号"; } }
            public override int Priority { get { return int.MaxValue; } }
            public override OperatingDirection Direction { get { return OperatingDirection.None; } }
            public override int OperandCount { get { return 0; } }

            protected override double OnCalculate(double[] operands)
            {
                throw new InvalidOperationException("无法在左括号上执行运算。");
            }
        }

        /// <summary>
        /// 表示闭括号运算符
        /// </summary>
        private class OperatorCloseBracket : OperatorBase
        {
            public override string OperatorSymbol { get { return ")"; } }
            public override string OperatorName { get { return "右括号"; } }
            public override int Priority { get { return 0; } }
            public override OperatingDirection Direction { get { return OperatingDirection.None; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                throw new InvalidOperationException("无法在右括号上执行运算。");
            }
        }

        /// <summary>
        /// 表示加法运算符
        /// </summary>
        private class OperatorPlus : OperatorBase
        {
            public override string OperatorSymbol { get { return "+"; } }
            public override string OperatorName { get { return "加号"; } }
            public override int Priority { get { return 12; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] + operands[1];
            }
        }

        /// <summary>
        /// 表示减法运算符
        /// </summary>
        private class OperatorMinus : OperatorBase
        {
            public override string OperatorSymbol { get { return "-"; } }
            public override string OperatorName { get { return "减号"; } }
            public override int Priority { get { return 12; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] - operands[1];
            }
        }

        /// <summary>
        /// 表示乘法运算符
        /// </summary>
        private class OperatorMultiply : OperatorBase
        {
            public override string OperatorSymbol { get { return "*"; } }
            public override string OperatorName { get { return "乘号"; } }
            public override int Priority { get { return 13; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] * operands[1];
            }
        }

        /// <summary>
        /// 表示除法运算符
        /// </summary>
        private class OperatorDivide : OperatorBase
        {
            public override string OperatorSymbol { get { return "/"; } }
            public override string OperatorName { get { return "除号"; } }
            public override int Priority { get { return 13; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] / operands[1];
            }
        }

        /// <summary>
        /// 表示取正运算符
        /// </summary>
        private class OperatorPositive : OperatorBase
        {
            public override string OperatorSymbol { get { return "+"; } }
            public override string OperatorName { get { return "取正号"; } }
            public override int Priority { get { return 15; } }
            public override OperatingDirection Direction { get { return OperatingDirection.RightToLeft; } }
            public override int OperandCount { get { return 1; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0];
            }
        }

        /// <summary>
        /// 表示取负运算符
        /// </summary>
        private class OperatorNegative : OperatorBase
        {
            public override string OperatorSymbol { get { return "-"; } }
            public override string OperatorName { get { return "取负号"; } }
            public override int Priority { get { return 15; } }
            public override OperatingDirection Direction { get { return OperatingDirection.RightToLeft; } }
            public override int OperandCount { get { return 1; } }

            protected override double OnCalculate(double[] operands)
            {
                return -operands[0];
            }
        }

        /// <summary>
        /// 表示取余运算符
        /// </summary>
        private class OperatorMod : OperatorBase
        {
            public override string OperatorSymbol { get { return "%"; } }
            public override string OperatorName { get { return "取余"; } }
            public override int Priority { get { return 13; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] % operands[1];
            }
        }

        /// <summary>
        /// 表示取幂运算符
        /// </summary>
        private class OperatorPower : OperatorBase
        {
            public override string OperatorSymbol { get { return "^"; } }
            public override string OperatorName { get { return "取幂"; } }
            public override int Priority { get { return 14; } }
            public override OperatingDirection Direction { get { return OperatingDirection.RightToLeft; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return Math.Pow(operands[0], operands[1]);
            }
        }

        /// <summary>
        /// 表示位与运算符
        /// </summary>
        private class OperatorBitAnd : OperatorBase
        {
            public override string OperatorSymbol { get { return "AND"; } }
            public override string OperatorName { get { return "按位与"; } }
            public override int Priority { get { return 8; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                int op1 = (int)operands[0];
                int op2 = (int)operands[1];

                if (op1 == operands[0] && op2 == operands[1])
                {
                    return op1 & op2;
                }
                else
                {
                    throw new InvalidCastException("AND 运算符必须用于两个整数。");
                }
            }
        }

        /// <summary>
        /// 表示位或运算符
        /// </summary>
        private class OperatorBitOr : OperatorBase
        {
            public override string OperatorSymbol { get { return "OR"; } }
            public override string OperatorName { get { return "按位或"; } }
            public override int Priority { get { return 6; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                int op1 = (int)operands[0];
                int op2 = (int)operands[1];

                if (op1 == operands[0] && op2 == operands[1])
                {
                    return op1 | op2;
                }
                else
                {
                    throw new InvalidCastException("OR 运算符必须用于两个整数。");
                }
            }
        }

        /// <summary>
        /// 表示位异或运算符
        /// </summary>
        private class OperatorBitXor : OperatorBase
        {
            public override string OperatorSymbol { get { return "XOR"; } }
            public override string OperatorName { get { return "按位异或"; } }
            public override int Priority { get { return 7; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                int op1 = (int)operands[0];
                int op2 = (int)operands[1];

                if (op1 == operands[0] && op2 == operands[1])
                {
                    return op1 ^ op2;
                }
                else
                {
                    throw new InvalidCastException("XOR 运算符必须用于两个整数。");
                }
            }
        }

        /// <summary>
        /// 表示按位取反运算符
        /// </summary>
        private class OperatorBitReverse : OperatorBase
        {
            public override string OperatorSymbol { get { return "~"; } }
            public override string OperatorName { get { return "按位取反"; } }
            public override int Priority { get { return 15; } }
            public override OperatingDirection Direction { get { return OperatingDirection.RightToLeft; } }
            public override int OperandCount { get { return 1; } }

            protected override double OnCalculate(double[] operands)
            {
                int op1 = (int)operands[0];

                if (op1 == operands[0])
                {
                    return ~op1;
                }
                else
                {
                    throw new InvalidCastException("~ 运算符必须用于整数。.");
                }
            }
        }

        /// <summary>
        /// 表示位左移运算符
        /// </summary>
        private class OperatorBitShiftLeft : OperatorBase
        {
            public override string OperatorSymbol { get { return "<<"; } }
            public override string OperatorName { get { return "左移"; } }
            public override int Priority { get { return 11; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                int op1 = (int)operands[0];
                int op2 = (int)operands[1];

                if (op1 == operands[0] && op2 == operands[1])
                {
                    return op1 << op2;
                }
                else
                {
                    throw new InvalidCastException("<< 运算符必须用于两个整数。");
                }
            }
        }

        /// <summary>
        /// 表示位异或运算符
        /// </summary>
        private class OperatorBitShiftRight : OperatorBase
        {
            public override string OperatorSymbol { get { return ">>"; } }
            public override string OperatorName { get { return "右移"; } }
            public override int Priority { get { return 11; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                int op1 = (int)operands[0];
                int op2 = (int)operands[1];

                if (op1 == operands[0] && op2 == operands[1])
                {
                    return op1 >> op2;
                }
                else
                {
                    throw new InvalidCastException(">> 运算符必须用于两个整数。");
                }
            }
        }

        /// <summary>
        /// 尝试返回一个运算符对象
        /// </summary>
        /// <param name="exp">要测试的字符串</param>
        /// <param name="leftItem"></param>
        /// <returns>如果成功，返回一个运算符对象实例；否则返回空</returns>
        private static OperatorBase TryGetOperator(string exp, IOperatorOrOperand leftItem)
        {
            // 判断左侧是否是操作数
            bool hasLeftOperand = false;
            if (leftItem == null)
            {
                // 没有左项
                hasLeftOperand = false;
            }
            else if (leftItem.IsOperand)
            {
                // 左项是操作数
                hasLeftOperand = true;
            }
            else if (leftItem.GetType() == typeof(OperatorCloseBracket))
            {
                // 左项是闭括号
                hasLeftOperand = true;
            }
            else
            {
                // 其它情况
                hasLeftOperand = false;
            }

            // 根据符号文本判断
            string symbol = exp.ToUpper();
            switch (symbol)
            {
                case "(":
                    return new OperatorOpenBracket();
                case ")":
                    return new OperatorCloseBracket();
            }

            // 根据左操作数情况判断
            if (hasLeftOperand == true)
            {
                // 有左操作数者
                switch (exp.ToUpper())
                {
                    case "+":
                        return new OperatorPlus();
                    case "-":
                        return new OperatorMinus();
                    case "*":
                        return new OperatorMultiply();
                    case "/":
                        return new OperatorDivide();
                    case "%":
                        return new OperatorMod();
                    case "^":
                        return new OperatorPower();
                    case "AND":
                        return new OperatorBitAnd();
                    case "OR":
                        return new OperatorBitOr();
                    case "XOR":
                        return new OperatorBitXor();
                    case "<<":
                        return new OperatorBitShiftLeft();
                    case ">>":
                        return new OperatorBitShiftRight();
                }
            }
            else
            {
                // 没有左操作数
                switch (exp.ToUpper())
                {
                    case "+":
                        return new OperatorPositive();
                    case "-":
                        return new OperatorNegative();
                    case "~":
                        return new OperatorBitReverse();
                }
            }

            // 不可判断者，返回空
            return null;
        }

        #endregion
    }
}
