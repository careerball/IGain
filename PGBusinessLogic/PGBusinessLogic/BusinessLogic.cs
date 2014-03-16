namespace PGBusinessLogic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class BusinessLogic
    {
        [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        internal static extern bool ConvertSidToStringSid(IntPtr sid, [In, Out, MarshalAs(UnmanagedType.LPTStr)] ref string pStringSid);
        [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        internal static extern bool ConvertStringSidToSid([In, MarshalAs(UnmanagedType.LPTStr)] string pStringSid, ref IntPtr sid);
        public static DataSet FlipDataSet(DataSet my_DataSet)
        {
            DataSet set = new DataSet();
            foreach (DataTable table in my_DataSet.Tables)
            {
                DataTable table2 = new DataTable();
                for (int i = 0; i <= table.Rows.Count; i++)
                {
                    table2.Columns.Add(Convert.ToString(i));
                }
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    DataRow row = table2.NewRow();
                    row[0] = table.Columns[j].ToString();
                    for (int k = 1; k <= table.Rows.Count; k++)
                    {
                        row[k] = table.Rows[k - 1][j];
                    }
                    table2.Rows.Add(row);
                }
                set.Tables.Add(table2);
            }
            return set;
        }

        public static long GetNextTransactionID(OleDbConnection Con)
        {
            long num = -1L;
            if (Con == null)
            {
                return -1L;
            }
            try
            {
                object obj2 = new OleDbCommand("SELECT MAX(transactionid) from transactions", Con).ExecuteScalar();
                if ((obj2 == DBNull.Value) || (Convert.ToUInt32(obj2) < 1))
                {
                    num = 1L;
                }
                else
                {
                    num = Convert.ToUInt32(obj2) + 1;
                }
            }
            catch (Exception)
            {
                return -1L;
            }
            return num;
        }

        public static string GetSid(string name)
        {
            int num3;
            IntPtr zero = IntPtr.Zero;
            int cbSid = 0;
            int cbReferencedDomainName = 0;
            StringBuilder referencedDomainName = new StringBuilder();
            int error = 0;
            string pStringSid = "";
            LookupAccountName(null, name, zero, ref cbSid, referencedDomainName, ref cbReferencedDomainName, out num3);
            error = Marshal.GetLastWin32Error();
            if (error != 0x7a)
            {
                throw new Exception(new Win32Exception(error).Message);
            }
            referencedDomainName = new StringBuilder(cbReferencedDomainName);
            zero = Marshal.AllocHGlobal(cbSid);
            if (!LookupAccountName(null, name, zero, ref cbSid, referencedDomainName, ref cbReferencedDomainName, out num3))
            {
                error = Marshal.GetLastWin32Error();
                Marshal.FreeHGlobal(zero);
                throw new Exception(new Win32Exception(error).Message);
            }
            if (!ConvertSidToStringSid(zero, ref pStringSid))
            {
                error = Marshal.GetLastWin32Error();
                Marshal.FreeHGlobal(zero);
                throw new Exception(new Win32Exception(error).Message);
            }
            Marshal.FreeHGlobal(zero);
            return pStringSid;
        }

        public static double InferAmountReturnableFromInterestSchema(DataTable dtIS, double CarriedPrinciple, DateTime TransactionInitDate, DateTime TransactionEndDate, double CarriedInterest, string CreditID, string TransactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated, OleDbConnection Con)
        {
            Exception exception2;
            Exception exception3;
            double num = CarriedPrinciple;
            double num2 = num;
            foreach (DataRow row in dtIS.Rows)
            {
                string str;
                Exception exception;
                double num3 = Convert.ToDouble(row["Rate"]);
                double num4 = (Convert.ToString(row["Per"]) == "Year") ? 365.0 : 30.4;
                double y = 0.0;
                TimeSpan span = new TimeSpan(0L);
                DateTime time = Convert.ToDateTime(row["StartDate"]);
                DateTime time2 = Convert.ToDateTime(row["EndDate"]);
                if ((TransactionInitDate > time2) || (TransactionEndDate < time))
                {
                    if (((TransactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated != null) && (Con != null)) && (TransactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated.Length > 0))
                    {
                        str = string.Concat(new object[] { "UPDATE Transactions set Debit='0',Credit='0',DateOfTransaction='", TransactionEndDate, "' where TransactionSet='", TransactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated, "' and SequenceOfInterestApplication=", Convert.ToString(row["SequenceOfInterestApplication"]), "" });
                        try
                        {
                            if (new OleDbCommand(str, Con).ExecuteNonQuery() == 0)
                            {
                                throw new Exception("Update of Interest Row " + Convert.ToString(row["SequenceOfInterestApplication"]) + " failed!");
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            throw exception;
                        }
                    }
                }
                else
                {
                    if ((TransactionInitDate <= time) && (TransactionEndDate <= time2))
                    {
                        span = TransactionEndDate.Subtract(time);
                    }
                    else if ((TransactionInitDate > time) && (TransactionEndDate < time2))
                    {
                        span = TransactionEndDate.Subtract(TransactionInitDate);
                    }
                    else if ((TransactionInitDate >= time) && (TransactionEndDate >= time2))
                    {
                        span = time2.Subtract(TransactionInitDate);
                    }
                    else if ((TransactionInitDate < time) && (TransactionEndDate > time2))
                    {
                        span = time2.Subtract(time);
                    }
                    y = ((double) span.Days) / num4;
                    if (Convert.ToBoolean(row["IsCompound"]))
                    {
                        num2 = Convert.ToDouble((double) (num * Math.Pow(1.0 + (num3 / 100.0), y)));
                    }
                    else
                    {
                        num2 = Convert.ToDouble((double) (num + (((num * num3) * y) / 100.0)));
                    }
                    double num7 = num2 - num;
                    if (((TransactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated != null) && (Con != null)) && (TransactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated.Length > 0))
                    {
                        string queryToRun = string.Concat(new object[] { "UPDATE CREDITFLOW SET INTERESTDUE='", num7.ToString(), "', AMOUNTRETURNABLE='", num2.ToString(), "', CarriedPrinciple='", num, "', BroughtForward='", CarriedInterest, "' WHERE CREDITID='", CreditID, "'" });
                        try
                        {
                            exception2 = ModifyStoreHouse(Con, queryToRun, null);
                            if (exception2 != null)
                            {
                                throw exception2;
                            }
                        }
                        catch (Exception exception4)
                        {
                            exception3 = exception4;
                            throw exception3;
                        }
                        str = string.Concat(new object[] { "UPDATE Transactions set Debit='", num7.ToString(), "',Credit='", num7, "',DateOfTransaction='", TransactionEndDate, "' where TransactionSet='", TransactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated, "' and SequenceOfInterestApplication=", Convert.ToString(row["SequenceOfInterestApplication"]), "" });
                        try
                        {
                            if (new OleDbCommand(str, Con).ExecuteNonQuery() == 0)
                            {
                                throw new Exception("Update of Interest Row [" + Convert.ToString(row["SequenceOfInterestApplication"]) + "] failed!");
                            }
                        }
                        catch (Exception exception5)
                        {
                            exception = exception5;
                            throw exception;
                        }
                    }
                    num = num2 + CarriedInterest;
                    CarriedInterest = 0.0;
                    num2 = num;
                }
            }
            try
            {
                string str3 = "UPDATE CREDITFLOW SET AmountReturnable='" + num2.ToString() + "' where CreditID='" + CreditID + "' ";
                exception2 = ModifyStoreHouse(Con, str3, null);
                if (exception2 != null)
                {
                    throw exception2;
                }
            }
            catch (Exception exception6)
            {
                exception3 = exception6;
                throw exception3;
            }
            return num2;
        }

        [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool LookupAccountName([In, MarshalAs(UnmanagedType.LPTStr)] string systemName, [In, MarshalAs(UnmanagedType.LPTStr)] string accountName, IntPtr sid, ref int cbSid, StringBuilder referencedDomainName, ref int cbReferencedDomainName, out int use);
        [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern bool LookupAccountSid([In, MarshalAs(UnmanagedType.LPTStr)] string systemName, IntPtr sid, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder name, ref int cbName, StringBuilder referencedDomainName, ref int cbReferencedDomainName, out int use);
        public static Exception ModifyStoreHouse(OleDbConnection Con, string QueryToRun, string QueryToValidate)
        {
            Exception exception;
            if (Con == null)
            {
                return new Exception("Connection is not a valid object");
            }
            OleDbCommand command = new OleDbCommand();
            OleDbTransaction transaction = null;
            try
            {
                transaction = Con.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Connection = Con;
                command.Transaction = transaction;
            }
            catch (Exception exception1)
            {
                exception = exception1;
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return exception;
            }
            try
            {
                if (QueryToValidate != null)
                {
                    command.CommandText = QueryToValidate;
                    int num = (int) command.ExecuteScalar();
                    if (num > 0)
                    {
                        throw new Exception("Validation Query Failed.Account is in use.");
                    }
                }
            }
            catch (Exception exception3)
            {
                exception = exception3;
                transaction.Rollback();
                return exception;
            }
            try
            {
                command.CommandText = QueryToRun;
                if (command.ExecuteNonQuery() == 0)
                {
                    throw new Exception(" The record for this entry does not exist! Can not update.");
                }
            }
            catch (Exception exception4)
            {
                exception = exception4;
                transaction.Rollback();
                return exception;
            }
            try
            {
                transaction.Commit();
            }
            catch (Exception exception5)
            {
                return exception5;
            }
            return null;
        }

        public static DialogResult MyMessageBox(string text)
        {
            return MessageBox.Show(text, "IGain", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static DialogResult MyMessageBox(string text, string caption)
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static DialogResult MyMessageBox(string text, string caption, MessageBoxButtons MB_Buttons, MessageBoxIcon MB_Icon)
        {
            return MessageBox.Show(text, caption, MB_Buttons, MB_Icon);
        }

        public static Exception PerformMultipleQueriesWithoutValidation(OleDbConnection Con, string[] QueriesToRun)
        {
            Exception exception;
            if ((Con == null) || (QueriesToRun == null))
            {
                return new Exception("Either Connection or Query Object is null");
            }
            OleDbCommand command = new OleDbCommand();
            OleDbTransaction transaction = null;
            try
            {
                transaction = Con.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Connection = Con;
                command.Transaction = transaction;
            }
            catch (Exception exception1)
            {
                exception = exception1;
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return exception;
            }
            try
            {
                for (int i = 0; i < QueriesToRun.Length; i++)
                {
                    string str = QueriesToRun[i];
                    command.CommandText = str;
                    if (command.ExecuteNonQuery() == 0)
                    {
                        throw new Exception(" No record affected ! Can not execute.");
                    }
                }
            }
            catch (Exception exception4)
            {
                exception = exception4;
                transaction.Rollback();
                return new Exception(exception.Message + "\nError Executing Query :- " + command.CommandText);
            }
            try
            {
                transaction.Commit();
            }
            catch (Exception exception5)
            {
                return exception5;
            }
            return null;
        }
    }
}

