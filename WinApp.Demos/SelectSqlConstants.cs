using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Demos
{
    static public class SelectSqlConstants
    {
		public const string SSelectClauses = @"
SELECT column_name
FROM table_name
WHERE condition
GROUP BY column_name 
HAVING condition
ORDER BY column_name;
";


		public const string SBaufoxHuge = @"
select 
	StockKeep.[Item No_]								as ProductId,
	StockKeep.[Location Code]							as WarehouseId,
		case when 
			IsNull(sum(ItemLedger.Quantity) - Isnull(SalesLines.SalesOrderQty,0),0)<=0 
		then 0 
		else
			IsNull(sum(ItemLedger.Quantity) - Isnull(SalesLines.SalesOrderQty,0),0) 
	end													as Qty,
	SalesPrice.[Unit Price]								as Price,
		case 
			when StockKeep.[Lead Time Calculation] = '' or StockKeep.[Lead Time Calculation] is null
			then 0 
			else LEFT(StockKeep.[Lead Time Calculation], LEN(StockKeep.[Lead Time Calculation]) - 1) 
		end                                
														as AvailabilityDays
from 
	[Baufox$Stockkeeping Unit] StockKeep WITH (NOLOCK)
		inner join [Baufox$Item] Item WITH (NOLOCK) on StockKeep.[Item No_] = Item.[No_]
		left join [Baufox$Item Ledger Entry] ItemLedger WITH (NOLOCK) on ItemLedger.[Item No_] =  StockKeep.[Item No_] and  ItemLedger.[Location Code] =  StockKeep.[Location Code]
		inner join [Baufox$Sales Price] SalesPrice WITH (NOLOCK) on StockKeep.[Item No_] = SalesPrice.[Item No_] and SalesPrice.[Location Code] = StockKeep.[Location Code]
		left join 
(
select 
	No_								as ItemCode,
	[Location Code]					as Location,
	sum([Outstanding Qty_ (Base)])	as SalesOrderQty
from 
	[Baufox$Sales Line] WITH (NOLOCK)
where 
		[Document Type] = 1 
	and [Defined Table 1] not in ('ΑΚΥΡΩΣΗ')
group by
	No_ ,
	[Location Code] 
) as SalesLines on SalesLines.ItemCode = ItemLedger.[Item No_] and SalesLines.Location = ItemLedger.[Location Code]
where 
	    SalesPrice.[Sales Code] = N'GOLD'
    and SalesPrice.[Unit of Measure Code] = 'Kg'
    and StockKeep.[Item No_] = '000123'
    and StockKeep.[Location Code]  = N'KENTRIKH'
group by 
	StockKeep.[Item No_],
	StockKeep.[Location Code],
	StockKeep.[Item No_] ,
	StockKeep.[Location Code],
	SalesPrice.[Unit Price],
	StockKeep.[Lead Time Calculation],
	SalesLines.SalesOrderQty

";


		public const string SSelectSimple = @"
select 
  Customer.Id	as Id
 ,Customer.Name as Name
 ,Country.Name	as Country
from
  Customer
	left join Country on Country.Id = Customer.CountryId

";

		public const string SSelectWithSubSelect = @"
select 
  Customer.Id	as Id
 ,Customer.Name as Name
 ,Country.Name	as Country
from
  Customer
	left join 
left join 
(select * from Countries where Continent = 'Europe' order by Name) 
as Country on Country.Id = Customer.CountryId 
where
	Customer.Name like 'teo%'
order by
    Customer.Name
";
	}
}
