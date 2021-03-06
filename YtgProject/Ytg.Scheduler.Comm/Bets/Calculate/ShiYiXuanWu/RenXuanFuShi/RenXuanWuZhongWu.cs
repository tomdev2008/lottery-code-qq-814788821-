﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ytg.Scheduler.Comm.Bets.Calculate.ShiYiXuanWu.RenXuanFuShi
{
    /// <summary>
    /// 验证通过2015/05/25
    /// </summary>
    public class RenXuanWuZhongWu : BaseCalculate
    {
        protected override void IssueCalculate(string issueCode, string openResult, BasicModel.LotteryBasic.BetDetail item)
        {
            var isWin = IsWin(item.BetContent.Split(','), openResult);
            if (isWin>0)
            {
                item.IsMatch = true;
                decimal stepAmt = 0;
                item.WinMoney = TotalWinMoney(item, GetBaseAmt(item, ref stepAmt), stepAmt, isWin);
            }
        }

        public override int TotalBetCount(BasicModel.LotteryBasic.BetDetail item)
        {
            var result = item.BetContent;

            var items = result.Split(',');
            if (items.Length < 5) return 0;
            return TotalBet(items).Count;
        }

        public override string HtmlContentFormart(string betContent)
        {
            var newCt = VerificationSelectBetContent11x5(betContent.Replace('&', ','), false);
            if (newCt.Split(',').Length < 5)
                return "";
            return newCt;
        }

        private  List<string> TotalBet(string[] items)
        {
            var list = new List<string>();
            var item = from a in items
                       from b in items
                       from c in items
                       from d in items
                       from e in items
                       select string.Format("{0},{1},{2},{3},{4}", a, b, c, d, e);
            foreach (var it in item)
            {
                if (it.Split(',').Distinct().Count() == 5)
                {

                    var i = string.Join(",", it.Split(',').OrderBy(m => m));
                    if (!list.Any(m => m == i)) list.Add(i);
                }
            }
            return list;
        }

        private int IsWin(string[] list, string result)
        {
            var opens = result.Split(',');
            var source = TotalBet(list);

            return source.Count(c => c.Split(',').All(x => opens.Contains(x)));
        }

       
    }
}
