
/* eslint no-constant-condition: ["error", { "checkLoops": false }] */
 
//---------------------------------------------------------------------------------------
// tp.TestData
//---------------------------------------------------------------------------------------

 

//#region tp.TestData
/** Helper static class 
 @class
 @static
 */
tp.TestData = class {

 

    /** Test data. Returns an array of first names. 
     @private
     @type {string[]} */
    static get FirstNames() { return ['Julian', 'Nigel', 'Sawyer', 'Cullen', 'Brennan', 'Boris', 'Guy', 'Kato', 'Bevis', 'Cain', 'Lester', 'Kevin', 'Herman', 'Isaac', 'Jerome', 'Benjamin', 'Phelan', 'Calvin', 'Yuli', 'Amery', 'Michael', 'Demetrius', 'Ethan', 'Jacob', 'Castor', 'Peter', 'Richard', 'Brody', 'Ray', 'Todd', 'Thaddeus', 'Arden', 'Hamish', 'Hayes', 'Davis', 'Steven', 'Addison', 'Kevin', 'Benjamin', 'Gannon', 'Lev', 'Burton', 'Ross', 'Macon', 'Rooney', 'Chester', 'Eric', 'Wesley', 'David', 'Octavius', 'Keaton', 'Maxwell', 'Caleb', 'Geoffrey', 'Lars', 'Clayton', 'Nasim', 'Channing', 'Alexander', 'Hakeem', 'Magee', 'Valentine', 'Zane', 'Asher', 'Ali', 'Hu', 'Justin', 'Raphael', 'Clark', 'Quinn', 'Slade', 'Deacon', 'Abbot', 'George', 'Seth', 'Ulric', 'Noah', 'Jesse', 'Tobias', 'Vaughan', 'Wayne', 'Gabriel', 'Roth', 'Murphy', 'Todd', 'Joshua', 'Marvin', 'Cameron', 'Uriah', 'Axel', 'Joseph', 'Zachary', 'Alvin', 'Warren', 'Erasmus', 'Blaze', 'Tiger', 'Rajah', 'Nathan', 'Armand']; }
    /** Test data. Returns an array of last names.
     @private
     @type {string[]} */
    static get LastNames() { return ['Bonner', 'Fields', 'Norman', 'Jacobson', 'Weiss', 'Forbes', 'Garrett', 'Martin', 'Turner', 'Parsons', 'Campos', 'Golden', 'Allison', 'Lindsey', 'Hines', 'Weber', 'Douglas', 'Hendrix', 'Le', 'Deleon', 'Lyons', 'Mann', 'Goff', 'Mccarty', 'Silva', 'Rogers', 'Horn', 'Crane', 'Mays', 'Hendrix', 'Turner', 'Bennett', 'Savage', 'Bauer', 'Contreras', 'Knox', 'Schultz', 'Rodriquez', 'Sharpe', 'Haney', 'Good', 'Harrell', 'Terrell', 'Vincent', 'Malone', 'Ewing', 'Bryan', 'Jenkins', 'Lindsay', 'Gates', 'Stanton', 'Guerrero', 'Hubbard', 'Berg', 'Torres', 'Hooper', 'Ochoa', 'Smith', 'Gardner', 'Ewing', 'Lane', 'Robertson', 'Bowman', 'Robertson', 'Gilmore', 'Lamb', 'Odom', 'Hines', 'Young', 'Riddle', 'Herring', 'Peters', 'Jarvis', 'Bates', 'Quinn', 'Blackwell', 'Chavez', 'Ewing', 'Kinney', 'Fitzgerald', 'Delaney', 'Riddle', 'Mckenzie', 'Andrews', 'Foster', 'Richardson', 'Copeland', 'Chang', 'Mayer', 'Kerr', 'Knox', 'Strickland', 'House', 'Castillo', 'Talley', 'Ortega', 'Morin', 'Neal', 'Duncan', 'Morales']; }
    /** Test data. Returns an array of country names.
     @private
     @type {string[]} */
    static get Countries() { return ['Greece', 'Niger', 'Isle of Man', 'British Indian Ocean Territory', 'Chad', 'Cambodia', 'Chile', 'Uruguay', 'Yemen', 'Anguilla', 'Macao', 'Mozambique', 'Papua New Guinea', 'Mauritania', 'Albania', 'Belgium', 'Northern Mariana Islands', 'Saint Lucia', 'Slovenia', 'Maldives', 'Ecuador', 'Saint Lucia', 'Botswana', 'Morocco', 'Iran', 'Solomon Islands', 'Mongolia', 'Luxembourg', 'Martinique', 'Saint Pierre and Miquelon', 'Bosnia and Herzegovina', 'United States', 'French Polynesia', 'Gambia', 'Guernsey', 'Albania', 'Cyprus', 'Mauritius', 'Sandwich Islands', 'Venezuela', 'Bermuda', 'Slovenia', 'Moldova', 'Cuba', 'American Samoa', 'Lesotho', 'Micronesia', 'New Caledonia', 'Nepal', 'Lesotho', 'Slovenia', 'Cape Verde', 'Philippines', 'Haiti', 'Uganda', 'Kenya', 'Portugal', 'Malawi', 'New Caledonia', 'Mauritania', 'Vanuatu', 'Guam', 'Benin', 'Bermuda', 'Armenia', 'Vatican', 'Latvia', 'Tonga', 'Congo', 'Morocco', 'Kazakhstan', 'Ghana', 'Thailand', 'Kyrgyzstan', 'Cameroon', 'United Kingdom', 'Gibraltar', 'Senegal', 'French Polynesia', 'Antarctica', 'Trinidad and Tobago', 'Honduras', 'Cayman Islands', 'Yemen', 'Italy', 'Niger', 'Bangladesh', 'Madagascar', 'United Arab Emirates', 'Monaco', 'Spain', 'Latvia', 'Philippines', 'Romania', 'Bonaire', 'France', 'Guernsey', 'Ivory Coast', 'Burundi', 'Saint Barthelemy'];}
    /** Test data. Returns an array of department names.
     @private
     @type {string[]} */
    static get Departments() { return ['RnD', 'I.T.', 'Marketing', 'Sales', 'Finance']; } //  ['RnD', 'Production', 'I.T.', 'Marketing', 'Maintenance', 'Sales', 'Customer Service', 'Dispatch', 'Human Resources', 'Accounting and Finance', 'Purchasing',];
    /** Test data. Returns an array of boolean values.
     @private
     @type {boolean[]} */
    static get Booleans() { return [true, false, false, false, false, true, true, false, false, true, false, false, true, false, false, false, true, false, true, true, false, false, true, true, false, true, true, true, false, true, false, false, false, false, false, false, true, true, true, true, false, true, false, false, false, false, false, false, true, false, true, false, true, false, true, false, false, true, false, true, false, false, true, false, true, true, true, true, false, false, false, false, true, false, true, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false, false, true, false, false]; }
    /** Test data. Returns an array of numeric values.
     @private
     @type {number[]} */
    static get Numbers() { return [1877, 1375, 2653, 1090, 1550, 1963, 1305, 1021, 2020, 2867, 1034, 915, 857, 2001, 2965, 1737, 2802, 2099, 2671, 2559, 2515, 2803, 1895, 1854, 2852, 1421, 1734, 1140, 2840, 1802, 2285, 2517, 1263, 2285, 1249, 1430, 2651, 2705, 1820, 1385, 1404, 1035, 2901, 2745, 1612, 2377, 2506, 1860, 1805, 1678, 2235, 1803, 1813, 1291, 1498, 1184, 960, 2874, 1541, 1631, 1621, 1314, 2646, 1041, 2674, 2408, 2421, 1793, 1683, 1677, 2633, 2186, 1715, 1641, 1491, 1392, 2615, 1215, 1414, 840, 822, 1965, 2140, 2540, 1387, 1934, 2993, 2872, 2873, 1393, 1247, 2160, 1041, 1078, 1196, 1458, 2595, 1371, 917, 2703]; }
    /** Test data. Returns an array of Date values.
     @private
     @type {Date[]} */
    static get Dates() {
        if (!this.fDates) {
            this.fDates = [];
            let D = new Date();
            let D2;

            for (var i = 0; i < 100; i++) {
                D2 = new Date(tp.AddDays(D, i + 1).getTime());
                this.fDates.push(D2);
            }
        }

        return this.fDates;
    }

    /**
     Creates the Countries or Departments {@link tp.DataTable}
     @private
     @param {string} TableName The table name of the table to create
     @returns {tp.DataTable} Returns the new {@link tp.DataTable}
     */
    static CreateLookUpTable(TableName) {
        var A = tp.IsSameText(TableName, 'Countries') ? this.Countries : this.Departments;
        var Table = new tp.DataTable();
        Table.AutoGenerateGuidKeys = false;

        Table.AddColumn("Id", tp.DataType.Integer);
        Table.AddColumn("Name");

        for (var i = 0, ln = A.length; i < ln; i++) {
            Table.AddRow(i + 1, A[i]);
        }

        return Table;
    }
    /** Creates a lookup list of objects <code>{ Id: number, Name: string }</code> for Countries or Departments.
     @private
     @param {string} TableName The table name
     @returns {object[]} Returns a list of objects <code>{ Id: number, Name: string }</code>
     */
    static CreateLookUpList(TableName) {
        var A = tp.IsSameText(TableName, 'Countries') ? this.Countries : this.Departments;
        var List = [];

        for (var i = 0, ln = A.length; i < ln; i++) {
            List.push({ Id: i + 1, Name: A[i] });
        }

        return List;
    }

    /* look-up tables and lists */

    /** Gets the Countries {@link tp.DataTable}
     @type {tp.DataTable}
     */
    static get tblCountries() {
        if (!this.ftblCountries) {
            this.ftblCountries = this.CreateLookUpTable("Countries");
        }
        return this.ftblCountries;
    }
    /** Gets the Departments {@link tp.DataTable}
     @type {tp.DataTable}
     */
    static get tblDepartments() {
        if (!this.ftblDepartments) {
            this.ftblDepartments = this.CreateLookUpTable("Departments");
        }
        return this.ftblDepartments;
    }
    /** Gets the Countries list of objects <code>{ Id: number, Name: string }</code>
     @type {object[]}
     */
    static get CountryList() {
        if (!this.fCountryList) {
            this.fCountryList = this.CreateLookUpList("Countries");
        }
        return this.fCountryList;
    }
    /** Gets the Departments list of objects <code>{ Id: number, Name: string }</code>
     @type {object[]}
     */
    static get DepartmentList() {
        if (!this.fDepartmentList) {
            this.fDepartmentList = this.CreateLookUpList("Departments");
        }
        return this.fDepartmentList;
    }
 
    /* testing */
    /**
    Creates a test data table. Returns an object { Table: tp.DataTable, MSecs: Number } with the table and the elapsed msecs.
    @param {number} [RowCount=100] - Optional. Defaults to 100. The number of data rows in the resulting data table.
    @returns {Object} Returns a <code>{ Table: tp.DataTable, MSecs: number }</code> object
    */
    static CreateTestDataTable(RowCount = 100)  {
 
        var StartTime = +new Date();

        RowCount = RowCount || 100;
        var Table = new tp.DataTable();
        Table.AutoGenerateGuidKeys = false;

        Table.AddColumn("Id");
        Table.AddColumn("Code");
        Table.AddColumn("Name");
        Table.AddColumn("Age", tp.DataType.Integer);
        Table.AddColumn("DepartmentId", tp.DataType.Integer);
        Table.AddColumn("Salary", tp.DataType.Float);
        Table.AddColumn('Married', tp.DataType.Boolean);
        Table.AddColumn("CountryId", tp.DataType.Integer);
        Table.AddColumn("EntryDate", tp.DataType.Date);

        var Id, Code, Name, Age, Department, Salary, Married, Country, EntryDate;
        var Row, Rows = [];
        Rows.length = RowCount;

        for (var i = 0; i < RowCount; i++) {
            Id = i;
            Code = tp.PadLeft(i.toString(), "0", 6);
            Name = this.FirstNames[Math.floor(Math.random() * this.FirstNames.length)] + ' ' + this.LastNames[Math.floor(Math.random() * this.LastNames.length)];
            Age = Math.floor(Math.random() * 20) + 25;

            Department = Math.floor(Math.random() * this.Departments.length) + 1;
            Salary = this.Numbers[Math.floor(Math.random() * this.Numbers.length)] * Math.random();
            Married = this.Booleans[Math.floor(Math.random() * this.Booleans.length)];

            Country = Math.floor(Math.random() * this.Countries.length) + 1;
            EntryDate = this.Dates[Math.floor(Math.random() * this.Dates.length)];

            Row = new tp.DataRow(Table, [
                Id,
                Code,
                Name,
                Age,
                Department,
                Salary,
                Married,
                Country,
                EntryDate
            ]);

            Row.State = tp.DataRowState.Unchanged;

            Rows[i] = Row;
        }


        Table.Rows = Rows;

        var Result = {
            Table: Table,
            MSecs: +new Date() - StartTime
        };

        return Result;
    }
    /**
    Creates a test data table and displays that table to a html table. The html table becomes a child to a specified parent element.
    @param {string | HTMLElement} ElementOrSelector - The parent element
    @param {number} [RowCount=500] - Optional. The number of data rows in the resulting data table.
    */
    static TestDataTable(ElementOrSelector, RowCount = 500) {
        RowCount = RowCount || 500;

        var Data = this.CreateTestDataTable(RowCount);
        var Table = Data.Table;

        alert('start');

        var div = new tp.tpElement(ElementOrSelector);

        //div.Position = tp.Position.Absolute;
        //div.Y = 300;
        div.Width = 800;

        var table = div.AddComponent('table');
        table.StyleProp('border', '1px solid black');
        table.StyleProp('border-collapse', 'collapse');

        var th;
        var td;
        var i, ln, k, kln;

        var tr = table.AddComponent('tr');
        for (i = 0, ln = Table.ColumnCount; i < ln; i++) {
            th = tr.AddComponent('th');
            th.StyleProp('border', '1px solid black');
            th.StyleProp('border-collapse', 'collapse');
            th.StyleProp('padding', '5px');

            th.Text = Table.Columns[i].Title;
        }

        for (k = 0, kln = Table.RowCount; k < kln; k++) {
            tr = table.AddComponent('tr');

            for (i = 0, ln = Table.ColumnCount; i < ln; i++) {
                td = tr.AddComponent('td');
                td.StyleProp('border', '1px solid black');
                td.StyleProp('border-collapse', 'collapse');
                td.StyleProp('padding', '5px');
                td.Text = Table.Rows[k].Get(i);
            }
        }


    }
 
};


// Firefox v.69 does NOT support static keyword in fields
tp.TestData.fDates = null;
tp.TestData.ftblCountries = null;
tp.TestData.ftblDepartments = null;
tp.TestData.fCountryList = null;
tp.TestData.fDepartmentList = null;

//#endregion  
