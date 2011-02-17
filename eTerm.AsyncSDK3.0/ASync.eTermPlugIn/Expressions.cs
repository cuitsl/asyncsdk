/*
 * ���ʽ���������㸨����
 * ��Ȩ����(C) Richard Bao
 * 
 * ��Դ�����������ڸ��������������ҵ�����
 * ����Դ˴���Ľ�һ���޸��뿪��
 * ���������������˰�Ȩ����Ȩ����
 * 
 * ʵ��ʹ��ֻ��Ҫ Calculate() �������ɣ�����������������ʾ���㷨�͵���
 * ���磺
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
    /// �ṩ���ʽ���㹦��
    /// </summary>
    public class Expressions
    {
        /// <summary>
        /// �����ʽ�еĲ�������������ָ����
        /// </summary>
        /// <param name="expression">�ı����ʽ</param>
        /// <returns>���������������</returns>
        internal static List<IOperatorOrOperand> SplitExpression(string expression)
        {
            List<IOperatorOrOperand> output = new List<IOperatorOrOperand>();
            StringBuilder operandbuf = new StringBuilder();
            StringBuilder operatorbuf = new StringBuilder();

            // ��¼�ղ��������ı��ʽ��
            IOperatorOrOperand lastItem = null;

            // ��β�����һ���ո񣬰����������һ���������������
            expression = expression + " ";

            double result = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsDigit(expression[i]) == true || expression[i] == '.')
                {
                    // ��������ֻ�С���㣨�������ɷݣ�

                    // ����ǰһ�������
                    if (operatorbuf.Length > 0)
                    {
                        // ���Ի�ȡ�����
                        OperatorBase opr = TryGetOperator(operatorbuf.ToString(), lastItem);
                        if (opr != null)
                        {
                            output.Add(opr);
                            lastItem = opr;
                            operatorbuf.Length = 0;
                        }
                        else
                        {
                            throw new InvalidCastException(operatorbuf.ToString() + " �޷�����Ϊ�Ϸ����������");
                        }
                    }
                    
                    // �ϲ��뵱ǰ��������
                    operandbuf.Append(expression[i]);
                }
                else 
                {
                    // �������ֻ�С���㣨������ɷݣ�

                    // ����ǰһ��������
                    if (operandbuf.Length > 0)
                    {
                        if (double.TryParse(operandbuf.ToString(), out result) == false)
                        {
                            throw new FormatException(operandbuf.ToString() + " �޷�����Ϊ�Ϸ��Ĳ�������");
                        }

                        // ���������
                        OperandInfo operand = new OperandInfo(double.Parse(operandbuf.ToString()));
                        output.Add(operand);
                        lastItem = operand;
                        operandbuf.Length = 0;
                    }

                    // �ϲ��ǿհ��ַ�����ǰ�������
                    if(char.IsWhiteSpace(expression[i]) == false)
                    {
                        operatorbuf.Append(expression[i]);
                    }

                    // ��������������
                    if (operatorbuf.Length > 0)
                    {
                        // ���Ի�ȡ�����
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
        /// �����ʽת��Ϊ��׺���ʽ
        /// </summary>
        /// <param name="expression">�ı����ʽ</param>
        /// <returns>ת����ĺ�׺���ʽ</returns>
        internal static List<IOperatorOrOperand> ConvertInfixToPostfix(string expression)
        {
            // Ԥ������׺���ʽ
            List<IOperatorOrOperand> infix = SplitExpression(expression);
            // �����ջ
            System.Collections.Generic.Stack<OperatorBase> opr = new System.Collections.Generic.Stack<OperatorBase>();
            // ��׺���ʽ���
            List<IOperatorOrOperand> output = new List<IOperatorOrOperand>();

            // ����
            foreach (IOperatorOrOperand item in infix)
            {
                if (item.IsOperator)
                {
                    // �������
                    if (item.GetType() == typeof(OperatorCloseBracket))
                    {
                        // ������

                        // �����������ֱ������������Ϊֹ
                        while (opr.Peek().GetType() != typeof(OperatorOpenBracket))
                        {
                            output.Add(opr.Pop());
                            if (opr.Count == 0)
                            {
                                // ���Ų����
                                throw new InvalidCastException("�������Ų�ƥ�䡣");
                            }
                        }

                        // ����������
                        opr.Pop();
                    }
                    else
                    {
                        // ���������
                        OperatorBase thisopr = item as OperatorBase;

                        // �������ȼ��߻���ȵ������
                        int thisPriority = thisopr.Priority;
                        while (opr.Count > 0)
                        {
                            OperatorBase topopr = opr.Peek();
                            if(topopr.GetType() != typeof(OperatorOpenBracket))
                            {
                                // ���ջ���������Ϊ������
                                if (topopr.Priority > thisopr.Priority)
                                {
                                    // ���ջ���е���������ȼ����ڵ�ǰ������������������ջ
                                    output.Add(opr.Pop());
                                }
                                else if (topopr.Priority == thisopr.Priority)
                                {
                                    // ���ջ���е���������ȼ��뵱ǰ��������
                                    if (topopr.Direction == OperatingDirection.LeftToRight)
                                    {
                                        // ���ջ�����������Է���Ϊ�������ң������������ջ
                                        output.Add(opr.Pop());
                                    }
                                    else
                                    {
                                        // ����Ǵ���������ֹ��ջ
                                        break;
                                    }
                                }
                                else
                                {
                                    // ��ֹ��ջ
                                    break;
                                }
                            }
                            else
                            {
                                // ��ֹ��ջ
                                break;
                            }
                        }

                        // ����ǰ�����ѹ��ջ��
                        opr.Push(thisopr);
                    }
                }
                else
                {
                    // �ǲ�����
                    // ֱ�����
                    output.Add(item);
                }
            }

            // �������������ջ��ȫ��ʣ��
            while (opr.Count > 0)
            {
                output.Add(opr.Pop());
            }

            return output;
        }

        /// <summary>
        /// ������ʽ��ֵ
        /// </summary>
        /// <param name="expression">�ı����ʽ</param>
        /// <returns>������</returns>
        public static double Calculate(string expression)
        {
            // Ԥ�����׺���ʽ
            List<IOperatorOrOperand> postfix = Expressions.ConvertInfixToPostfix(expression);
            // ������ջ
            System.Collections.Generic.Stack<double> data = new System.Collections.Generic.Stack<double>();

            // ����
            foreach (IOperatorOrOperand item in postfix)
            {
                if (item.IsOperator)
                {
                    // �����
                    OperatorBase opr = item as OperatorBase;

                    // �Ӳ�����ջ��ȡ��������
                    if (data.Count < opr.OperandCount)
                    {
                        throw new InvalidCastException("��Ч�ı��ʽ��ȱ�����������ֶ���Ĳ�������");
                    }
                    double[] operands = new double[opr.OperandCount];
                    for (int i = opr.OperandCount - 1; i >= 0; i--)
                    {
                        operands[i] = data.Pop();
                    }

                    // ���㲢�����ѹ��ջ��
                    data.Push(opr.Calculate(operands));
                }
                else
                {
                    // ������
                    // ѹ�������ջ
                    data.Push(((OperandInfo)item).Value);
                }
            }

            // ȡ�����
            if (data.Count != 1)
            {
                throw new InvalidCastException("��Ч�ı��ʽ��ȱ�����������ֶ���Ĳ�������");
            }
            return data.Pop();
        }

        #region ��������������Ϣ

        /// <summary>
        /// ָ��������Ľ���Է���
        /// </summary>
        private enum OperatingDirection
        {
            /// <summary>
            /// ��ʾ�������ҵĽ���Է���
            /// </summary>
            LeftToRight,
            /// <summary>
            /// ��ʾ��������Ľ���Է���
            /// </summary>
            RightToLeft,
            /// <summary>
            /// �޽����
            /// </summary>
            None
        }

        /// <summary>
        /// ��ʾ��������߲�����
        /// </summary>
        internal interface IOperatorOrOperand
        {
            /// <summary>
            /// �Ƿ�Ϊ�����
            /// </summary>
            bool IsOperator
            {
                get;
            }

            /// <summary>
            /// �Ƿ�Ϊ������
            /// </summary>
            bool IsOperand
            {
                get;
            }
        }

        /// <summary>
        /// ��ʾһ������������������Ϣ
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
                return "��������" + Value.ToString();
            }

            #region IOperatorOrOperand ��Ա

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
        /// ��ʾһ�����������������Ϣ
        /// </summary>
        private abstract class OperatorBase : IOperatorOrOperand
        {
            /// <summary>
            /// ���������
            /// </summary>
            public abstract string OperatorSymbol { get; }
            /// <summary>
            /// ���������
            /// </summary>
            public abstract string OperatorName { get; }
            /// <summary>
            /// ���ȼ�
            /// </summary>
            public abstract int Priority { get; }
            /// <summary>
            /// ����Է���
            /// </summary>
            public abstract OperatingDirection Direction { get; }
            /// <summary>
            /// ��Ҫ�Ĳ���������
            /// </summary>
            public abstract int OperandCount { get; }

            /// <summary>
            /// ������
            /// </summary>
            /// <param name="operands">��Ҫ�Ĳ�����</param>
            /// <returns>���ؼ�����</returns>
            public double Calculate(double[] operands)
            {
                if (operands == null)
                {
                    throw new ArgumentNullException("�Ҳ�����������");
                }

                if (operands.Length != OperandCount)
                {
                    throw new ArgumentException(OperatorSymbol + " �������Ҫ " + OperandCount.ToString() +
                        " ������������ֻ�ҵ� " + operands.Length + " ����");
                }

                return OnCalculate(operands);
            }

            /// <summary>
            /// �������������Ѽ�飩
            /// </summary>
            /// <param name="operands">��Ҫ�Ĳ��������Ѽ�飩</param>
            /// <returns>���ؼ�����</returns>
            protected abstract double OnCalculate(double[] operands);

            public override string ToString()
            {
                return "�������" + OperatorName + " [" + OperatorSymbol + "]";
            }

            #region IOperatorOrOperand ��Ա

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
        /// ��ʾ�����������
        /// </summary>
        private class OperatorOpenBracket : OperatorBase
        {
            public override string OperatorSymbol { get { return "("; } }
            public override string OperatorName { get { return "������"; } }
            public override int Priority { get { return int.MaxValue; } }
            public override OperatingDirection Direction { get { return OperatingDirection.None; } }
            public override int OperandCount { get { return 0; } }

            protected override double OnCalculate(double[] operands)
            {
                throw new InvalidOperationException("�޷�����������ִ�����㡣");
            }
        }

        /// <summary>
        /// ��ʾ�����������
        /// </summary>
        private class OperatorCloseBracket : OperatorBase
        {
            public override string OperatorSymbol { get { return ")"; } }
            public override string OperatorName { get { return "������"; } }
            public override int Priority { get { return 0; } }
            public override OperatingDirection Direction { get { return OperatingDirection.None; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                throw new InvalidOperationException("�޷�����������ִ�����㡣");
            }
        }

        /// <summary>
        /// ��ʾ�ӷ������
        /// </summary>
        private class OperatorPlus : OperatorBase
        {
            public override string OperatorSymbol { get { return "+"; } }
            public override string OperatorName { get { return "�Ӻ�"; } }
            public override int Priority { get { return 12; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] + operands[1];
            }
        }

        /// <summary>
        /// ��ʾ���������
        /// </summary>
        private class OperatorMinus : OperatorBase
        {
            public override string OperatorSymbol { get { return "-"; } }
            public override string OperatorName { get { return "����"; } }
            public override int Priority { get { return 12; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] - operands[1];
            }
        }

        /// <summary>
        /// ��ʾ�˷������
        /// </summary>
        private class OperatorMultiply : OperatorBase
        {
            public override string OperatorSymbol { get { return "*"; } }
            public override string OperatorName { get { return "�˺�"; } }
            public override int Priority { get { return 13; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] * operands[1];
            }
        }

        /// <summary>
        /// ��ʾ���������
        /// </summary>
        private class OperatorDivide : OperatorBase
        {
            public override string OperatorSymbol { get { return "/"; } }
            public override string OperatorName { get { return "����"; } }
            public override int Priority { get { return 13; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] / operands[1];
            }
        }

        /// <summary>
        /// ��ʾȡ�������
        /// </summary>
        private class OperatorPositive : OperatorBase
        {
            public override string OperatorSymbol { get { return "+"; } }
            public override string OperatorName { get { return "ȡ����"; } }
            public override int Priority { get { return 15; } }
            public override OperatingDirection Direction { get { return OperatingDirection.RightToLeft; } }
            public override int OperandCount { get { return 1; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0];
            }
        }

        /// <summary>
        /// ��ʾȡ�������
        /// </summary>
        private class OperatorNegative : OperatorBase
        {
            public override string OperatorSymbol { get { return "-"; } }
            public override string OperatorName { get { return "ȡ����"; } }
            public override int Priority { get { return 15; } }
            public override OperatingDirection Direction { get { return OperatingDirection.RightToLeft; } }
            public override int OperandCount { get { return 1; } }

            protected override double OnCalculate(double[] operands)
            {
                return -operands[0];
            }
        }

        /// <summary>
        /// ��ʾȡ�������
        /// </summary>
        private class OperatorMod : OperatorBase
        {
            public override string OperatorSymbol { get { return "%"; } }
            public override string OperatorName { get { return "ȡ��"; } }
            public override int Priority { get { return 13; } }
            public override OperatingDirection Direction { get { return OperatingDirection.LeftToRight; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return operands[0] % operands[1];
            }
        }

        /// <summary>
        /// ��ʾȡ�������
        /// </summary>
        private class OperatorPower : OperatorBase
        {
            public override string OperatorSymbol { get { return "^"; } }
            public override string OperatorName { get { return "ȡ��"; } }
            public override int Priority { get { return 14; } }
            public override OperatingDirection Direction { get { return OperatingDirection.RightToLeft; } }
            public override int OperandCount { get { return 2; } }

            protected override double OnCalculate(double[] operands)
            {
                return Math.Pow(operands[0], operands[1]);
            }
        }

        /// <summary>
        /// ��ʾλ�������
        /// </summary>
        private class OperatorBitAnd : OperatorBase
        {
            public override string OperatorSymbol { get { return "AND"; } }
            public override string OperatorName { get { return "��λ��"; } }
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
                    throw new InvalidCastException("AND �����������������������");
                }
            }
        }

        /// <summary>
        /// ��ʾλ�������
        /// </summary>
        private class OperatorBitOr : OperatorBase
        {
            public override string OperatorSymbol { get { return "OR"; } }
            public override string OperatorName { get { return "��λ��"; } }
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
                    throw new InvalidCastException("OR �����������������������");
                }
            }
        }

        /// <summary>
        /// ��ʾλ��������
        /// </summary>
        private class OperatorBitXor : OperatorBase
        {
            public override string OperatorSymbol { get { return "XOR"; } }
            public override string OperatorName { get { return "��λ���"; } }
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
                    throw new InvalidCastException("XOR �����������������������");
                }
            }
        }

        /// <summary>
        /// ��ʾ��λȡ�������
        /// </summary>
        private class OperatorBitReverse : OperatorBase
        {
            public override string OperatorSymbol { get { return "~"; } }
            public override string OperatorName { get { return "��λȡ��"; } }
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
                    throw new InvalidCastException("~ �������������������.");
                }
            }
        }

        /// <summary>
        /// ��ʾλ���������
        /// </summary>
        private class OperatorBitShiftLeft : OperatorBase
        {
            public override string OperatorSymbol { get { return "<<"; } }
            public override string OperatorName { get { return "����"; } }
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
                    throw new InvalidCastException("<< �����������������������");
                }
            }
        }

        /// <summary>
        /// ��ʾλ��������
        /// </summary>
        private class OperatorBitShiftRight : OperatorBase
        {
            public override string OperatorSymbol { get { return ">>"; } }
            public override string OperatorName { get { return "����"; } }
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
                    throw new InvalidCastException(">> �����������������������");
                }
            }
        }

        /// <summary>
        /// ���Է���һ�����������
        /// </summary>
        /// <param name="exp">Ҫ���Ե��ַ���</param>
        /// <param name="leftItem"></param>
        /// <returns>����ɹ�������һ�����������ʵ�������򷵻ؿ�</returns>
        private static OperatorBase TryGetOperator(string exp, IOperatorOrOperand leftItem)
        {
            // �ж�����Ƿ��ǲ�����
            bool hasLeftOperand = false;
            if (leftItem == null)
            {
                // û������
                hasLeftOperand = false;
            }
            else if (leftItem.IsOperand)
            {
                // �����ǲ�����
                hasLeftOperand = true;
            }
            else if (leftItem.GetType() == typeof(OperatorCloseBracket))
            {
                // �����Ǳ�����
                hasLeftOperand = true;
            }
            else
            {
                // �������
                hasLeftOperand = false;
            }

            // ���ݷ����ı��ж�
            string symbol = exp.ToUpper();
            switch (symbol)
            {
                case "(":
                    return new OperatorOpenBracket();
                case ")":
                    return new OperatorCloseBracket();
            }

            // ���������������ж�
            if (hasLeftOperand == true)
            {
                // �����������
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
                // û���������
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

            // �����ж��ߣ����ؿ�
            return null;
        }

        #endregion
    }
}
