//---------------------------------------------------------------------------------------
// column
//---------------------------------------------------------------------------------------

//#region tp.GridColumn

/**
A grid column. <br />
Produced markup
<pre>
    <div class="tp-Grid-Col tp-Object">
        <div class="tp-Text" draggable="true" title="Name">Name</div>
        <div class="tp-Sorter">▴</div>
        <div class="tp-Resizer"></div>
    </div>

    beneath the column lies the filter cell

    <div class="tp-Filter-Cell">
        <input type="text" />
    </div>

    in the grid footer lies the summary cell

    <div class="tp-Summary-Cell">
        sum=123
    </div>
</pre>
*/
tp.GridColumn = class extends tp.tpObject {

    /**
    Constructor <br />
    Produced markup
    @example
    <pre>
        <div class="tp-Grid-Col tp-Object">
            <div class="tp-Text" draggable="true" title="Name">Name</div>
            <div class="tp-Sorter">▴</div>
            <div class="tp-Resizer"></div>
        </div>

        beneath the column lies the filter cell

        <div class="tp-Filter-Cell">
            <input type="text" />
        </div>

        in the grid footer lies the summary cell

        <div class="tp-Summary-Cell">
            sum=123
        </div>
    </pre>
    @param {tp.Grid} Grid The owner grid
    @param {string | tp.DataColumn} NameOrDataColumn - The name of the data column or the data column itself. If a data column is passed the rest of the parameters are ignored.
    @param {string} [Text] - Optional. The display title.
     */
    constructor(Grid, NameOrDataColumn, Text) {
        super();

        this.fGrid = Grid;

        this.CreateHandle();
        this.InitializeFields();
        this.CreateControls();

        if (NameOrDataColumn instanceof tp.DataColumn) {
            this.fName = NameOrDataColumn.Name;
            this.fDataColumn = NameOrDataColumn;
            this.Text = this.DataColumn.Title;
            this.ToolTip = this.DataColumn.Title;
            this.Decimals = this.DataColumn.Decimals;
        } else {
            this.Name = NameOrDataColumn;
            this.Text = Text || this.Name;
            this.ToolTip = this.Text;
            this.Bind();
        }
    }



    /**
    Returns the combo-box of the inplace editor when this is a lookup column
    @protected
    @type {tp.ComboBox}
    */
    get ComboBox()  {
        if (!(this.fEditor instanceof tp.GridInplaceEditorComboBox)) {
            this.fEditorComboBox = new tp.GridInplaceEditorComboBox(this);
            this.fComboBox = this.fEditorComboBox.ComboBox;
            this.fEditor = this.fEditorComboBox;
        }

        return this.fComboBox;
    }
    /**
    Returns the locator inplace editor when this is a locator column.
    @protected
    @type {tp.GridInplaceEditorLocator}
    */
    get LocatorEditor() {
        if (tp.IsEmpty(this.fEditorLocator)) {
            this.fEditorLocator = new tp.GridInplaceEditorLocator(this);
            this.fEditor = this.fEditorLocator;
        }

        return this.fEditorLocator;
    }

    /* properties */
    /**
    Internal. The handle (HTMLElement) of this instance
    @type {HTMLElement}
    */
    get Handle() {
        return this.fHandle;
    }
    /**
    Internal. The grid this column belongs to.
    @type {tp.Grid}
    */
    get Grid() {
        return this.fGrid;
    }
    /**
    Returns the data column this grid column is associated with.
    @type {tp.DataColumn}
    */
    get DataColumn() {
        return this.fDataColumn;
    }
    /**
    Returns true if this grid column is bound to a data column
    @type {boolean}
    */
    get IsDataBound() {
        return !tp.IsEmpty(this.Grid) && this.Grid.IsDataBound && !tp.IsEmpty(this.DataColumn);
    }

    /**
    Gets or sets the column name (field name)
    @type {string}
    */
    get Name() {
        return this.DataColumn instanceof tp.DataColumn ? this.DataColumn.Name : this.fName;
    }
    set Name(v) {
        if (v !== this.fName) {
            this.fName = v;
            this.Changed('Name');
        }
    }
    /**
    Returns the data type of the column. One of the tp.DataType string constants.
    @type {string}
    */
    get DataType() {
        return this.DataColumn instanceof tp.DataColumn ? this.DataColumn.DataType : tp.DataType.Unknown;  
    }
    /**
    Gets or sets the number of decimal digits of this column
    @type {number}
    */
    get Decimals() {
        return this.fDecimals;
    }
    set Decimals(v) {
        if (v !== this.fDecimals) {
            this.fDecimals = v;
            this.Changed('Decimals');
        }
    }
    /**
    Gets or sets the alignment of this column. One of the tp.Alignment constants.
    @type {number}
    */
    get Alignment() {
        if (this.fIsAlignmentSet !== true) {
            if (this.IsLookUp) {
                var DisplayDataColumn = this.LookUpDisplayDataColumn;
                if (!tp.IsEmpty(DisplayDataColumn))
                    return tp.DataType.DefaultAlignment(DisplayDataColumn.DataType);
            } else if (this.DataColumn instanceof tp.DataColumn) {
                let AlignmentDataType = this.DataColumn.DisplayType === tp.ColumnDisplayType.CheckBox ? tp.DataType.Boolean: this.DataColumn.DataType
                return tp.DataType.DefaultAlignment(AlignmentDataType);
            }
        }

        return this.fAlignment;
    }
    set Alignment(v) {
        if (v !== this.fAlignment) {
            this.fAlignment = v;
            this.fIsAlignmentSet = true;
            this.Changed('Alignment');
        }
    }
    /**
    Gets or sets the title text (column caption)  of this column
    @type {string}
    */
    get Text() {
        return tp.IsHTMLElement(this.fTextContainer) ? this.fTextContainer.textContent : '';
    }
    set Text(v) {
        if (tp.IsHTMLElement(this.fTextContainer)) {
            this.fTextContainer.textContent = v;
        }
    }
    /**
    Gets or sets the tool-tip (hint) of the column
    @type {string}
    */
    get ToolTip() {
        return tp.IsHTMLElement(this.fTextContainer) ? this.fTextContainer.title : '';
    }
    set ToolTip(v) {
        if (tp.IsHTMLElement(this.fTextContainer)) {
            this.fTextContainer.title = v;
        }
    }
    /**
    Gets or sets the width of the column
    @type {number}
    */
    get Width() {
        return this.fWidth;
    }
    set Width(v) {
        this.SetWidth(v);
        this.Changed('Width');
    }
    /**
    Gets or sets a boolean value indicating whether a Date column should display date values in local date.
    @type {boolean}
    */
    get LocalDate()  {
        return this.fLocalDate === true;
    }
    set LocalDate(v) {
        v = Boolean(v);
        if (v !== this.LocalDate) {
            this.fLocalDate = v;
            this.Changed('LocalDate');
        }
    }
    /**
    Gets or sets a boolean value indicating whether a Date column should display seconds to.
    @type {boolean}
    */
    get DisplaySeconds() {
        return this.fDisplaySeconds === true;
    }
    set DisplaySeconds(v) {
        v = Boolean(v);
        if (v !== this.DisplaySeconds) {
            this.fDisplaySeconds = v;
            this.Changed('DisplaySeconds');
        }
    }

    /**
    Gets or sets a boolean value indicating whether the column is visible
    @type {boolean}
    */
    get Visible() {
        if (tp.IsEmpty(this.fVisible)) {
            return (this.DataColumn instanceof tp.DataColumn) ? this.DataColumn.Visible : true;
        }
        return this.fVisible;
    }
    set Visible(v) {
        if (v !== this.fVisible) {
            this.fVisible = v;
            this.Changed('Visible');
        }
    }
    /**
    Gets or sets a boolean value indicating whether the column is read-only
    @type {boolean}
    */
    get ReadOnly() {
        if ((this.DataColumn instanceof tp.DataColumn) && (this.DataColumn.ReadOnly === true))
            return true;
        return this.fReadOnly;
    }
    set ReadOnly(v) {
        if (v !== this.fReadOnly) {
            this.fReadOnly = v;
            this.Changed('ReadOnly');
        }
    }
    /**
    Gets or sets a boolean value indicating whether the column is resizable
    @type {boolean}
    */
    get Resizable() {
        return this.fResizable;
    }
    set Resizable(v) {
        if (v !== this.fResizable) {
            this.fResizable = v;
            this.fResizer.style.cursor = this.fResizable ? tp.Cursors.ResizeCol : tp.Cursors.Default;
            this.Changed('Resizable');
        }
    }
    /**
    Gets or sets a boolean value indicating whether the column is groupable
    @type {boolean}
    */
    get Groupable() {
        return this.fGroupable;
    }
    set Groupable(v) {
        if (v !== this.fGroupable) {
            this.fGroupable = v;
            this.Changed('Groupable');
        }
    }

    /**
    Returns the index of the tp.DataColumn in its data table.  
    @type {number}
    */
    get DataIndex() {
        if (this.Grid.IsDataBound && this.DataColumn instanceof tp.DataColumn) {
            return this.Grid.DataSource.Table.IndexOfColumn(this.DataColumn);
        }
        return -1;
    }
    /**
    Returns the index of this column among the group columns of the grid
    @type {number}
    */
    get GroupIndex() {
        return this.Grid ? this.Grid.IndexOfGroupColumn(this) : -1;
    }
    /**
    Returns the index of this column among the value columns of the grid
    @type {number}
    */
    get ValueIndex() {
        return this.Grid ? this.Grid.IndexOfValueColumn(this) : -1;
    }
    /**
    Returns the index of this column among the aggregate (summary) columns of the grid
    @type {number}
    */
    get AggregateIndex() {
        return this.Grid ? this.Grid.IndexOfAggregateColumn(this) : -1;
    }

    /**
    Returns true if this is a group column
    @type {boolean}
    */
    get IsGroupColumn() {
        return this.GroupIndex !== -1;
    }
    /**
    Returns true if this is a value column
    @type {boolean}
    */
    get IsValueColumn() {
        return this.ValueIndex !== -1;
    }
    /**
    Returns true if this is an aggregate (summary) column
    @type {boolean}
    */
    get IsAggregateColumn() {
        return this.AggregateIndex !== -1;
    }

    /**
    Gets or sets the inplace editor of this column
    @type {tp.GridInplaceEditor}
    */
    get Editor() {
        if (tp.IsEmpty(this.fEditor)) {
            this.fEditor = this.CreateEditor();
        }
        return this.fEditor;
    }
    set Editor(v) {
        if (v !== this.fEditor) {

            this.fEditorComboBox = null;
            this.fComboBox = null;
            this.fLookUpTable = null;
            this.fLookUpDisplayDataColumn = null;

            this.fEditor = v;

            if (this.fEditor instanceof tp.GridInplaceEditorComboBox) {
                this.fEditorComboBox = this.fEditor;
                this.fComboBox = this.fEditorComboBox.ComboBox;
            }
        }
    }

    /**
    Returns a string indicating the sort mode, i.e. '', 'asc', 'desc'
    @type {string}
    */
    get SortMode() {
        return this.fSortMode;
    }

    /**
    Returns a boolean value indicating whether this is a look-up column (a column with a look-up combo-box)
    @type {boolean}
    */
    get IsLookUp() {
        return this.fEditor instanceof tp.GridInplaceEditorComboBox;
    }


    /**
    Gets or sets a {@link tp.DataSource} to be to be used as the source for the item list. It accepts a {@link tp.DataTable} too and uses that table to create the {@link tp.DataSource}.
    @type {tp.DataSource | tp.DataTable}
    */
    get ListSource() {
        return this.ComboBox.ListSource;
    }
    set ListSource(v) {
        this.ComboBox.ListSource = v;
    }
    /**
    Gets or sets the name of the list datasource. Used in declarative scenarios.
    @type {string}
    */
    get ListSourceName() {
        return this.ComboBox.ListSourceName;
    }
    set ListSourceName(v) {
        this.ComboBox.ListSourceName = v;
    }
    /**
    Gets or sets the name of the value "field-name"/property of objects contained in the item list. <br />
    @type {string}
    */
    get ListValueField() {
        return this.ComboBox.ListValueField;
    }
    set ListValueField(v) {
        this.ComboBox.ListValueField = v;
    }
    /**
    Gets or sets the name of the display "field-name"/property of objects contained in the data list. <br />
    @type {string}
    */
    get ListDisplayField() {
        return this.ComboBox.ListDisplayField;
    }
    set ListDisplayField(v) {
        this.ComboBox.ListDisplayField = v;
    }

    /**
    Returns the look-up {@link tp.DataTable} table. Used when this is a look-up column
    @type {tp.DataTable}
    */
    get LookUpTable() {
        if (this.IsLookUp && tp.IsEmpty(this.fLookUpTable) && !tp.IsEmpty(this.ComboBox.ListSource)) {
            this.fLookUpTable = this.ComboBox.ListSource.Table;
        }

        return this.fLookUpTable;
    }
    /**
    Returns the look-up display {@link tp.DataColumn} data column. This data column belongs to the look-up table.
    @type {tp.DataColumn}
    */
    get LookUpDisplayDataColumn() {
        if (this.IsLookUp && tp.IsEmpty(this.fLookUpDisplayDataColumn) && !tp.IsEmpty(this.LookUpTable)) {
            this.fLookUpDisplayDataColumn = this.LookUpTable.FindColumn(this.ListDisplayField);
        }

        return this.fLookUpDisplayDataColumn;
    }

    /**
    Returns true when this is a locator column
    @type {boolean}
    */
    get IsLocator() {
        return this.fEditor instanceof tp.GridInplaceEditorLocator;
    }
    /**
    Returns the locator. Used when this is a locator column.
    @type {tp.Locator}
    */
    get Locator() {
        return this.LocatorEditor.Locator;
    }
    /**
    Gets or sets the locator descriptor name. Used when this is a locator column.
    @type {string}
    */
    get LocatorName() {
        return this.LocatorEditor.LocatorName;
    }
    set LocatorName(v) {
        this.LocatorEditor.LocatorName = v;
    }


    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridColumn';
    }

    /* overridables */
    /**
    Creates the handle (HTMLElement) of this instance
    @protected
    */
    CreateHandle() {
        this.fHandle = this.Grid.Document.createElement('div'); // tp.Doc.createElement('div');
        tp.SetObject(this.Handle, this);
        this.Handle.className = tp.Classes.GridColumn;

        tp.On(this.Handle, tp.Events.MouseDown, this);
        tp.On(this.Handle, tp.Events.MouseUp, this);
        tp.On(this.Handle, tp.Events.Click, this);
    }
    /**
    Initializes fields and properties    
    @protected
    */
    InitializeFields() {
        this.fLookUpTable = null;
        this.fLookUpDisplayDataColumn = null;

        this.fDecimals = 0;

        this.fAlignment = tp.Alignment.Left;
        this.fIsAlignmentSet = false;
        this.fWidth = 80;
        this.fLocalDate = true;
        this.fDisplaySeconds = false;
        this.fResizable = true;
        this.fGroupable = true;

        this.fResizeTimeStamp = null;
        this.fSortMode = '';
        this.fAggregate = tp.AggregateType.None;

        this.fReadOnly = false;
        this.fVisible = true;

    }
    /**
    Creates the HTMLElement elements of this column
    @protected
    */
    CreateControls() {
        let self = this;

        function CreateDiv(CssClasses) {
            let Result = self.Handle.ownerDocument.createElement('div');
            tp.GridColumn.SetInfo(Result, self);
            Result.className = CssClasses;
            return Result;
        }

        // text container
        this.fTextContainer = CreateDiv(tp.Classes.Text);
        this.Handle.appendChild(this.fTextContainer);
        this.fTextContainer.draggable = true;

        // sort indicator
        this.fSorter = CreateDiv(tp.Classes.Sorter);
        this.Handle.appendChild(this.fSorter);

        // resize handle
        this.fResizer = CreateDiv(tp.Classes.Resizer);
        this.Handle.appendChild(this.fResizer);

        // complement cell, used when a column is grouped
        this.fGroupCell = CreateDiv(tp.Classes.GroupCell);



        // filter cell
        this.fFilterCell = CreateDiv(tp.Classes.FilterCell);

        // filter text box
        this.fFilterCellTextBox = this.Handle.ownerDocument.createElement('input');
        this.fFilterCellTextBox.type = 'text';
        this.fFilterCellTextBox.spellcheck = false;
        tp.GridColumn.SetInfo(this.fFilterCellTextBox, this);
        this.fFilterCell.appendChild(this.fFilterCellTextBox);
        this.fFilterCellTextBox.className = tp.ConcatClasses(tp.Classes.FilterTextBox); // tp.Classes.NoBrowserAppearance, 

        // complement cell, used when a column is grouped
        this.fFilterGroupCell = CreateDiv(tp.Classes.GroupCell);



        // fSummaryCell
        this.fSummaryCell = CreateDiv(tp.Classes.SummaryCell);

        // complement cell, used when a column is grouped
        this.fSummaryGroupCell = CreateDiv(tp.Classes.GroupCell);

        // as drag source
        tp.On(this.fTextContainer, tp.Events.DragStart, this.FuncBind(this.DragStart));
        tp.On(this.fTextContainer, tp.Events.DragEnd, this.FuncBind(this.DragEnd));

        // as drop target
        tp.On(this.fTextContainer, tp.Events.DragEnter, this.FuncBind(this.DragEnter));
        tp.On(this.fTextContainer, tp.Events.DragLeave, this.FuncBind(this.DragLeave));
        tp.On(this.fTextContainer, tp.Events.DragOver, this.FuncBind(this.DragOver));
        tp.On(this.fTextContainer, tp.Events.DragDrop, this.FuncBind(this.DragDrop));

        tp.On(this.fFilterCellTextBox, tp.Events.InputChanged, this.FuncBind(this.FilterTextBox_TextChanged));
        tp.On(this.fSummaryCell, tp.Events.ContextMenu, this.FuncBind(this.FooterCell_ContextMenu));

    }

    /**
    Called whenever a property changes in order to inform the grid for the change
    @protected
    @param {string} PropName The name of the property that has changed
    */
    Changed(PropName) {
        if (this.Grid) {
            this.Grid.ColumnChanged(this, PropName);
        }
    }
    /**
    Sets the width of the column
    @protected
    @param {number} v The new width
    */
    SetWidth(v) {
        this.fWidth = v;
        if (tp.IsHTMLElement(this.Handle)) {
            this.Handle.style.width = tp.IsNumber(v) ? tp.px(v) : v;
            if (tp.IsHTMLElement(this.fFilterCell))
                this.fFilterCell.style.width = this.Handle.style.width;
            if (tp.IsHTMLElement(this.fSummaryCell))
                this.fSummaryCell.style.width = this.Handle.style.width;
        }
    }
    /**
    Internal. Sets the width to the three group cells (group cell, filter group cell and footer group cell)
    @param {number} v The new width
    */
    SetGroupCellWidth(v) {
        if (this.fGroupCell) {
            this.fGroupCell.style.width = tp.IsNumber(v) ? tp.px(v) : v;
            this.fFilterGroupCell.style.width = this.fGroupCell.style.width;
            this.fSummaryGroupCell.style.width = this.fGroupCell.style.width;
        }
    }
    /**
    Binds this grid column to data column
    */
    Bind() {
        this.fDataColumn = null;

        if (this.Grid.IsDataBound) {
            this.fDataColumn = this.Grid.DataSource.Table.FindColumn(this.Name);
        }
    }

    /**
    Creates and returns the inplace editor of this column
    @protected
    @returns {tp.GridInplaceEditor} Returns the {@link tp.GridInplaceEditor} inplace editor of this column
    */
    CreateEditor() {
        if (this.DataType === tp.DataType.Boolean || this.DataColumn.DisplayType === tp.ColumnDisplayType.CheckBox)
            return new tp.GridInplaceEditorCheckBox(this);

        return new tp.GridInplaceEditorTextBox(this);
    }
    /**
    Performs a look-up in the list-source table and returns the display value.
    @protected
    @param {any} v - The value to look-up for.
    @returns {any} Returns the display value of a specified value.
    */
    GetLookUpDisplayValue(v) {
        var Table = this.LookUpTable;
        if (!tp.IsEmpty(Table)) {
            var Row = Table.FindRow(this.ListValueField, v);
            if (!tp.IsEmpty(Row))
                return Row.Get(this.ListDisplayField);
        }

        return null;
    }

    // as drag source
    /**
    Event handler. Called when this is a drag source
    @protected
    @param {DragEvent} e The {@link DragEvent} object
    */
    DragStart(e) {
        this.Grid.DraggedColumn = this;
        if (e.dataTransfer) {
            e.dataTransfer.setData("text/plain", "just for the Firefox");
        }
        tp.AddClass(this.fTextContainer, tp.Classes.DragSource);
    }
    /**
    Event handler. Called when this is a drag source
    @protected
    @param {DragEvent} e The {@link DragEvent} object
    */
    DragEnd(e) {
        this.Grid.DraggedColumn = null;
        tp.RemoveClass(this.fTextContainer, tp.Classes.DragSource);
    }

    // as drop target
    /**
    Event handler. Called when this is a drop target
    @protected
    @param {DragEvent} e The {@link DragEvent} object
    */
    DragEnter(e) {
        if (this.Grid.AllowUserToOrderColumns) {
            var Column = this.Grid.DraggedColumn;
            if (Column && (Column !== this)) {
                tp.AddClass(this.Handle, tp.Classes.DropTarget);
                //this.StyleProp('outline', 'green double thick');
            }
        }
    }
    /**
    Event handler. Called when this is a drop target
    @protected
    @param {DragEvent} e The {@link DragEvent} object
    */
    DragLeave(e) {
        if (this.Grid.AllowUserToOrderColumns) {
            var Column = this.Grid.DraggedColumn;
            if (Column && (Column !== this)) {
                tp.RemoveClass(this.Handle, tp.Classes.DropTarget);
                //this.StyleProp('outline', '');
            }
        }
    }
    /**
    Event handler. Called when this is a drop target
    @protected
    @param {DragEvent} e The {@link DragEvent} object
    */
    DragOver(e) {
        if (this.Grid.AllowUserToOrderColumns) {
            var Column = this.Grid.DraggedColumn;
            if (Column && (Column !== this)) {
                if (e.preventDefault) {
                    e.preventDefault();        // Necessary. Allows us to drop.
                }

                if (e.dataTransfer) {
                    e.dataTransfer.dropEffect = 'move';
                }

                //return false;
            }
        }
    }
    /**
    Event handler. Called when this is a drop target
    @protected
    @param {DragEvent} e The {@link DragEvent} object
    */
    DragDrop(e) {
        if (this.Grid.AllowUserToOrderColumns) {
            var Column = this.Grid.DraggedColumn;
            if (Column && (Column !== this)) {
                tp.RemoveClass(this.Handle, tp.Classes.DropTarget);
                //this.StyleProp('outline', '');
                if (e.preventDefault) {
                    e.preventDefault();        // Necessary. Allows us to drop.
                }

                if (this.IsGroupColumn) {
                    this.Grid.ColumnGrouped(Column, this);
                } else {
                    this.Grid.ColumnReordered(Column, this);
                }


                //return false;
            }
        }
    }

    /**
    Event handler. Handles the column resizing by mouse
    @protected
    @param {MouseEvent} e A {@link MouseEvent} object
    */
    ResizeHandler(e) {
        this.Grid.fResizing = true;

        var self = this;
        var Grid = this.Grid;
        var Doc = this.Grid.Document;
        var Style = Doc.defaultView.getComputedStyle(this.Handle);

        var StartX = e.clientX;
        var StartY = e.clientY;

        var StartWidth = parseInt(Style.width, 10);
        var StartHeight = parseInt(Style.height, 10);

        var BodyCursor = Doc.body.style.cursor;
        Doc.body.style.cursor = tp.Cursors.ResizeCol;

        // ---------------------
        var Resize = function (e) {
            var w = StartWidth + e.clientX - StartX;
            var h = StartHeight + e.clientY - StartY;

            if ((w >= 15) && (w <= 3000)) {
                self.Width = w;
            }
        };
        // ---------------------
        var ResizeEnd = function (e) {
            self.fResizeTimeStamp = tp.Now().getTime();
            Doc.body.style.cursor = BodyCursor;
            Doc.documentElement.removeEventListener('mousemove', Resize, false);
            Doc.documentElement.removeEventListener('mouseup', ResizeEnd, false);

            self.Grid.fResizing = false;
            Resize(e);

            self.Grid.Render();
        };

        Doc.documentElement.addEventListener('mousemove', Resize, false);
        Doc.documentElement.addEventListener('mouseup', ResizeEnd, false);
    }
    /**
    Event handler. Handles the filtering.
    @protected
    @param {Event} e An {Event} object
    */
    FilterTextBox_TextChanged(e) {
        var Name = this.Name;
        var S = this.fFilterCellTextBox.value;
        var FilterInfoList = this.Grid.DataSource.FilterInfoList;
        var FilterItem; //: tp.DataTableFilterItem;
        var Flag = false;
        var Info, v, Res;

        //--------------------------
        var CancelFilter = function () {
            if (FilterInfoList.Contains(Name)) {
                FilterInfoList.Remove(Name);
                Flag = true;
            }
        };
        //--------------------------
        var GetStartFilterOp = function (Text, DefaultOperator) {
            var Result = {
                Text: Text,
                Operator: DefaultOperator || tp.FilterOp.None
            };

            if (!tp.IsBlank(Text)) {
                Text = tp.TrimStart(Text);
                if (tp.StartsWith(Text, '<>', false)) {
                    Result.Operator = tp.FilterOp.NotEqual;
                    Result.Text = Text.substring(2);
                } else if (tp.StartsWith(Text, '>=', false)) {
                    Result.Operator = tp.FilterOp.GreaterOrEqual;
                    Result.Text = Text.substring(2);
                } else if (tp.StartsWith(Text, '<=', false)) {
                    Result.Operator = tp.FilterOp.LessOrEqual;
                    Result.Text = Text.substring(2);
                } else if (tp.StartsWith(Text, '>', false)) {
                    Result.Operator = tp.FilterOp.Greater;
                    Result.Text = Text.substring(1);
                } else if (tp.StartsWith(Text, '=', false)) {
                    Result.Operator = tp.FilterOp.Equal;
                    Result.Text = Text.substring(1);
                } else if (tp.StartsWith(Text, '<', false)) {
                    Result.Operator = tp.FilterOp.Less;
                    Result.Text = Text.substring(1);
                } else if (Text.charAt(0) === '?') {
                    Result.Operator = tp.FilterOp.Contains;
                    Result.Text = Text.substring(1);
                } else if (Text.charAt(0) === '^') {
                    Result.Operator = tp.FilterOp.StartsWith;
                    Result.Text = Text.substring(1);
                } else if (Text.charAt(0) === '%') {
                    Result.Operator = tp.FilterOp.EndsWith;
                    Result.Text = Text.substring(1);
                }

            }

            return Result;
        };
        //--------------------------


        if (tp.IsBlank(S)) {
            CancelFilter();
            this.Grid.DoFilter();
            return;
        } else {
            switch (this.DataType) {
                case tp.DataType.String:
                    Info = GetStartFilterOp(S, tp.FilterOp.Contains);
                    FilterItem = FilterInfoList.FindOrAdd(this.Name, Info.Operator, Info.Text);  // NameOrIndexOrColumn, Operator, Value
                    if (this.IsLookUp) {
                        FilterItem.LookUpTable = this.ListSource.Table;
                        FilterItem.ListValueField = this.ListValueField;
                        FilterItem.ListDisplayField = this.ListDisplayField;
                    }
                    Flag = true;
                    break;
                case tp.DataType.Integer:
                    Info = GetStartFilterOp(S, tp.FilterOp.GreaterOrEqual);
                    if (this.IsLookUp) {
                        FilterItem = FilterInfoList.FindOrAdd(this.Name, Info.Operator, Info.Text);
                        FilterItem.DataType = tp.DataType.String;
                        FilterItem.LookUpTable = this.ListSource.Table;
                        FilterItem.ListValueField = this.ListValueField;
                        FilterItem.ListDisplayField = this.ListDisplayField;
                        Flag = true;
                    } else {
                        Res = tp.TryStrToInt(Info.Text);
                        if (Res.Result) {
                            FilterItem = FilterInfoList.FindOrAdd(this.Name, Info.Operator, Res.Value);
                            Flag = true;
                        }
                    }

                    break;
                case tp.DataType.Float:
                case tp.DataType.Decimal:
                    Info = GetStartFilterOp(S, tp.FilterOp.GreaterOrEqual);
                    Res = tp.TryStrToFloat(Info.Text);
                    if (Res.Result) {
                        FilterInfoList.FindOrAdd(this.Name, Info.Operator, Res.Value);
                        Flag = true;
                    }
                    break;
                case tp.DataType.Boolean:
                    v = tp.IsSameText(S, 'true') || tp.IsSameText(S, '1');
                    if (v) {
                        FilterInfoList.FindOrAdd(this.Name, tp.FilterOp.Equal, true);
                        Flag = true;
                    } else {
                        v = tp.IsSameText(S, 'false') || tp.IsSameText(S, '0');
                        if (v) {
                            FilterInfoList.FindOrAdd(this.Name, tp.FilterOp.Equal, false);
                            Flag = true;
                        }
                    }
                    break;
                case tp.DataType.Date:
                case tp.DataType.DateTime:
                    Info = GetStartFilterOp(S, tp.FilterOp.GreaterOrEqual);
                    if ((Info.Operator !== tp.FilterOp.None) && (Info.Text.length > 4)) {
                        Res = tp.TryParseDateTime(Info.Text);
                        if (Res.Result) {
                            FilterInfoList.FindOrAdd(this.Name, Info.Operator, Res.Value);
                            Flag = true;
                        }
                    }
                    break;
            }
        }

        if (Flag) {
            this.Grid.DoFilter();
        } else {
            CancelFilter();
        }
    }
    /**
    Event handler. Handles the displaying of the summary context menu. Redirects the event to the parent grid.
    @protected
    @param {MouseEvent} e The {@link MouseEvent} object
    */
    FooterCell_ContextMenu(e) {
        this.Grid.AnyContextMenu(e);
    }

    /* internal use only */
    /**
    Processes the this.CreateParams by applying its properties to the properties of this instance
    @param {object} [o] - Optional. The create params object to processs.
   */
    ProcessCreateParams(o) {
        o = o || {};

        for (var Prop in o) {
            if (!tp.IsFunction(o[Prop]) /* NO  && (Prop in this) */) {
                this[Prop] = o[Prop];
            }
        }
    }
    /**
    Renders a specified text in the summary cell in the footer of the grid
    @param {string} Text The text to render
    */
    RenderFooterSummary(Text) {
        this.fSummaryCell.textContent = Text;
        this.fSummaryCell.style.justifyContent = tp.Alignment.ToFlex(this.Alignment);
    }
    /**
     * For internal use only. Appends this column to grid group columns
     * @param {number} ColumnHeight The column height
     * @param {HTMLElement} Groups The groups element
     * @param {HTMLElement} Columns The columns element
     * @param {HTMLElement} Filter The filter element
     * @param {HTMLElement} Summary The summary element
     */
    AppendToGroupColumns(ColumnHeight, Groups, Columns, Filter, Summary) {
        this.Handle.style.height = tp.px(ColumnHeight);
        tp.AddClass(this.Handle, tp.Classes.Grouped);

        Groups.appendChild(this.Handle);
        Columns.appendChild(this.fGroupCell);
        Filter.appendChild(this.fFilterGroupCell);
        Summary.appendChild(this.fSummaryGroupCell);
    }
    /**
     * For internal use only. Appends this column to grid value columns
     * @param {HTMLElement} Columns The columns element
     * @param {HTMLElement} Filter The filter element
     * @param {HTMLElement} Summary The summary element
    */
    AppendToValueColumns(Columns, Filter, Summary) {
        this.Handle.style.height = '';
        tp.RemoveClass(this.Handle, tp.Classes.Grouped);

        if (this.Visible === true) {
            Columns.appendChild(this.Handle);
            Filter.appendChild(this.fFilterCell);
            Summary.appendChild(this.fSummaryCell);

            tp.Enabled(this.fFilterCellTextBox, tp.DataType.IsSortableType(this.DataType));
        }
    }
    /**
    Removes this column from its parent HTMLElement
    */
    RemoveFromDom() {
        tp.Remove(this.fSummaryGroupCell);
        tp.Remove(this.fSummaryCell);

        tp.Remove(this.fFilterGroupCell);
        tp.Remove(this.fFilterCell);

        tp.Remove(this.fGroupCell);
        tp.Remove(this.Handle);
    }

    /* public */
    /**
    Returns a string representation of this instance.
    @returns {string} Returns a string representation of this instance.
    */
    toString() {
        return this.Name;
    }
    /**
    Implementation of the DOM EventListener interface. <br />
    For handling all DOM element events. Either when this is a DOM element and the sender (target) of the event is this.Handle
    or when the sender (target) of the event is any other object and listener is this instance.
    @see {@ling http://www.w3.org/TR/DOM-Level-2-Events/events.html#Events-EventListener|specification}
    @param {Event} e The {@link Event} object
     */
    handleEvent(e) {
        let el = e.target;
        let T = tp.Events.ToTripous(e.type);
        let Node;


        switch (T) {

            case tp.Events.MouseDown:
                if (tp.IsHTMLElement(el)) {
                    let Column = tp.GridColumn.GetInfo(el);
                    if (Column === this && tp.HasClass(el, tp.Classes.Resizer)) {
                        if (this.Resizable) {
                            this.Grid.HideEditor(false);
                            this.ResizeHandler(e);
                            tp.CancelEvent(e);
                        }
                    }
                }
                break;

            case tp.Events.Click:
                tp.CancelEvent(e);

                // prevent the actions taken when clicked, if we are just after a resizing
                if (this.fResizeTimeStamp) {
                    var TimeStamp = tp.Now().getTime();

                    if (TimeStamp - this.fResizeTimeStamp < 1000) {
                        this.Grid.fResizing = false;
                        this.fResizeTimeStamp = null;
                        //return false;
                        e.returnValue = false;
                        return;
                    }
                }


                if (tp.DataType.IsSortableType(this.DataType)) {
                    this.Sort();
                }
                break;
        }

    }

    /**
    Sorts on this column.
    @param {string} [Mode] - Optional. Empty string cancels any existing sorting. The words 'asc' and 'desc' can be used for sorting ascending and descending respectively.
    */
    Sort(Mode) {
        if (this.Visible && tp.DataType.IsSortableType(this.DataType)) {
            var S = '';

            if (!tp.IsEmpty(Mode)) {
                if (tp.IsSameText('ASC', Mode))
                    S = 'asc';
                else if (tp.IsSameText('DESC', Mode))
                    S = 'desc';
            } else {
                S = this.fSortMode;
                if (S === '') {
                    S = 'asc';
                } else if (S === 'asc') {
                    S = 'desc';
                } else if (S === 'desc') {
                    S = '';
                }
            }

            if (S !== this.fSortMode) {
                this.fSortMode = S;
                S = tp.IsBlank(S) ? '' : (S === 'asc' ? '&utrif;' : '&dtrif;');

                this.fSorter.innerHTML = S;
                this.Grid.DoSort();
            }
        }

    }

    /**
    Returns a specified value formatted as text
    @param {any} v The value to format
    @returns {string} Returns a specified value formatted as text
    */
    Format(v) {
        var v2;

        if (this.IsLookUp && !tp.IsEmpty(this.LookUpDisplayDataColumn)) {
            var LookUpDataColumn = this.LookUpDisplayDataColumn;
            v2 = this.GetLookUpDisplayValue(v);
            return LookUpDataColumn.Format(v2, true);
        }

        if (this.DataColumn) {
            return this.DataColumn.Format(v, true);
        }

        return tp.Db.Format(v, this.DataType, this.DataColumn.DisplayType, true, this.Decimals, this.LocalDate, this.DisplaySeconds);
    }
    /**
    Converts a specified string into a primitive value (or a date-time)
    @param {string} S - The text to parse
    @returns {any} Returns the value after parsing the specified text.
    */
    Parse(S) {
        if (this.DataColumn) {
            return this.DataColumn.Parse(S);
        } else {
            return tp.Db.Parse(S, this.DataType);
        }

    }
    /**
    Renders the value this column represents, to a grid cell
    @param {HTMLElement} Cell - The {@link HTMLElement} dom element used as the cell for displaying the value
    @param {tp.DataRow} Row - The {@link tp.DataRow} data-row to get the value from
    */
    Render(Cell, Row) {
        var v = this.GetValue(Row);
        Cell.textContent = this.Format(v);

    }
    /**
    Returns the value this column represents, from a specified data-row
    @param {tp.DataRow} Row The row to operate on.
    @returns {any} Returns the value this column represents, from a specified data-row
    */
    GetValue(Row) {
        var v = this.Grid.DataSource.GetValue(Row, this.DataIndex);
        return v;
    }
    /**
    Puts a value this column represents, to a specified data-row
    @param {tp.DataRow} Row The row to operate on.
    @param {any} v The value to set.
    */
    SetValue(Row, v) {
        this.Grid.DataSource.SetValue(Row, this.DataIndex, v);
    }


    /* static */
    /**
    Returns the grid column associated to a DOM element, if any or null. 
    @static
    @param {string | Node} el The DOM element
    @returns {tp.GridColumn} Returns the grid column associated to a DOM element, if any or null
    */
    static GetInfo(el) {
        if (tp.IsString(el))
            el = tp.Select(el);

        if (!tp.IsEmpty(el)) {
            if (el['__Column'] && el['__Column'] instanceof tp.GridColumn)
                return el['__Column'];
        }

        return null;
    }
    /**
    Associates a DOM element to a grid column.
    @static
    @param {string | Node} el The DOM element
    @param {tp.GridColumn} v  The {@link tp.GridColumn}
    */
    static SetInfo(el, v) {
        if (tp.IsString(el))
            el = tp.Select(el);

        if (!tp.IsEmpty(el))
            el['__Column'] = v;
    }
    /**
    Returns true if a specified DOM element is associated to a tp.GridColumn
    @static
    @param {string | Node} el The DOM element
    @returns {boolean} Returns true if a specified DOM element is associated to a tp.GridColumn
    */
    static HasInfo(el) {
        return tp.GridColumn.GetInfo(el) !== null;
    }
};

/* protected */
/** Field.
* @protected
* @type {tp.Grid}
*/
tp.GridColumn.prototype.fGrid;
/** Field.
* @protected
* @type {HTMLElement}
*/
tp.GridColumn.prototype.fHandle;
/** Field.
* @protected
* @type {tp.DataColumn}
*/
tp.GridColumn.prototype.fDataColumn;

/** Field.
* @protected
* @type {tp.GridInplaceEditor}
*/
tp.GridColumn.prototype.fEditor;
/** Field. The inplace editor when this is a lookup column
* @protected
* @type {tp.GridInplaceEditorComboBox}
*/
tp.GridColumn.prototype.fEditorComboBox;
/** Field.
* @protected
* @type {tp.ComboBox}
*/
tp.GridColumn.prototype.fComboBox;
/** Field. The combo-box of the inplace editor when this is a lookup column
* @protected
* @type {tp.DataTable}
*/
tp.GridColumn.prototype.fLookUpTable;
/** Field.
* @protected
* @type {tp.DataColumn}
*/
tp.GridColumn.prototype.fLookUpDisplayDataColumn;
/** Field. The inplace editor when this is a locator column
* @protected
* @type {tp.GridInplaceEditorLocator}
*/
tp.GridColumn.prototype.fEditorLocator;

/** Field.
* @protected
* @type {string}
*/
tp.GridColumn.prototype.fName;
/** Field.
* @protected
* @type {number}
*/
tp.GridColumn.prototype.fDecimals;
/** Field. One of the {@link tp.Alignment} constants.
* @protected
* @type {tp.Alignment}
*/
tp.GridColumn.prototype.fAlignment;
/** Field.
* @protected
* @type {boolean}
*/
tp.GridColumn.prototype.fIsAlignmentSet;
/** Field. Sould be undefined initially
* @protected
* @type {number}
*/
tp.GridColumn.prototype.fWidth;
/** Field.
* @protected
* @type {boolean}
*/
tp.GridColumn.prototype.fLocalDate;
/** Field.
* @protected
* @type {boolean}
*/
tp.GridColumn.prototype.fDisplaySeconds;
/** Field.
* @protected
* @type {boolean}
*/
tp.GridColumn.prototype.fVisible;
/** Field.
* @protected
* @type {boolean}
*/
tp.GridColumn.prototype.fReadOnly;
/** Field.
* @protected
* @type {boolean}
*/
tp.GridColumn.prototype.fResizable;
/** Field.
* @protected
* @type {boolean}
*/
tp.GridColumn.prototype.fGroupable;

/** Field.
* @protected
* @type {number}
*/
tp.GridColumn.prototype.fResizeTimeStamp;
/** Field. One of '', 'asc', 'desc'
* @protected
* @type {string}
*/
tp.GridColumn.prototype.fSortMode;

/* internal */
/** Field.
* @protected
* @type {tp.AggregateType}
*/
tp.GridColumn.prototype.fAggregate;

/*

<div class="tp-Grid-Col tp-Object">
    <div class="tp-Text" draggable="true" title="Name">Name</div>
    <div class="tp-Sorter">▴</div>
    <div class="tp-Resizer"></div>
</div>

beneath the column lies the filter cell

<div class="tp-Filter-Cell">
    <input type="text" />
</div>

in the grid footer lies the summary cell

<div class="tp-Summary-Cell">
    sum=123
</div>

*/

/**
The HTMLElement that contains the column title text
@protected
@type {HTMLElement}
*/
tp.GridColumn.prototype.fTextContainer;
/**
The HTMLElement that when clicked sorts the column
@protected
@type {HTMLElement}
*/
tp.GridColumn.prototype.fSorter;
/**
The HTMLElement used in resizing the column
@protected
@type {HTMLElement}
*/
tp.GridColumn.prototype.fResizer;

/**
The HTMLElement that contains the filter text box
@protected
@type {HTMLElement}
*/
tp.GridColumn.prototype.fFilterCell;
/**
The text-box of the filter cell
@protected
@type {HTMLInputElement}
*/
tp.GridColumn.prototype.fFilterCellTextBox;
/**
The HTMLElement that displays the summary (aggregate function) text in the footer of the grid
@protected
@type {HTMLElement}
*/
tp.GridColumn.prototype.fSummaryCell;

/**
The HTMLElement that is added as a complement when this column is a group column
@protected
@type {HTMLElement}
*/
tp.GridColumn.prototype.fGroupCell;
/**
The HTMLElement that is added in the filter row as a complement when this column is a group column
@protected
@type {HTMLElement}
*/
tp.GridColumn.prototype.fFilterGroupCell;
/**
The HTMLElement that is added in the footer row (summary) as a complement when this column is a group column
@protected
@type {HTMLElement}
*/
tp.GridColumn.prototype.fSummaryGroupCell;

//#endregion

//---------------------------------------------------------------------------------------
// in-place editors
//---------------------------------------------------------------------------------------

//#region  tp.GridInplaceEditor

/**
The base class for any inplace editor of the tp.Grid
*/
tp.GridInplaceEditor = class extends tp.tpObject {

    /**
    Constructor.
    @param {tp.GridColumn} Column - The {@link tp.GridColumn} column this inplace editor is associated with.
    */
    constructor(Column) {
        super();

        this.fColumn = Column;
        this.CreateControl();
    }



    /* properties */
    /**
    The column this inplace editor is associated with
    @type {tp.GridColumn}
    */
    get Column() {
        return this.fColumn;
    }
    /**
    Returns the control (tp.Control perhaps) this inplace editor uses.
    @type {tp.tpElement}
    */
    get Control() {
        return this.fControl;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridInplaceEditor';
    }

    /* internal */
    /**
    Creates the control and assigns the proper protected field
    @protected
    */
    CreateControl() {
    }
    /**
    Shows the inplace editor by positioning the control to the cell (HTMLElement)
    @protected
    */
    ShowControl() {
    }
    /**
    Hides the inplace editor
    @protected
    @param {boolean} PostChanges The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    HideControl(PostChanges) {
    }
    /**
    Renders the value (the protected field) to the cell.
    @protected
    @param {boolean} PostChanges The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    RenderCell(PostChanges) {
    }
    /**
    Called by the Show() after the Cell, Row and Value are assigned.
    @protected
    */
    CellAssigned() {

    }

    /* methods */
    /**
    Shows the inplace editor by positioning the control to a specified data cell (HTMLElement)
    @param {HTMLElement} Cell - The container cell for the editor
    */
    Show(Cell) {
        if (this.Column.DataType === tp.DataType.Unknown
            || this.Column.DataType === tp.DataType.Memo
            || this.Column.DataType === tp.DataType.Blob) {
            return;
        }

        this.fCell = Cell;
        this.fRow = tp.GetElementInfo(Cell).Node.Row; // elCell.__tpInfo.Node.Row;
        this.fValue = this.Column.GetValue(this.fRow);

        if (tp.IsEmpty(this.Control)) {
            this.CreateControl();

            if (!tp.IsEmpty(this.Control)) {                
                this.Control.Y = -10000;
            }
        }

        this.CellAssigned();

        if (!tp.IsEmpty(this.Control)) {
            
            if (this.Control.ParentHandle !== Cell)
                Cell.appendChild(this.Control.Handle);
 
            this.Control.Position = 'absolute';
            this.Control.Handle.style.top = 0;
            this.Control.Handle.style.left = 0;
            this.Control.Handle.style.height = '100%';
            this.Control.Handle.style.width = '100%';
            this.Control.Handle.style.display = '';

/*
            if (tp.IsEmpty(this.Control.Parent)) {
                this.Control.SetParent(Cell.ownerDocument.body);
            }

            var R = tp.BoundingRect(Cell);

            this.Control.X = R.X + 1;
            this.Control.Y = R.Y + 1;
            this.Control.Width = R.Width - 2;
            this.Control.Height = R.Height - 1;
 */

            if (this.Control instanceof tp.Control) {
                if (tp.IsEmpty(this.Control.DataSource))
                    this.Control.DataSource = this.Column.Grid.DataSource;
            }
 
            this.ShowControl();

            
        }        
    }
    /**
    Hides the inplace editor
    @param {boolean} PostChanges - The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    Hide(PostChanges) {
        this.Column.Grid.AlteringData = true;
        try {
            this.HideControl(PostChanges);
            this.RenderCell(PostChanges);

            if (!tp.IsEmpty(this.Control)) {
                //this.Control.Y = -10000;       
                //this.Control.ParentHandle = null;
                this.Control.Handle.style.display = 'none';
            }
 
            if (this.Column.IsAggregateColumn) {
                this.Column.Grid.Render();
            }
        } finally {
            this.Column.Grid.AlteringData = false;
        }

    }
    /**
    Returns true when a specifield HTMLElement is contained directly or indirectly by the control of this inplace editor.
    @param {HTMLElement} el - The element to check.
    @returns {boolean} Returns true when a specifield HTMLElement is contained directly or indirectly by the control of this inplace editor.
    */
    ContainsHandle(el) {
        if (!tp.IsEmpty(this.Control) && tp.IsElement(this.Control.Handle))
            if (tp.ContainsElement(this.Control.Handle, el))
                return true;

        return false;
    }
};
 

/* protected */
/** Field
 * @protected
 * @type {tp.GridColumn}
 */
tp.GridInplaceEditor.prototype.fColumn;
/** Field
* @protected
* @type {tp.tpElement}
*/
tp.GridInplaceEditor.prototype.fControl;

/** Field, it is valid just while the editor is showing the control, that is after Show()
* @protected
* @type {HTMLElement}
*/
tp.GridInplaceEditor.prototype.fCell;           
/** Field, it is valid just while the editor is showing the control, that is after Show()
* @protected
* @type {tp.DataRow}
*/
tp.GridInplaceEditor.prototype.fRow;        
/** Field, it is valid just while the editor is showing the control, that is after Show()
* @protected
* @type {any}
*/
tp.GridInplaceEditor.prototype.fValue;                  

//#endregion

//#region  tp.GridInplaceEditorTextBox

/**
A text-box inplace editor for the tp.Grid
*/
tp.GridInplaceEditorTextBox = class extends tp.GridInplaceEditor {

    /**
    Constructor.
    @param {tp.GridColumn} Column - The {@link tp.GridColumn} column this inplace editor is associated with.
    */
    constructor(Column) {
        super(Column);
    }

 

    /* properties */
    /**
    Returns the text-box
    @type {tp.TextBox}
    */
    get TextBox() {
        return this.fTextBox;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridInplaceEditorTextBox';
    }

    /* internal */
    /**
    Creates the control and assigns the proper protected field
    @protected
    @override
    */
    CreateControl() {
        this.fTextBox = new tp.TextBox(null, null);
        this.fTextBox.AddClass(tp.Classes.GridInplaceEditor);
        this.fTextBox.AddClass(tp.Classes.NoBrowserAppearance);
        this.fTextBox.TextAlign = tp.Alignment.ToText(this.Column.Alignment);

        this.fControl = this.fTextBox;
    }
    /**
    Shows the inplace editor by positioning the control to the {@link HTMLElement} cell
    @protected
    @override
    */
    ShowControl() {
        var self = this;

        this.fTextBox.Text = this.Column.Format(this.fValue);
        this.fTextBox.Focus();

        setTimeout(() => {
            self.fTextBox.Select();       // its a text-box
        }, 0);
    }
    /**
    Hides the inplace editor
    @protected
    @override
    @param {boolean} PostChanges The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    HideControl(PostChanges) {
        if (PostChanges === true) {
            var sOld = this.Column.Format(this.fValue);
            var S = this.fTextBox.Text;
            if (S !== sOld) {
                try {
                    var v = this.Column.Parse(S);
                    this.Column.SetValue(this.fRow, v);
                } catch (e) {
                    tp.ErrorNote(tp.ExceptionText(e));
                }



            }
        }
    }
    /**
    Renders the value (the protected field) to the cell.
    @protected
    @override
    @param {boolean} PostChanges The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    RenderCell(PostChanges) {
        if (PostChanges === true)
            this.Column.Render(this.fCell, this.fRow);
    }

};

/** Field
 * @protected
 * @type {tp.TextBox}
 */
tp.GridInplaceEditorTextBox.prototype.fTextBox;

//#endregion

//#region  tp.GridInplaceEditorCheckBox

/**
A check-box inplace editor for the tp.Grid
*/
tp.GridInplaceEditorCheckBox = class extends tp.GridInplaceEditor {

    /**
    Constructor.
    @param {tp.GridColumn} Column - The {@link tp.GridColumn} column this inplace editor is associated with.
    */
    constructor(Column) {
        super(Column);
    }


 

    /* properties */
    /**
    Returns the check-box
    @type {HTMLInputElement}
    */
    get CheckBox() {
        return this.fCheckBox;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridInplaceEditorCheckBox';
    }


    /* internal */
    /**
    Creates the control and assigns the proper protected field
    @protected
    @override
    */
    CreateControl() {
        this.fControl = new tp.tpElement(null, null);
        this.fControl.AddClass(tp.Classes.GridInplaceEditorCheckBox);

        this.fCheckBox = this.Column.Handle.ownerDocument.createElement('input');
        this.fCheckBox.type = 'checkbox';
        this.fControl.Handle.appendChild(this.fCheckBox);
    }
    /**
    Shows the inplace editor by positioning the control to the cell {@link HTMLElement}
    @protected
    @override
    */
    ShowControl() {
        this.fCheckBox.checked = Boolean(this.fValue);
        this.fCheckBox.focus();
    }
    /**
    Hides the inplace editor
    @protected
    @override
    @param {boolean} PostChanges - The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    HideControl(PostChanges) {
        if (PostChanges === true) {
            if (this.fCheckBox.checked !== Boolean(this.fValue)) {
                this.Column.SetValue(this.fRow, this.fCheckBox.checked);
            }
        }
    }
    /**
    Renders the value (the protected field) to the cell.
    @protected
    @param {boolean} PostChanges - The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    RenderCell(PostChanges) {
        if (PostChanges === true)
            this.Column.Render(this.fCell, this.fRow);
    }

};
/** Field
* @protected
* @type {HTMLInputElement}
*/
tp.GridInplaceEditorCheckBox.prototype.fCheckBox;
//#endregion

//#region  tp.GridInplaceEditorComboBox
/**
A look-up combo-box inplace editor for the tp.Grid
*/
tp.GridInplaceEditorComboBox = class extends tp.GridInplaceEditor {

    /**
    Constructor.
    @param {tp.GridColumn} Column - The {@link tp.GridColumn} column this inplace editor is associated with.
    */
    constructor(Column) {
        super(Column);
    }
 
    /* properties */
    /**
    Returns the combo-box
    @type {tp.ComboBox}
    */
    get ComboBox() {
        return this.fComboBox;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridInplaceEditorComboBox';
    }


    /* internal */
    /**
    Creates the control and assigns the proper protected field
    @protected
    @override
    */
    CreateControl() {
        this.fComboBox = new tp.ComboBox(null, null);
        this.fComboBox.AddClass(tp.Classes.GridInplaceEditor);
        this.fComboBox.ListOnly = true;

        //this.fTextBox.value = v;

        this.fControl = this.fComboBox;
    }
    /**
    Shows the inplace editor by positioning the control to the cell {@link HTMLElement}
    @protected
    @override
    */
    ShowControl() {
        if (tp.IsBlank(this.fComboBox.DataField)) {
            this.fComboBox.DataField = this.Column.Name;
            this.fComboBox.DataSource = this.Column.Grid.DataSource;
        }

        this.fComboBox.ZIndex = tp.ZIndex(this.fCell) + 100; //1000;
    }
    /**
    Hides the inplace editor
    @protected
    @override
    @param {boolean} PostChanges - The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    HideControl(PostChanges) {
        this.fComboBox.Close();
    }
};

/** Field
* @protected
* @type {tp.ComboBox}
*/
tp.GridInplaceEditorComboBox.prototype.fComboBox;

//#endregion

//#region  tp.GridInplaceEditorLocator
/**
A locator inplace editor for the grid
*/
tp.GridInplaceEditorLocator = class extends tp.GridInplaceEditor {

    /**
    Constructor.
    @param {tp.GridColumn} Column - The {@link tp.GridColumn} column this inplace editor is associated with.
    */
    constructor(Column) {
        super(Column);
    }



    /* properties */

    /**
    Returns the locator. <br />
    NOTE: Locators are created and kept by the grid
    @type {tp.Locator}
    */
    get Locator() {
        if (tp.IsEmpty(this.fLocator)) {
            this.fLocator = this.Column.Grid.FindLocator(this.LocatorName);
        }
        return this.fLocator;
    }
    /**
    Gets or sets the locator descriptor name.<br />
    NOTE: Locators are created and kept by the grid
    @type {string}
    */
    get LocatorName() {
        return this.fLocatorName;
    }
    set LocatorName(v) {
        if (v !== this.LocatorName) {
            this.fLocatorName = v;
            this.fLocator = this.Column.Grid.FindLocator(this.LocatorName);
        }
    }
    /**
    Returns true if this inplace editor a valid locator inplace editor.
    @type {boolean}
    */
    get IsValidLocator() {
        return !tp.IsEmpty(this.Locator)
            && this.Locator.Descriptor.Name !== tp.NO_NAME
            && this.Locator.Descriptor.Fields.length > 0;
    }


    /* private */

    /** Event handler
     * @private
     * @param {Event} e The {@link Event} event object
     */
    AnyButton_Click(e) {
        if (e.target === this.btnZoom) {
            // TODO: Zoom
        } else if (e.target === this.btnList) {
            e.stopPropagation();
            this.Column.Locator.ShowList(this.fControl.Handle);            
        }
    }
    /** Event handler
     * @private
     * @param {Event} e The {@link Event} event object
     */
    Box_Enter(e) {
        let textBox = e.target;

        if (!this.Column.ReadOnly && this.IsValidLocator) {
            let FieldDes = this.Locator.Descriptor.FindByDataField(this.Column.Name);

            this.Locator.Controls.Set(FieldDes, textBox);
            this.Locator.Box_Enter(this.Column.Grid, textBox, FieldDes);
        }
    }
    /** Event handler
     * @private
     * @param {Event} e The {@link Event} event object
     */
    Box_Leave(e) {
        let textBox = e.target;

        if (!this.Column.ReadOnly && this.IsValidLocator) {
            let FieldDes = this.Locator.Descriptor.FindByDataField(this.Column.Name);
            this.Locator.Box_Leave(this.Column.Grid, textBox, FieldDes);
        }
    }
    /** Event handler
     * @private
     * @param {KeyboardEvent} e The {@link KeyboardEvent} event object
     */
    Box_KeyDown(e) {
        let textBox = e.target;

        if (!this.Column.ReadOnly && this.IsValidLocator) {
            let FieldDes = this.Locator.Descriptor.FindByDataField(this.Column.Name);
            this.Locator.Box_KeyDown(this.Column.Grid, textBox, FieldDes, e);
        }
    }
    /** Event handler
     * @private
     * @param {KeyboardEvent} e The {@link KeyboardEvent} event object
     */
    Box_KeyPress(e) {
        let textBox = e.target;

        if (!this.Column.ReadOnly && this.IsValidLocator) {
            let FieldDes = this.Locator.Descriptor.FindByDataField(this.Column.Name);
            this.Locator.Box_KeyPress(this.Column.Grid, textBox, FieldDes, e);
        }
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridInplaceEditorLocator';
    }

    /* internal */
    /**
    Creates the control and assigns the proper protected field
    @protected
    @override
    */
    CreateControl() {

        // control
        this.fControl = new tp.tpElement(null, null);
        this.fControl.AddClass(tp.Classes.GridInplaceEditorLocator);

        // textbox
        this.fTextBox = this.Column.Handle.ownerDocument.createElement('input');
        this.fTextBox.type = 'text';
        this.fTextBox.spellcheck = false;
        this.fControl.Handle.appendChild(this.fTextBox);

        tp.On(this.fTextBox, tp.Events.Focus, this.FuncBind(this.Box_Enter));
        tp.On(this.fTextBox, tp.Events.LostFocus, this.FuncBind(this.Box_Leave));
        tp.On(this.fTextBox, tp.Events.KeyDown, this.FuncBind(this.Box_KeyDown));
        tp.On(this.fTextBox, tp.Events.KeyPress, this.FuncBind(this.Box_KeyPress));

        // buttons
        this.btnList = this.Column.Handle.ownerDocument.createElement('div');
        this.btnList.className = tp.Classes.Btn;
        this.btnList.tabIndex = 0;
        this.btnList.innerHTML = '&dtrif;'; // tp.IcoChars.LargeButtonDown; //

        this.btnZoom = this.Column.Handle.ownerDocument.createElement('div');
        this.btnZoom.className = tp.Classes.Btn;
        this.btnZoom.tabIndex = 0;
        this.btnZoom.innerHTML = tp.IcoChars.Find;

        this.btnList.addEventListener('click', this.FuncBind(this.AnyButton_Click));
        this.btnZoom.addEventListener('click', this.FuncBind(this.AnyButton_Click));

        this.fControl.Handle.appendChild(this.btnList);
        this.fControl.Handle.appendChild(this.btnZoom);
    }
    /**
    Shows the inplace editor by positioning the control to the cell {@link HTMLElement}
    @protected
    @override
    */
    ShowControl() {
        this.Locator.Control = this.Column.Grid;
        this.Locator.Initialize();            

        if (this.Locator.Initialized) {
            // layout
            let ButtonWidth = 0;
            let R; // tp.Rect

            if (tp.Visible(this.btnZoom)) {
                R = tp.BoundingRect(this.btnZoom);
                ButtonWidth += R.Width;
            }

            if (tp.Visible(this.btnList)) {
                R = tp.BoundingRect(this.btnList);
                ButtonWidth += R.Width;
            }

            if (ButtonWidth > 0)
                ButtonWidth += 3;

            let W = tp.BoundingRect(this.Control.Handle).Width - ButtonWidth;
            this.fTextBox.style.width = tp.px(W);

            setTimeout((box) => {
                box.select();
                box.focus();                
            }, 0, this.fTextBox);
        }
    }
    /**
    Hides the inplace editor
    @protected
    @param {boolean} PostChanges The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    HideControl(PostChanges) {
    }
};

/** Field
* @protected
* @type {tp.Locator}
*/
tp.GridInplaceEditorLocator.prototype.fLocator;
/** Field
* @protected
* @type {string}
*/
tp.GridInplaceEditorLocator.prototype.fLocatorName;

/** Field
* @protected
* @type {HTMLInputElement}
*/
tp.GridInplaceEditorLocator.prototype.fTextBox;
/** Field
* @protected
* @type {HTMLElement}
*/
tp.GridInplaceEditorLocator.prototype.btnList;
/** Field
* @protected
* @type {HTMLElement}
*/
tp.GridInplaceEditorLocator.prototype.btnZoom;
//#endregion

//---------------------------------------------------------------------------------------
// nodes
//---------------------------------------------------------------------------------------

//#region tp.GridNodeType enum
/**
Internal. Indicates the type of a grid node
@class
@enum {number}
*/
tp.GridNodeType = {
    None: 0,
    Group: 1,
    Row: 2,
    Footer: 4
};
Object.freeze(tp.GridNodeType);
//#endregion

//#region tp.GridNode
/**
Internal. The grid node
*/
tp.GridNode = class {

    /**
     * Constructor
     * @param {tp.Grid} Grid The owner grid
     * @param {tp.GridNode} Parent The parent {@link tp.GridNode}
     * @param {tp.GridNodeType} Type The node type, one of the {@link tp.GridNodeType} constants
     * @param {tp.DataRow} Row The {@link tp.DataRow} row
     */
    constructor(Grid, Parent, Type, Row) {

        this.Grid = Grid;
        this.Parent = Parent;
        this.Type = Type;
        this.Row = Row;

        this.tpClass = 'GridNode';

        this.IsRoot = false;
        this.IsGroup = false;
        this.IsRow = false;
        this.IsFooter = false;

        this.IsExpanded = true;

        this.Level = Parent === null ? -1 : Parent.Level + 1;

        if (Type === tp.GridNodeType.Group)
            this.IsGroup = true;
        else if (Type === tp.GridNodeType.Row)
            this.IsRow = true;
        else if (Type === tp.GridNodeType.Footer)
            this.IsFooter = true;

        this.List = [];
        this.Aggregates = [];

        this.Footer = this.IsGroup ? new tp.GridNode(Grid, this, tp.GridNodeType.Footer, Row) : null;
    }

    /** Field
    * @type {string}
    */
    tpClass;

    /* fields read-only */

    /** Field
    * @readonly
    * @type {tp.Grid}
    */
    Grid;
    /** Field
    * @readonly
    * @type {tp.GridNode}
    */
    Parent;
    /** Field
    * @readonly
    * @type {tp.GridNodeType}
    */
    Type;
    /** Field
    * @readonly
    * @type {tp.DataRow}
    */
    Row;
    /** Field
    * @readonly
    * @type {tp.GridNode}
    */
    Footer;

    /**
    Used when this is a group node. The value upon which this group is build
    @type {any}
    */
    Key;                            

    /**
    The array of child {@link tp.GridNode} nodes
    @type {tp.GridNode[]}
    */
    List;      
    /** The array of aggregates 
     @type {any[]} 
     */
    Aggregates;
    /** The level in the nodes tree
     * @type {number}
     */
    Level;

    /** Flag
    * @type {boolean}
    */
    IsRoot;
    /** Flag
    * @type {boolean}
    */
    IsGroup;
    /** Flag
    * @type {boolean}
    */
    IsRow;
    /** Flag
    * @type {boolean}
    */
    IsFooter;

    /** Flag
     * @type {boolean}
     */
    IsExpanded;

    /* properties */
    /**
    Returns the first child node
    @type {tp.GridNode}
    */
    get First() {
        return this.List.length > 0 ? this.List[0] : null;
    }
    /**
    Returns the last child node
    @type {tp.GridNode}
    */
    get Last() {
        return this.List.length > 0 ? this.List[this.List.length - 1] : null;
    }
    /**
    Returns true if this is the first child node in its parent node
    @type {boolean}
    */
    get IsFirst() {
        return this.Parent ? this === this.Parent.First : false;
    }
    /**
    Returns true if this is the last child node in its parent node
    @type {boolean}
    */
    get IsLast() {
        return this.Parent ? this === this.Parent.Last : false;
    }
    /**
    Returns true if this is the last child group node in its parent node
    @type {boolean}
    */
    get IsLastGroup() {
        return (this.IsRoot && this.Grid.GroupColumnCount === 0)
            || (this.IsGroup && this.Grid.GroupColumnCount === this.Level - 1);
    }

    /* protecttd */
    /**
    Creates a new group node and adds the node to this node. Returns the newly created group node.
    @protected
    @param {any} Key The key of the node.
    @returns {tp.GridNode} Returns the newly created group node.
    */
    AddGroup(Key) {
        var Result = new tp.GridNode(this.Grid, this, tp.GridNodeType.Group, null);
        this.List.push(Result);
        Result.Key = Key;
        return Result;
    }
    /**
    Adds a list of data-rows to this node as row nodes.
    @protected
    @param {tp.DataRow[]} RowList The list of {@link tp.DataRow} rows to add
    */
    AddRowList(RowList) {
        var i, ln, Child;
        for (i = 0, ln = RowList.length; i < ln; i++) {
            Child = new tp.GridNode(this.Grid, this, tp.GridNodeType.Row, RowList[i]);
            this.List.push(Child);
        }
    }

    /* methods */
    /**
    Expands or collapses this node according to a specified flag. Valid only when this is a group node. Returns true only if operation was a valid one
    @protected
    @param {boolean} Flag Controls the expanding/collapsing
    @returns {boolean} Returns true only if operation was a valid one
    */
    Expand(Flag) {
        var Result = false;
        if (!this.IsRoot && this.IsGroup) {
            Flag = Flag === true;

            if (Flag !== this.IsExpanded) {
                this.IsExpanded = Flag;
                Result = true;
            }
        }

        if (Result) {
            this.Grid.ToggleNode(this);
        }

        return Result; // returns true only if operation was a valid one
    }
    /**
    Expands or collapses this node. Valid only when this is a group node.
    */
    Toggle() {
        this.Expand(!this.IsExpanded);
    }

    /**
    Returns the number of nodes in this node that should be displayed, i.e. are expanded if they are group nodes. <br />
    If this is a group node, then it returns the count regarding all levels. Header (title) and footer (summary) nodes are taken into account as well.
    @returns {number}  Returns the number of nodes in this node that should be displayed,
    */
    GetNodeListCount() {
        var Result = 0;

        if (this.IsGroup) {

            if (!this.IsRoot) {
                Result = 1;
            }

            if (this.IsExpanded) {

                var i, ln = this.List.length;
                if (ln > 0) {
                    if (this.List[0].IsGroup) {
                        for (i = 0; i < ln; i++) {
                            Result += this.List[i].GetNodeListCount();
                        }
                    } else {
                        Result += ln;
                    }
                }

                if (!this.IsRoot && this.Grid.GroupFooterVisible) {
                    Result++;
                }
            }

        }

        return Result;
    }
    /**
    Re-creates the list of nodes in all levels.
    */
    UpdateNodeList() {

        if (this.IsGroup) {

            if (!this.IsRoot) {
                this.Grid.NodeList.push(this);
            }

            if (this.IsExpanded) {

                var Child, i, ln = this.List.length;
                if (ln > 0) {
                    if (this.List[0].IsGroup) {
                        for (i = 0; i < ln; i++) {
                            Child = this.List[i];
                            Child.UpdateNodeList();
                        }
                    } else {
                        for (i = 0; i < ln; i++) {
                            Child = this.List[i];
                            this.Grid.NodeList.push(Child);
                        }
                    }
                }

                if (!this.IsRoot && this.Grid.GroupFooterVisible) {
                    this.Grid.NodeList.push(this.Footer);

                }
            }
        }

    }
    /**
     * Returns the aggregate value of a column by applying a specified aggregate function to all rows of this node.
     * @param {tp.GridColumn} Column The column to operate on.
     * @param {tp.AggregateType} [AggregateType] Optional. One of the {@link tp.AggregateType} constants.
     * @returns {any} Returns the aggregate value of a column by applying a specified aggregate function to all rows of this node.
     */
    GetAggregateValue(Column, AggregateType) {
        AggregateType = AggregateType || Column.fAggregate;
        let i, ln, v, Result;
        switch (AggregateType) {

            case tp.AggregateType.Count:
                if (this.IsRow)
                    return 1;

                if (this.IsRoot || this.IsGroup) {
                    Result = 0;
                    for (i = 0, ln = this.List.length; i < ln; i++)
                        Result += this.List[i].GetAggregateValue(Column, AggregateType);
                    return Result;
                }
                break;
            case tp.AggregateType.Sum:
                if (this.IsRow) {
                    return this.Row.Get(Column.DataColumn);
                }

                if (this.IsRoot || this.IsGroup) {
                    Result = 0;
                    for (i = 0, ln = this.List.length; i < ln; i++)
                        Result += this.List[i].GetAggregateValue(Column, AggregateType);
                    return Result;
                }
                break;
            case tp.AggregateType.Min:
                if (this.IsRow) {
                    return this.Row.Get(Column.DataColumn);
                }

                if (this.IsRoot || this.IsGroup) {
                    Result = null;
                    for (i = 0, ln = this.List.length; i < ln; i++) {
                        v = this.List[i].GetAggregateValue(Column, AggregateType);

                        if (!tp.IsEmpty(v)) {
                            if (tp.IsEmpty(Result)) {
                                Result = v;
                            } else {
                                if (v < Result) {
                                    Result = v;
                                }
                            }
                        }
                    }

                    return Result;
                }
                break;
            case tp.AggregateType.Max:
                if (this.IsRow) {
                    return this.Row.Get(Column.DataColumn);
                }

                if (this.IsRoot || this.IsGroup) {
                    Result = null;
                    for (i = 0, ln = this.List.length; i < ln; i++) {
                        v = this.List[i].GetAggregateValue(Column, AggregateType);

                        if (!tp.IsEmpty(v)) {
                            if (tp.IsEmpty(Result)) {
                                Result = v;
                            } else {
                                if (v > Result) {
                                    Result = v;
                                }
                            }
                        }
                    }

                    return Result;
                }
                break;

            case tp.AggregateType.Avg:
                Result = this.GetAggregateValue(Column, tp.AggregateType.Sum);

                if (this.IsRoot || this.IsGroup) {
                    v = this.GetAggregateValue(Column, tp.AggregateType.Count);
                    if (!tp.IsEmpty(v) && v > 0)
                        Result = Math.ceil(Result / v);
                }

                return Result;

        }

        return null;
    }
    /**
     * Returns the aggregate text of a {@link tp.GridColumn} column, that is the display text of an aggregation.
     * @param {tp.GridColumn} Column The column to operate on.
     * @returns {string} Returns the aggregate text, that is the display text of an aggregation.
     */
    GetAggregateText(Column) {
        var Result = '';

        if (Column.fAggregate !== tp.AggregateType.None) {
            var v = this.GetAggregateValue(Column);

            switch (Column.fAggregate) {
                case tp.AggregateType.Count:
                    Result = 'count=' + (v ? v.toString() : '0');
                    break;
                case tp.AggregateType.Sum:
                    Result = 'sum=' + Column.Format(v);
                    break;
                case tp.AggregateType.Min:
                    Result = 'min=' + Column.Format(v);
                    break;
                case tp.AggregateType.Max:
                    Result = 'max=' + Column.Format(v);
                    break;
                case tp.AggregateType.Avg:
                    Result = 'avg=' + Column.Format(v);
                    break;
            }
        }

        return Result;
    }

};
//#endregion

//#region tp.GridRootNode
/**
Internal. The grid root node
*/
tp.GridRootNode = class extends tp.GridNode {

    /**
    Constructor
    @param {tp.Grid} Grid The owner grid
    */
    constructor(Grid) {
        super(Grid, null, tp.GridNodeType.Group, null);
        this.tpClass = 'RootNode';
        this.IsRoot = true;
    }

    /* methods */
    /**
    Empties the list of child nodes and the aggregate list
    */
    Clear() {
        if (this.List)
            this.List.length = 0;

        if (this.Aggregates)
            this.Aggregates.length = 0;
    }


    /**
    Re-creates the list of nodes in all levels.
    */
    UpdateNodeList() {
        this.Grid.NodeList.length = 0;
        super.UpdateNodeList();
    }
    /**
    Builds the groups and the nodes of the grid.
    @param {tp.DataRow[]} RowList The list of {@link tp.DataRow} rows to use.
    */
    BuildGroups(RowList) {
        var self = this;
        var i, ln, Count, Index, Props = [];

        //---------------------------------
        var _InitializeNodeList = function () {


            var i, ln, Node = null;
            for (i = 0, ln = RowList.length; i < ln; i++) {
                Node = new tp.GridNode(self.Grid, self, tp.GridNodeType.Row, RowList[i]);
                self.Grid.NodeList.push(Node);
                self.List.push(Node);
            }
        };
        //---------------------------------
        var _GroupBy = function (ParentNode, DataList, PropIndex) {
            var Prop = Props[PropIndex];

            var i, ln, Data, Key, Node,
                Groups = {};

            for (i = 0, ln = DataList.length; i < ln; i++) {
                Data = DataList[i];
                Key = Data.Data[Prop];
                if (Key in Groups === false) {
                    Groups[Key] = {             // Key becomes a string here (a property name)
                        Key: Key,               // Key is a typed value here
                        List: []                // the list of rows under this Key
                    };
                }

                Groups[Key].List.push(Data);
            }

            var Keys = Object.keys(Groups);

            for (i = 0, ln = Keys.length; i < ln; i++) {
                Key = Keys[i];
                Node = ParentNode.AddGroup(Groups[Key].Key);

                if (PropIndex < Props.length - 1) {
                    //
                } else {
                    Node.AddRowList(Groups[Key].List);
                }

            }


            if (PropIndex < Props.length - 1) {
                for (i = 0, ln = ParentNode.List.length; i < ln; i++) {
                    Node = ParentNode.List[i];
                    _GroupBy(Node, Groups[Node.Key].List, PropIndex + 1);
                }
            }
        };
        //---------------------------------

        this.List.length = 0;
        this.Grid.NodeList.length = 0;

        if (this.Grid.GroupColumnCount === 0) {
            _InitializeNodeList();
        } else {

            // prepare the array with the group column indexes
            for (i = 0, ln = this.Grid.GroupColumnCount; i < ln; i++) {
                Props.push(this.Grid.GroupColumnByIndex(i).DataIndex);
            }

            // group
            _GroupBy(this, RowList, 0);

            // create the flat list                
            this.UpdateNodeList();
        }

    }
};
//#endregion

//---------------------------------------------------------------------------------------
// panels
//---------------------------------------------------------------------------------------

//#region tp.GridGroupPanel
/**
 * Internal. The group panel.
 * */
tp.GridGroupPanel = class extends tp.tpObject {

    /**
     * Constructor
     * @param {tp.Grid} Grid The owner grid
     * @param {HTMLElement} Parent The parent {@link HTMLElement} element
     */
    constructor(Grid, Parent) {
        super();

        this.Grid = Grid;

        this.Handle = Grid.Document.createElement('div');
        Parent.appendChild(this.Handle);
        this.Handle.className = tp.Classes.Groups;
        this.Handle.style.height = tp.px(this.Grid.GroupPanelHeight);
        this.Handle.style.minHeight = this.Handle.style.height;

        // as drop target
        tp.On(this.Handle, tp.Events.DragEnter, this.FuncBind(this.DragEnter));
        tp.On(this.Handle, tp.Events.DragOver, this.FuncBind(this.DragOver));
        tp.On(this.Handle, tp.Events.DragLeave, this.FuncBind(this.DragLeave));
        tp.On(this.Handle, tp.Events.DragDrop, this.FuncBind(this.DragDrop));
    }

    /** The owner grid
     * @type {tp.Grid}
     */
    Grid;
    /** The {@link HTMLElement} of this panel
     * @type {HTMLElement}
     */
    Handle;

    /**
    Hides or displays this panel
    @type {boolean}
    */
    get Visible() {
        return this.Handle.style.display === '';
    }
    set Visible(v) {
        this.Handle.style.display = v === true ? '' : 'none';
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridGroupPanel';
    }

    // as drop target

    /**
     * Event handler
     * @protected
     * @param {DragEvent} e The {@link DragEvent} event object
     */
    DragEnter(e) {
    }
    /**
     * Event handler
     * @protected
     * @param {DragEvent} e The {@link DragEvent} event object
     */
    DragOver(e) {
        let Column = this.Grid.DraggedColumn;
        if (Column) {
            if (e.preventDefault) {
                e.preventDefault();        // Necessary. Allows us to drop.
            }

            if (e.dataTransfer) {
                e.dataTransfer.dropEffect = 'move';
            }

            //return false;
            e.returnValue = false;
        }
    }
    /**
     * Event handler
     * @protected
     * @param {DragEvent} e The {@link DragEvent} event object
     */
    DragLeave(e) {
    }
    /**
     * Event handler
     * @protected
     * @param {DragEvent} e The {@link DragEvent} event object
     */
    DragDrop(e) {
        let Column = this.Grid.DraggedColumn;
        if (Column) {
            if (e.preventDefault) {
                e.preventDefault();        // Necessary. Allows us to drop.
            }

            var RefColumn = null;

            if (this.Grid.GroupColumnCount > 0) {
                var P = tp.Mouse.ToElement(e, this.Handle);
                var R;

                var C = null;
                for (var i = 0, ln = this.Grid.GroupColumnCount; i < ln; i++) {
                    C = this.Grid.GroupColumnByIndex(i);
                    R = tp.OffsetRect(C.Handle);
                    if (R.Contains(P)) {
                        RefColumn = C;
                        break;
                    }
                }
            }

            this.Grid.ColumnGrouped(Column, RefColumn);

            //return false;
            e.returnValue = false;
        }
    }

    /* public */

    /**
    Clears the values in fields/properties of this instance
    @override
    */
    Clear() {
        super.Clear();
        if (this.Handle) {
            this.Handle.innerHTML = '';
        }
    }
};
//#endregion

//#region  tp.GridColumnPanel

/** Internal. The column panel 
 */
tp.GridColumnPanel = class extends tp.tpObject {

    /**
     * Constructor
     * @param {tp.Grid} Grid The owner grid
     * @param {HTMLElement} Parent The parent {@link HTMLElement} element
     */
    constructor(Grid, Parent) {
        super();

        this.Grid = Grid;

        this.Handle = Grid.Document.createElement('div');
        Parent.appendChild(this.Handle);
        this.Handle.className = tp.Classes.Columns;

        this.Handle.style.height = tp.px(this.Grid.ColumnHeight);
        this.Handle.style.minHeight = this.Handle.style.height;

        this.Content = this.Handle.ownerDocument.createElement('div');
        this.Handle.appendChild(this.Content);
        this.Content.className = tp.Classes.Content;
    }


    /** The owner grid
     * @type {tp.Grid}
     */
    Grid;
    /** The {@link HTMLElement} of this panel
     * @type {HTMLElement}
     */
    Handle;
    /** The content {@link HTMLElement} elemnt
     @type {HTMLElement}
    */
    Content;


    /**
     Gets or sets the scroll left value.
     @type {number}
     */
    get ScrollLeft() {
        return this.Handle.scrollLeft;
    }
    set ScrollLeft(v) {
        this.Handle.scrollLeft = v;
    }
    /**
    Hides or displays this panel
    @type {boolean}
    */
    get Visible() {
        return this.Handle.style.display === '';
    }
    set Visible(v) {
        this.Handle.style.display = v === true ? '' : 'none';
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridColumnPanel';
    }

    /* public */
    /**
    Clears the values in fields/properties of this instance
    @override
    */
    Clear() {
        super.Clear();
        if (this.Content) {
            this.Content.innerHTML = '';
        }
    }
};
//#endregion

//#region  tp.GridFilterPanel

/** Internal. The filter panel
 */
tp.GridFilterPanel = class extends tp.tpObject {

    /**
     * Constructor
     * @param {tp.Grid} Grid The owner grid
     * @param {HTMLElement} Parent The parent {@link HTMLElement} element
     */
    constructor(Grid, Parent) {
        super();

        this.Grid = Grid;

        this.Handle = Grid.Document.createElement('div');
        Parent.appendChild(this.Handle);
        this.Handle.className = tp.Classes.Filters;

        this.Handle.style.height = tp.px(this.Grid.RowHeight);
        this.Handle.style.minHeight = this.Handle.style.height;

        this.Content = this.Handle.ownerDocument.createElement('div');
        this.Handle.appendChild(this.Content);
        this.Content.className = tp.Classes.Content;
    }


    /** The owner grid
     * @type {tp.Grid}
     */
    Grid;
    /** The {@link HTMLElement} of this panel
     * @type {HTMLElement}
     */
    Handle;
    /** The content {@link HTMLElement} elemnt
     @type {HTMLElement}
    */
    Content;

    /**
     Gets or sets the scroll left value.
     @type {number}
     */
    get ScrollLeft() {
        return this.Handle.scrollLeft;
    }
    set ScrollLeft(v) {
        this.Handle.scrollLeft = v;
    }
    /**
    Hides or displays this panel
    @type {boolean}
    */
    get Visible() {
        return this.Handle.style.display === '';
    }
    set Visible(v) {
        this.Handle.style.display = v === true ? '' : 'none';
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridFilterPanel';
    }

    /* public */

    /**
    Clears the values in fields/properties of this instance
    @override
    */
    Clear() {
        super.Clear();
        if (this.Content) {
            this.Content.innerHTML = '';
        }
    }
};
//#endregion

//#region  tp.GridViewportPanel 
/** Internal. The viewport panel
 */
tp.GridViewportPanel = class extends tp.tpObject {

    /**
     * Constructor
     * @param {tp.Grid} Grid The owner grid
     * @param {HTMLElement} Parent The parent {@link HTMLElement} element
     */
    constructor(Grid, Parent) {
        super();

        this.Grid = Grid;

        this.Handle = Grid.Document.createElement('div');
        Parent.appendChild(this.Handle);
        this.Handle.className = tp.Classes.Viewport;

        this.Content = this.Handle.ownerDocument.createElement('div');
        this.Handle.appendChild(this.Content);
        this.Content.className = tp.Classes.Content;

        this.fResizeDetector = new tp.ResizeDetector(this.Handle, this.OnElementSizeChanged, this, true);
    }
 
    /**
     Gets or sets the scroll top value.
     @type {number}
     */
    get ScrollTop() {
        return this.Handle.scrollTop;
    }
    set ScrollTop(v) {
        this.Handle.scrollTop = v;
    }
    /**
     Gets or sets the scroll left value.
     @type {number}
     */
    get ScrollLeft() {
        return this.Handle.scrollLeft;
    }
    set ScrollLeft(v) {
        this.Handle.scrollLeft = v;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridViewportPanel';
    }

    /**
    Notification sent by tp.ResizeDetector when the size of this element changes.
    This method is called only if this.IsElementResizeListener is true.
    @param {object} ResizeInfo An object of type <code>{Width: boolean, Height: boolean}</code>
    */
    OnElementSizeChanged(ResizeInfo) {
        this.Trigger('Resized', ResizeInfo);
    }

    /* public */
    /**
    Clears the values in fields/properties of this instance
    @override
    */
    Clear() {
        super.Clear();
        if (this.Content) {
            this.Content.innerHTML = '';
        }
    }

};
/** The owner grid
 * @type {tp.Grid}
 */
tp.GridViewportPanel.prototype.Grid = null;
/** The {@link HTMLElement} of this panel
 * @type {HTMLElement}
 */
tp.GridViewportPanel.prototype.Handle = null;
/** The content {@link HTMLElement} elemnt
 @type {HTMLElement}
*/
tp.GridViewportPanel.prototype.Content = null;
/** Detects size changes in an HTMLElement and sends notifications to a listener function.
 * @type {tp.ResizeDetector}
 * */
tp.GridViewportPanel.prototype.fResizeDetector = null;
//#endregion

//#region  tp.GridSummariesPanel
/** Internal. The summaries panel
 */
tp.GridSummariesPanel = class extends tp.tpObject {

    /**
     * Constructor
     * @param {tp.Grid} Grid The owner grid
     * @param {HTMLElement} Parent The parent {@link HTMLElement} element
     */
    constructor(Grid, Parent) {
        super();

        this.Grid = Grid;

        this.Handle = Grid.Document.createElement('div');
        Parent.appendChild(this.Handle);
        this.Handle.className = tp.Classes.Summaries;

        this.Handle.style.height = tp.px(this.Grid.RowHeight);
        this.Handle.style.minHeight = this.Handle.style.height;

        this.Content = this.Handle.ownerDocument.createElement('div');
        this.Handle.appendChild(this.Content);
        this.Content.className = tp.Classes.Content;
    }


    /** The owner grid
     * @type {tp.Grid}
     */
    Grid;
    /** The {@link HTMLElement} of this panel
     * @type {HTMLElement}
     */
    Handle;
    /** The content {@link HTMLElement} elemnt
     @type {HTMLElement}
    */
    Content;

    /**
    Hides or displays this panel
    @type {boolean}
    */
    get Visible() {
        return this.Handle.style.display === '';
    }
    set Visible(v) {
        this.Handle.style.display = v === true ? '' : 'none';
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridSummariesPanel';
    }

    /* public */
    /**
    Clears the values in fields/properties of this instance
    @override
    */
    Clear() {
        super.Clear();
        if (this.Content) {
            this.Content.innerHTML = '';
        }
    }
};
//#endregion

//#region  tp.GridBottomPanel

/** Internal. The bottom panel
 */
tp.GridBottomPanel = class extends tp.tpObject {
    /**
     * Constructor
     * @param {tp.Grid} Grid The owner grid
     * @param {HTMLElement} Parent The parent {@link HTMLElement} element
     */
    constructor(Grid, Parent) {
        super();

        this.Grid = Grid;

        this.Handle = Grid.Document.createElement('div');
        Parent.appendChild(this.Handle);
        this.Handle.className = tp.Classes.Bottom;

        this.Handle.style.height = tp.px(tp.Environment.ScrollbarSize.Height);
        this.Handle.style.minHeight = this.Handle.style.height;

        this.Content = this.Handle.ownerDocument.createElement('div');
        this.Handle.appendChild(this.Content);
        this.Content.className = tp.Classes.Content;
    }


    /** The owner grid
     * @type {tp.Grid}
     */
    Grid;
    /** The {@link HTMLElement} of this panel
     * @type {HTMLElement}
     */
    Handle;
    /** The content {@link HTMLElement} elemnt
     @type {HTMLElement}
    */
    Content;

    /**
     Gets or sets the scroll left value.
     @type {number}
     */
    get ScrollLeft() {
        return this.Handle.scrollLeft;
    }
    set ScrollLeft(v) {
        this.Handle.scrollLeft = v;
    }
    /**
    Hides or displays this panel
    @type {boolean}
    */
    get Visible() {
        return this.Handle.style.display === '';
    }
    set Visible(v) {
        this.Handle.style.display = v === true ? '' : 'none';
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.GridBottomPanel';
    }


    /* public */

    /**
    Clears the values in fields/properties of this instance
    @override
    */
    Clear() {
        super.Clear();
        if (this.Content) {
            this.Content.innerHTML = '';
        }
    }
};
//#endregion


//---------------------------------------------------------------------------------------
// grid
//---------------------------------------------------------------------------------------


//#region  tp.GridElementInfo

/** Internal. A structure used in recording information regarding a node and a list of cells or a column */
tp.GridElementInfo = class {
    /**
     * Constructor
     * @param {tp.GridNode} Node The {@link tp.GridNode} to record.
     * @param {HTMLElement[] | tp.GridColumn} CellsOrColumn An array of {@link HTMLElement} cells or a {@link tp.GridColumn}
     */
    constructor(Node, CellsOrColumn) {
        this.Cells = null;
        this.Column = null;

        if (arguments.length > 0) {
            this.Node = arguments[0];
        }

        if (arguments.length > 1) {
            if (tp.IsArray(arguments[1]))
                this.Cells = arguments[1];
            else if (arguments[1] instanceof tp.GridColumn)
                this.Column = arguments[1];
        }
    }

    /** Field 
     @type {tp.GridNode}
     */
    Node;
    /** Field
     @type {HTMLElement[]}
     */
    Cells;
    /** Field
     @type {tp.GridColumn}
     */
    Column;
};
//#endregion

//#region  tp.Grid

/**
A grid that supports groups, summaries, filtering, etc. <br />
@implements {tp.ILocatorLink}
@implements {tp.IDataSourceListener}
*/
tp.Grid = class extends tp.Control  {

    /**
    Constructor. <br />
    Example markup
    @example
    <pre>
        <div id="Grid"></div>
    </pre>

    Produced markup
    @example
    <pre>
        <div id="Grid" class="tp-Grid tp-Object" tabindex="-1">
            <div class="tp-Grid-ToolBar tp-Object">
                <div class="tp-FlexFill"></div>
            </div>
            <div class="tp-Groups" style="height: 41px; min-height: 41px;"></div>
            <div class="tp-Columns" style="height: 34px; min-height: 34px;">
                <div class="tp-Content"></div>
            </div>
            <div class="tp-Filters" style="height: 26px; min-height: 26px;">
                <div class="tp-Content"></div>
            </div>
            <div class="tp-Viewport">
                <div class="tp-Content"></div>
            </div>
            <div class="tp-Summaries" style="height: 26px; min-height: 26px;">
                <div class="tp-Content"></div>
            </div>
            <div class="tp-Bottom" style="height: 17px; min-height: 17px;">
                <div class="tp-Content"></div>
            </div>
        </div>

        // grid column

        <div class="tp-Grid-Col tp-Object">
            <div class="tp-Text" draggable="true" title="Name">Name</div>
            <div class="tp-Sorter">▴</div>
            <div class="tp-Resizer"></div>
        </div>

        // beneath the column lies the filter cell

        <div class="tp-Filter-Cell">
            <input type="text" />
        </div>

        // in the grid footer lies the summary cell

        <div class="tp-Summary-Cell">
            sum=123
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }


    /* properties */
    /**
    Returns true if the grid is grouped (has grouped columns)
    @type {boolean}
    */
    get IsGrouped() {
        return this.GroupColumns.length > 0;
    }


    /**
    Returns true when the columns are auto-generated
    @type {boolean}
    */
    get HasAutoGeneratedColumns() {
        return this.fHasAutoGeneratedColumns;
    }

    /**
    Shows or hides the tool-bar. Defaults to false.
    @type {boolean}
    */
    get ToolBarVisible() {
        return this.pnlToolBar ? this.pnlToolBar.Visible : false;
    }
    set ToolBarVisible(v) {
        if (this.pnlToolBar) {
            this.pnlToolBar.Visible = v;
        }
    }
    /**
    Shows or hides the groups panel. Defaults to true.
    @type {boolean}
    */
    get GroupsVisible() {
        return this.pnlGroups ? this.pnlGroups.Visible : false;
    }
    set GroupsVisible(v) {
        if (this.pnlGroups) {
            this.pnlGroups.Visible = v;
        }
    }
    /**
    Shows or hides the columns panel. Defaults to true.
    @type {boolean}
    */
    get ColumnsVisible() {
        return this.pnlColumns ? this.pnlColumns.Visible : false;
    }
    set ColumnsVisible(v) {
        if (this.pnlColumns) {
            this.pnlColumns.Visible = v;
        }
    }
    /**
    Shows or hides the filter panel. Defaults to true.
    @type {boolean}
    */
    get FilterVisible() {
        return this.pnlFilter ? this.pnlFilter.Visible : false;
    }
    set FilterVisible(v) {
        if (this.pnlFilter) {
            this.pnlFilter.Visible = v;
        }
    }
    /**
    Shows or hides the footer panel. Defaults to true.
    @type {boolean}
    */
    get FooterVisible() {
        return this.pnlFooter ? this.pnlFooter.Visible : false;
    }
    set FooterVisible(v) {
        if (this.pnlFooter) {
            this.pnlFooter.Visible = v;
        }
    }


    /**
    Gets or sets the focused data-row 
    @type {tp.DataRow}
    */
    get FocusedRow() {
        return this.fFocusedRow;
    }
    set FocusedRow(v) {
        this.SetFocusedRow(v);
    }

    /**
    Gets or sets a boolean value indicating whether data is in a normal state and the BuildGroups() method can be called.
    When true, prohibits BuildGroups().
    @type {boolean}
    */
    get AlteringData() {
        return this.fAlteringDataCounter > 0;
    }
    set AlteringData(v) {
        this.fAlteringDataCounter = v === true ? this.fAlteringDataCounter + 1 : this.fAlteringDataCounter - 1;
        if (this.fAlteringDataCounter < 0)
            this.fAlteringDataCounter = 0;
    }

    /**
    Returns the number of columns
    @type {number}
    */
    get ColumnCount() {
        return this.Columns.length;
    }
    /**
    Returns the number of group columns
    @type {number}
    */
    get GroupColumnCount() {
        return this.GroupColumns.length;
    }
    /**
    Returns the number of value columns
    @type {number}
    */
    get ValueColumnCount() {
        return this.ValueColumns.length;
    }
    /**
    Returns the number of aggregate columns
    @type {number}
    */
    get AggregateColumnCount() {
        return this.AggregateColumns.length;
    }

    /**
    Shows or hides a tool-bar standard button
    @type {boolean}
    */
    get ButtonInsertVisible() {
        return this.ToolBarVisible && !tp.IsEmpty(this.fButtonInsert) && this.fButtonInsert.Visible;
    }
    set ButtonInsertVisible(v) {
        v = v === true;
        if (!tp.IsEmpty(this.fButtonInsert))
            this.fButtonInsert.Visible = v;
    }
    /**
    Shows or hides a tool-bar standard button
    @type {boolean}
    */
    get ButtonDeleteVisible() {
        return this.ToolBarVisible && !tp.IsEmpty(this.fButtonDelete) && this.fButtonDelete.Visible;
    }
    set ButtonDeleteVisible(v) {
        v = v === true;
        if (!tp.IsEmpty(this.fButtonDelete))
            this.fButtonDelete.Visible = v;
    }
    /**
    Shows or hides a tool-bar standard button
    @type {boolean}
    */
    get ButtonEditVisible() {
        return this.ToolBarVisible && !tp.IsEmpty(this.fButtonEdit) && this.fButtonEdit.Visible;
    }
    set ButtonEditVisible(v) {
        v = v === true;
        if (!tp.IsEmpty(this.fButtonEdit))
            this.fButtonEdit.Visible = v;
    }
    /**
    Shows or hides a tool-bar standard button
    @type {boolean}
    */
    get ButtonFindVisible() {
        return this.ToolBarVisible && !tp.IsEmpty(this.fButtonFind) && this.fButtonFind.Visible;
    }
    set ButtonFindVisible(v) {
        if (!tp.IsEmpty(this.fButtonFind))
            this.fButtonFind.Visible = v;
    }

    /**
    Returns the number of locators of this grid
    @type {number}
    */
    get LocatorCount() {
        return this.Locators.length;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.Grid';
        this.fDefaultCssClasses = [tp.Classes.Grid];
        this.fDataBindMode = tp.ControlBindMode.Grid;
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        this.Handle.tabIndex = -1;

        this.RowHeight = 0;
        this.VirtualHeight = 0;
        this.ViewportHeight = 0;
        this.GroupPanelHeight = 0;
        this.ColumnHeight = 0;

        this.RowWidth = 0;
        this.ContentWidth = 0;
        this.ColumnContentWidth = 0;
        this.GroupCellWidth = 0;

        //this.Handle.style.minHeight = '200px';

        this.UpdateRowHeight();

        this.pnlToolBar = new tp.ControlToolBar(null, { Parent: this.Handle });           // tool-bar
        this.pnlToolBar.On('ButtonClick', this.OnToolBarButtonClick, this);

        this.pnlGroups = new tp.GridGroupPanel(this, this.Handle);         // groups panel
        this.pnlColumns = new tp.GridColumnPanel(this, this.Handle);       // columns panel (columns viewport)   
        this.pnlFilter = new tp.GridFilterPanel(this, this.Handle);        // filter row panel   
        this.pnlViewport = new tp.GridViewportPanel(this, this.Handle);    // viewport
        this.pnlFooter = new tp.GridSummariesPanel(this, this.Handle);     // footer panel
        this.pnlBottom = new tp.GridBottomPanel(this, this.Handle);        // bottom panel (used just for horizontal scrolling only) 

        this.fButtonInsert = this.AddToolBarButton('GridRowInsert', tp.IcoChars.Insert, 'Insert', '', '', false);  // tp.Classes.IcoToolBarInsert
        this.fButtonDelete = this.AddToolBarButton('GridRowDelete', tp.IcoChars.Delete, 'Delete', '', '', false);  // tp.Classes.IcoToolBarDelete
        this.fButtonEdit = this.AddToolBarButton('GridRowEdit', tp.IcoChars.Edit, 'Edit', '', '', false);        // tp.Classes.IcoToolBarEdit
        this.fButtonFind = this.AddToolBarButton('GridRowFind', tp.IcoChars.Find, 'Find', '', '', false);        // tp.Classes.IcoToolBarFind

        this.fButtonInsert.Visible = false;
        this.fButtonDelete.Visible = false;
        this.fButtonEdit.Visible = false;
        this.fButtonFind.Visible = false;

        // aggregates context menu
        this.mnuAggregates = new tp.ContextMenu(null, null);
        this.mnuAggregates.AddMenuItem('Sum', 'Sum').Tag = tp.AggregateType.Sum;
        this.mnuAggregates.AddMenuItem('Count', 'Count').Tag = tp.AggregateType.Count;
        this.mnuAggregates.AddMenuItem('Min', 'Min').Tag = tp.AggregateType.Min;
        this.mnuAggregates.AddMenuItem('Max', 'Max').Tag = tp.AggregateType.Max;
        this.mnuAggregates.AddMenuItem('Avg', 'Avg').Tag = tp.AggregateType.Avg;
        this.mnuAggregates.AddMenuItem('None', 'None').Tag = tp.AggregateType.None;

        // events 
        this.HookEvent(tp.Events.MouseDown);
        this.HookEvent(tp.Events.MouseUp);
        this.HookEvent(tp.Events.MouseMove);

        this.HookEvent(tp.Events.KeyDown);
        this.HookEvent(tp.Events.KeyUp);
        this.HookEvent(tp.Events.KeyPress);

        this.HookEvent(tp.Events.Click);
        this.HookEvent(tp.Events.DoubleClick);

        tp.On(window, tp.Events.MouseDown, this, false);
        tp.On(window, tp.Events.Scroll, this, false);

        this.mnuAggregates.On('ItemClick', this.mnuAggregates_ItemClick, this);

        this.pnlViewport.On('Resized', this.Viewport_OnResized, this);
        tp.On(this.pnlViewport.Handle, tp.Events.Scroll, this.FuncBind(this.Viewport_OnScroll));

        tp.On(this.pnlBottom.Handle, tp.Events.Scroll, this.FuncBind(this.Bottom_OnScroll));

        this.Root = new tp.GridRootNode(this);

    }
    /**
    Initializes fields and properties just before applying the create params.  
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        this.LastScrollTop = 0;
        this.Cache = {};

        this.NodeList = [];           // a flat list of nodes. Each row in the grid is a "node" (group, group footer, or row node)

        this.Columns = [];
        this.HiddenColumns = [];
        this.ValueColumns = [];
        this.GroupColumns = [];
        this.AggregateColumns = [];

        this.Locators = [];

        this.IsBinding = false;
        this.fSettingPosition = false;
        this.fAlteringDataCounter = 0;

        this.VisibleRowIndexFirst = 0;
        this.VisibleRowIndexLast = 0;

        this.fShowIdColumnsFlag = true;

        this.ConfirmDelete = true;
        this.AllowUserToAddRows = true;
        this.AllowUserToDeleteRows = true;
        this.AllowUserToOrderColumns = true;
        this.AllowUserToResizeColumns = true;

        this.AutoGenerateColumns = true;
        this.fHasAutoGeneratedColumns = false;

        this.GroupFooterVisible = true;

        this.fResizing = false;

        this.fReadOnly = true;
    }
    /**
    Processes the this.CreateParams by applying its properties to the properties of this instance
    @protected
    @override
    @param {object} [o] Optional. The create params object to processs.
    */
    ProcessCreateParams(o) {
        o = o || {};

        let CP,
            Name,
            Columns,    // []
            A;          // []
        let Column;     // tp.GridColumn

        // locators
        if ('Locators' in o && tp.IsArray(o.Locators)) {
            let Locators = o.Locators;
            let DataField;
            let NameOrDescriptor; // string | tp.LocatorDescriptor;
            for (let i = 0, ln = Locators.length; i < ln; i++) {
                DataField = Locators[i].DataField;
                NameOrDescriptor = Locators[i].Locator;
                this.AddLocatorAsync(DataField, NameOrDescriptor);
            }
        }

        // columns
        if ('Columns' in o && tp.IsArray(o['Columns'])) {
            Columns = o['Columns'];
            for (let i = 0, ln = Columns.length; i < ln; i++) {
                CP = Columns[i];
                Name = CP['Name'];
                Column = this.AddColumn(CP['Name']);
                Column.ProcessCreateParams(CP);
            }
        }

        if (Columns) {
            this.AutoGenerateColumns = false;
        }



        for (var Prop in o) {
            if (!tp.IsFunction(o[Prop])) {
                if ('Columns' === Prop || 'AutoGenerateColumns' === Prop || 'Locators' === Prop) {
                    // do nothing
                } else if (tp.IsSameText('CustomButtons', Prop) && tp.IsArray(o[Prop])) {
                    A = o[Prop];
                    for (let i = 0, ln = A.length; i < ln; i++) {
                        CP = A[i];
                        this.AddToolBarButton(CP['Command'] || '', CP['Text'] || '', CP['ToolTip'] || '', CP['IcoClasses'] || '', CP['CssClasses'] || '', CP['ToRight'] === true);
                    }
                } else {
                    this[Prop] = o[Prop];
                }
            }
        }
    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();
        this.IsElementResizeListener = true;
        this.IsScreenResizeListener = true;
    }
    /**
    Event trigger. Called right after the DataSource is set to a new value and the control should clear its display.
    @protected
    @override
    */
    OnClearDataDisplay() {
        this.Clear();
        this.ClearCache();
        this.pnlViewport.Handle.scrollTop = 0;
        super.OnClearDataDisplay();
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        this.IsBinding = true;
        try {
            super.Bind();

            this.VisibleRowIndexFirst = 0;
            this.VisibleRowIndexLast = 0;

            var i, ln,
                Column,     // tp.GridColumn,
                DataColumn, // tp.DataColumn,
                Table = this.DataSource.Table;


            if (this.AutoGenerateColumns === true) {
                // auto-generate columns
                this.ClearColumns();
                for (i = 0, ln = Table.Columns.length; i < ln; i++) {
                    DataColumn = Table.Columns[i];
                    Column = this.AddColumn(DataColumn.Name);
                }

                this.fHasAutoGeneratedColumns = true;
            } else {
                // connect columns to data-columns
                for (i = 0, ln = this.Columns.length; i < ln; i++) {
                    Column = this.Columns[i];
                    Column.Bind();
                }
            }


            // value and group columns
            this.ValueColumns = [];
            this.GroupColumns = [];

            for (i = 0, ln = this.Columns.length; i < ln; i++) {
                Column = this.Columns[i];
                if (Column.Visible === true) {
                    this.ValueColumns.push(this.Columns[i]);
                }
            }

            // attach columns to dom  (1. remove all from dom 2. render all again)
            this.UpdateValueColumns();

            // create nodes for the first time
            this.BuildGroups();

            this.pnlViewport.ScrollTop = 0;


        } finally {
            this.IsBinding = false;
        }
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {

        let self = this;
        let elTarget = null; // HTMLElement
        let elParent = null; // HTMLElement
        if (tp.IsHTMLElement(e.target)) {
            elTarget = e.target;
            if (tp.IsHTMLElement(elTarget.parentNode))
                elParent = elTarget.parentNode;
        }


        let Node;
        let Info;  //tp.GridElementInfo;
        let T = tp.Events.ToTripous(e.type);

        switch (T) {
            case tp.Events.Scroll:
                if (e.eventPhase === Event.BUBBLING_PHASE) {
                    if (this.Editor && !this.ContainsHandle(elTarget)) {
                        this.HideEditor(true);
                    }
                }
                break;

            case tp.Events.MouseDown:
                if (e.currentTarget === window) {
                    if (this.Editor && !this.ContainsHandle(elTarget)) {
                        this.HideEditor(true);
                    }
                } else
                    // expand-collapse group
                    if (tp.Mouse.IsLeft(e) && tp.HasClass(elTarget, tp.Classes.Expander)) {
                        tp.CancelEvent(e);
                        Info = this.GetElementInfo(elParent);
                        Node = Info.Node; //elTarget.parentNode.__tpInfo.Node;
                        if (Node && Node.IsGroup) {
                            Node.Toggle();
                        }

                        // focused data row
                    } else if (tp.HasClass(elTarget, tp.Classes.GridCell)) {
                        Info = this.GetElementInfo(elTarget);
                        Node = Info.Node; // elTarget.__tpInfo.Node;

                        if (this.FocusedRow !== Node.Row)
                            this.SetFocusedRow(Node.Row);

                        if (this.ReadOnly === false) {
                            this.ShowEditor(elTarget);
                        }
                    }
                    else if (this.Editor && this.Editor.Control && !this.Editor.ContainsHandle(elTarget)) {
                        this.HideEditor(true);
                    }

                break; 

            case tp.Events.MouseUp:
                if (elTarget && this.FocusedRow) {
                    if (this.ReadOnly === false) {
                        //this.ShowEditor(elTarget);
                    }
                }
                break;

            case tp.Events.KeyDown:
                if (e instanceof KeyboardEvent) {
                    switch (e.keyCode) {

                        case tp.Keys.Up:
                        case tp.Keys.Down:
                            tp.CancelEvent(e);
                            this.RowScroll(e.keyCode === tp.Keys.Up);
                            break;

                        case tp.Keys.Left:
                        case tp.Keys.Right:
                            tp.CancelEvent(e);
                            this.CellScroll(e.keyCode === tp.Keys.Left);
                            break;

                        case tp.Keys.PageUp:
                        case tp.Keys.PageDown:
                            tp.CancelEvent(e);
                            this.PageScroll(e.keyCode === tp.Keys.PageUp);
                            break;

                        case tp.Keys.Home:
                        case tp.Keys.End:
                            tp.CancelEvent(e);
                            this.GridScroll(e.keyCode === tp.Keys.Home);
                            break;

                        case tp.Keys.Enter:
                            if (tp.HasClass(elTarget, tp.Classes.GridCell)) {
                                this.ShowEditor(elTarget);
                                tp.CancelEvent(e);
                            }

                            break;

                        case tp.Keys.F2:
                            if (tp.HasClass(elTarget, tp.Classes.GridCell)) {
                                tp.CancelEvent(e);
                                this.ShowEditor(elTarget);
                            }

                            break;
                    }
                }

                break;

            case tp.Events.KeyUp:
                if (e instanceof KeyboardEvent) {
                    switch (e.keyCode) {
                        case tp.Keys.Tab:
                            this.HideEditor(true);
                            break;
                        case tp.Keys.Escape:
                            if (this.Editor) {
                                this.HideEditor(false);
                            }
                            break;
                    }
                }

                break;
        }



        super.OnAnyDOMEvent(e);
    }

 
    /* IDataSourceListener implementation */
    /**
    Notification
    @override
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    */
    DataSourceRowCreated(Table, Row) {
        super.DataSourceRowCreated(Table, Row);
    }
    /**
    Notification
    @override
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    */
    DataSourceRowAdded(Table, Row) {
        super.DataSourceRowAdded(Table, Row);

        if (!this.AlteringData) {
            this.BuildGroups();
        }
    }
    /**
    Notification
    @override
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    @param {tp.DataColumn} Column The {@link tp.DataColumn} column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    DataSourceRowModified(Table, Row, Column, OldValue, NewValue) {
        super.DataSourceRowModified(Table, Row, Column, OldValue, NewValue);

        if (!this.AlteringData) {
            var elCell, GridColumn = this.ColumnByName(Column.Name);
            if (!tp.IsEmpty(GridColumn)) {
                elCell = this.CellElementOf(Row, GridColumn);
                if (tp.IsHTMLElement(elCell)) {
                    GridColumn.Render(elCell, Row);
                }

                if (GridColumn.IsGroupColumn) {
                    this.BuildGroups();
                } else if (GridColumn.IsAggregateColumn) {
                    this.Render();
                }
            }
        }
    }
    /**
    Notification
    @override
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    */
    DataSourceRowRemoved(Table, Row) {
        super.DataSourceRowRemoved(Table, Row);

        if (!this.AlteringData) {
            this.BuildGroups();
        }
    }
    /**
    Notification
    @override
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    @param {number} Position The new position
    */
    DataSourcePositionChanged(Table, Row, Position) {
        if (!this.fSettingPosition) {
            super.DataSourcePositionChanged(Table, Row, Position);
            this.FocusedRow = Row;
        }
    }

    /**
    Notification
    @override
    */
    DataSourceSorted() {
        this.BuildGroups();
    }
    /**
    Notification
    @override
    */
    DataSourceFiltered() {
        this.BuildGroups();
    }
    /**
    Notification
    @override
    */
    DataSourceUpdated() {
        this.BuildGroups();
    }

    /* build groups and render nodes (internal methods) */
    /** Build groups
     * @private
     * */
    BuildGroups() {
        if (tp.IsEmpty(this.DataSource))
            return;

        if (this.IsGrouped) {
            tp.AddClass(this.Handle, tp.Classes.Grouped);
        } else {
            tp.RemoveClass(this.Handle, tp.Classes.Grouped);
        }

        for (var i = 0, ln = this.Columns.length; i < ln; i++) {
            if (this.Columns[i].IsGroupColumn) {
                tp.AddClass(this.Columns[i].Handle, tp.Classes.Grouped);
            } else {
                tp.RemoveClass(this.Columns[i].Handle, tp.Classes.Grouped);
            }
        }

        this.Root.BuildGroups(this.DataSource.Rows);

        this.UpdateRowWidth();          // row width               
        this.UpdateScrollHeight();      // calc scrollable heights and start rendering      

        this.ClearCache();
        this.RenderNodes();
        this.RenderFooterAggregates();
    }
    /** Render nodes
     * @private
     * */
    RenderNodes() {
        if (this.NodeList.length > 0) {

            this.LastScrollTop = this.pnlViewport.ScrollTop;

            var Plus = 3;

            var Y = this.LastScrollTop;         // calculate the viewport elevator  

            var Delta = this.RowHeight * Plus;
            var RowIndexTop = Math.floor((Y - Delta) / this.RowHeight);
            var RowIndexBottom = Math.ceil((Y + this.ViewportHeight + Delta) / this.RowHeight);

            RowIndexTop = Math.max(0, RowIndexTop);
            RowIndexBottom = Math.min(this.VirtualHeight / this.RowHeight, RowIndexBottom);

            // real visible row indexes
            this.VisibleRowIndexFirst = RowIndexTop + Plus;
            this.VisibleRowIndexLast = RowIndexBottom - Plus;

            // remove rows no longer in the viewport
            var Node,   // tp.GridNode
                el,     // HTMLElement
                Index;

            for (Index in this.Cache) {
                if (Index < RowIndexTop || Index > RowIndexBottom) {
                    el = this.Cache[Index];
                    el.parentNode.removeChild(el);
                    delete this.Cache[Index];
                }
            }

            // add new rows
            var doc = this.Document;
            var Length = this.NodeList.length;
            for (Index = RowIndexTop; Index <= RowIndexBottom; Index++) {
                if ((Index >= 0) && (Index <= Length - 1) && !this.Cache[Index]) {

                    el = doc.createElement('div');
                    el.className = tp.Classes.Node;

                    el.tabIndex = 1; // -1; 
                    el.style.top = tp.px(Index * this.RowHeight); // + 'px';
                    el.style.width = tp.px(this.RowWidth); // + 'px'; 
                    el.style.height = tp.px(this.RowHeight); // + 'px';

                    Node = this.NodeList[Index];
                    this.SetElementInfo(el, Node, [], null);

                    this.RenderNode(el, Node, Index);
                    this.pnlViewport.Content.appendChild(el);
                    this.Cache[Index] = el;
                }
            }

        }
    }
    /**
     * Render node
     * @private
     * @param {HTMLElement} elNode The {@link HTMLElement} node
     * @param {tp.GridNode} Node The {@link tp.GridNode} node
     * @param {number} RowIndex The row index
     */
    RenderNode(elNode, Node, RowIndex) {

        let i, ln,
            Level,          // number
            Column,         // tp.GridColumn
            GroupColumn,    // tp.GridColumn
            el,             // HTMLElement
            NodeInfo = this.GetElementInfo(elNode), // tp.GridElementInfo
            doc = this.Document;

        let Row = Node.Row;


        // Group Header ==========================================================================
        if (Node.IsGroup) {

            tp.AddClass(elNode, tp.Classes.Group);

            ln = Node.Level + 1;

            // group cells
            for (i = 0; i < ln; i++) {
                el = doc.createElement('div');
                el.tabIndex = -2;

                el.style.width = this.GroupCellWidth + 'px';

                if (i === ln - 1) {
                    el.innerHTML = Node.IsExpanded ? tp.Grid.CollapseSymbol : tp.Grid.ExpandSymbol;  
                    el.className = tp.ConcatClasses(tp.Classes.GroupCell, tp.Classes.UnSelectable, tp.Classes.Expander);
                } else {
                    el.className = tp.ConcatClasses(tp.Classes.GroupCell, tp.Classes.UnSelectable);
                }

                elNode.appendChild(el);
            }

            // group text
            el = doc.createElement('div');

            NodeInfo.Cells.push(el);

            el.className = tp.Classes.CellText;  
            el.tabIndex = -3;

            GroupColumn = this.GroupColumns[Node.Level];
            el.textContent = tp.Format('{0}: {1}', GroupColumn.Text, GroupColumn.Format(Node.Key));
            elNode.appendChild(el);
           
        }
         // Group Footer ==========================================================================
        else if (Node.IsFooter) {
            tp.AddClass(elNode, tp.Classes.Summary);

            ln = this.GroupColumns.length;
            Level = Node.Parent.Level;

            // group cells      
            for (i = 0; i < ln; i++) {
                el = doc.createElement('div');
                el.tabIndex = -2;

                el.style.width = this.GroupCellWidth + 'px';

                el.className = i >= Level ? tp.ConcatClasses(tp.Classes.GroupCell, tp.Classes.CellTube) : tp.Classes.GroupCell;
                elNode.appendChild(el);
            }

            // summary cells 
            for (i = 0, ln = this.ValueColumns.length; i < ln; i++) {

                Column = this.ValueColumns[i];
                if (Column.Visible) {
                    el = doc.createElement('div');
                    NodeInfo.Cells.push(el);

                    el.className = tp.ConcatClasses(tp.Classes.SummaryCell, tp.Classes.CellText);  
                    el.tabIndex = -3;

                    el.style.justifyContent = tp.Alignment.ToFlex(Column.Alignment);
                    el.style.width = Column.Width + 'px';

                    el.textContent = Node.Parent.GetAggregateText(Column);

                    this.SetElementInfo(el, Node.Parent, null, Column);

                    tp.On(el, tp.Events.ContextMenu, this.FuncBind(this.AnyContextMenu), false);

                    elNode.appendChild(el);
                }

            }
 
        }
         // Data Row ==========================================================================
        else if (Node.IsRow) {

            tp.AddClass(elNode, tp.Classes.GridRow);

            if (Node.Row === this.fFocusedRow) {
                tp.AddClass(elNode, tp.Classes.Focused);
            }

            // group cells
            if (Node.Parent && (Node.Parent.IsRoot === false)) {

                ln = Node.Parent.Level + 1;
                for (i = 0; i < ln; i++) {
                    el = doc.createElement('div');
                    el.tabIndex = -2;

                    el.style.width = this.GroupCellWidth + 'px';
                    el.className = i === ln - 1 ? tp.ConcatClasses(tp.Classes.GroupCell, tp.Classes.Last) : tp.Classes.GroupCell;

                    elNode.appendChild(el);
                }
            }

            // cells   
            for (i = 0, ln = this.ValueColumns.length; i < ln; i++) {

                Column = this.ValueColumns[i];
                if (Column.Visible) {
                    el = doc.createElement('div');
                    NodeInfo.Cells.push(el);
                    el.className = tp.ConcatClasses(tp.Classes.GridCell, tp.Classes.CellText); // tp.Classes.GridCell;
                    el.tabIndex = 0;

                    el.style.justifyContent = tp.Alignment.ToFlex(Column.Alignment);
                    el.style.width = Column.Width + 'px';

                    this.SetElementInfo(el, Node, null, Column);

                    Column.Render(el, Row);

                    elNode.appendChild(el);
                }

            }
        }


    }
    /** Render footer aggregates
     * @private
     */
    RenderFooterAggregates() {
        let S;
        for (var i = 0, ln = this.ValueColumns.length; i < ln; i++) {
            S = this.Root.GetAggregateText(this.ValueColumns[i]);
            this.ValueColumns[i].RenderFooterSummary(S);
        }
    }
    /** Render
     * @private
     * */
    Render() {
        this.ClearCache();
        this.RenderNodes();
        this.RenderFooterAggregates();
    }

    /** Forces a repaint of groups, data-rows and aggregates */
    RepaintRows() {
        setTimeout(() => {
            this.BuildGroups();
        }, 100);
    }

    /* cache (internal methods) */
    /** Clear cache
     * @protected
     * */
    ClearCache() {
        var el,     // HTMLElement
            Index;

        for (Index in this.Cache) {
            el = this.Cache[Index];
            el.parentNode.removeChild(el);
            delete this.Cache[Index];
        }

        this.Cache = {};

        this.pnlViewport.Clear();
    }
    /**
     * Returns the index of an {@link HTMLElement} node in the cache
     * @protected
     * @param {HTMLElement} elNode The {@link HTMLElement} node
     * @returns {number}  Returns the index of an {@link HTMLElement} node in the cache
     */
    CacheIndexOf(elNode) {
        for (var Index in this.Cache) {
            if (this.Cache[Index] === elNode) {
                return parseInt(Index, 10);
            }
        }
        return null;
    }
    /**
     * Returns an {@link HTMLElement} node by its index in the cache
     * @protected
     * @param {number} NodeIndex The node index
     * @returns {HTMLElement} Returns an {@link HTMLElement} node by its index in the cache
     */
    ByCacheIndex(NodeIndex){
        for (var Index in this.Cache) {
            if (NodeIndex === parseInt(Index, 10)) {
                return this.Cache[Index];
            }
        }

        return null;
    }

    /* keyboard scrolling (internal methods) */
    /** Returns the parent {@link HTMLElement} node of a specified {@link HTMLElement}
     * @protected
     * @param {HTMLElement} el The {@link HTMLElement}
     * @returns {HTMLElement} Returns the parent {@link HTMLElement} node of a specified {@link HTMLElement}
     */
    ParentNodeElementOf(el) {
        if (el) {
            if (tp.HasClass(el, tp.Classes.Node)) {
                return el;
            }

            var Node = el.parentNode;
            while (!tp.IsEmpty(Node)) {
                if (tp.HasClass(Node, tp.Classes.Node)) {
                    return Node;
                }
                Node = Node.parentNode;
            }
        }

        return null;
    }
    /** Returns the first sibling {@link HTMLElement} row of another row specified by an index in the cache. A specified flag controls the direction, up or down.
     * @protected
     * @param {number} CacheIndex The index of row
     * @param {boolean} Up Controls the direction
     * @returns {HTMLElement} Returns the first sibling {@link HTMLElement} row of another row specified by an index in the cache.
     */
    FindFirstSiblingRow(CacheIndex, Up) {
        if (!tp.IsEmpty(CacheIndex)) {
            Up = Up === true;
            CacheIndex = Up ? CacheIndex - 1 : CacheIndex + 1;
            var elNode = this.ByCacheIndex(CacheIndex);
            if (elNode) {
                var Node = this.GetElementInfo(elNode).Node; // elNode.__tpInfo.Node;
                if (!Node.IsRow) {
                    this.pnlViewport.ScrollTop = Up ? this.pnlViewport.ScrollTop + this.RowHeight : this.pnlViewport.ScrollTop - this.RowHeight;
                    return this.FindFirstSiblingRow(CacheIndex, Up);
                } else {
                    return elNode;
                }
            }
        }

        return null;
    }
    /** Scrolls a row
     * @protected
     * @param {boolean} Up Controls the direction
     */
    RowScroll(Up) {
        Up = Up === true;

        var elNode, CellIndex, el = tp.ActiveElement;
        if (tp.IsHTMLElement(el)) {
            if (tp.ContainsElement(this.pnlViewport.Content, el)) {
                elNode = this.ParentNodeElementOf(el);
                if (tp.IsHTMLElement(elNode)) {
                    CellIndex = this.GetElementInfo(elNode).Cells.indexOf(el);
                    var CacheIndex = this.CacheIndexOf(elNode);
                    elNode = this.FindFirstSiblingRow(CacheIndex, Up);
                    if (tp.IsHTMLElement(elNode)) {
                        CellIndex = CellIndex === -1 ? 0 : CellIndex;
                        let el2 = this.GetElementInfo(elNode).Cells[CellIndex];
                        if (tp.IsHTMLElement(el2)) {
                            this.pnlViewport.ScrollTop = Up ? this.pnlViewport.ScrollTop - this.RowHeight : this.pnlViewport.ScrollTop + this.RowHeight;
                            //el2.focus();
                        }
                    }
                }
            }
        }

    }
    /** Scrolls a cell
     * @protected
     * @param {boolean} Left Controls the direction
     */
    CellScroll(Left) {
        Left = Left === true;

        var elNode, el = tp.ActiveElement;
        if (tp.IsHTMLElement(el)) {
            if (tp.ContainsElement(this.pnlViewport.Content, el)) {
                elNode = this.ParentNodeElementOf(el);
                if (tp.IsHTMLElement(elNode) && tp.HasClass(elNode, tp.Classes.GridRow)) {
                    let el2 = Left ? el.previousSibling : el.nextSibling;
                    if (tp.IsHTMLElement(el2)) {
                        el2.focus();
                    }
                }
            }
        }
    }
    /** Scrolls a page
     * @protected
     * @param {boolean} Up Controls the direction
     */
    PageScroll(Up) {
        Up = Up === true;

        this.pnlViewport.ScrollTop = Up ? this.pnlViewport.ScrollTop - this.ViewportHeight : this.pnlViewport.ScrollTop + this.ViewportHeight;

        var CacheIndex = Up ? this.VisibleRowIndexFirst + 1 : this.VisibleRowIndexLast - 1;

        var elNode = this.ByCacheIndex(CacheIndex);
        if (elNode) {
            elNode.focus();
            if (elNode.children.length > 0) {
                //elNode.children[0].focus();
            }
        }


    }
    /** Scrolls the whole grid
     * @protected
     * @param {boolean} Start When true, the starts from the first row.
     */
    GridScroll(Start) {
        Start = Start === true;
        this.pnlViewport.ScrollTop = Start ? 0 : this.VirtualHeight - this.ViewportHeight;
        this.RenderNodes();
        var CacheIndex = Start ? 0 : Math.floor((this.VirtualHeight / this.RowHeight) - 1);
        var elNode = this.ByCacheIndex(CacheIndex);
        if (elNode) {
            elNode.focus();
            if (elNode.children.length > 0) {
                //elNode.children[0].focus();  
            }
        }
    }

    /* column (internal methods) */ 
    /** Internal. Called when a change happens in a column property
     * @param {tp.GridColumn} Column The {@link tp.GridColumn} column
     * @param {string} PropName The property name
     */
    ColumnChanged(Column, PropName) {
        if (tp.IsSameText('Width', PropName)) {
            this.ColumnResized(Column);
        } else if (tp.IsSameText('Visible', PropName)) {
            this.UpdateColumns();
            this.BuildGroups();
        }
    }
    /**
    A group or value column is already resized.
    @private
    @param {tp.GridColumn} Column The {@link tp.GridColumn} column
    */
    ColumnResized(Column) {
        if (Column.IsValueColumn) {
            this.BuildGroups();
        }
    }
    /**
    Internal. A group or value column is placed (re-ordered) before a value column.
    @param {tp.GridColumn} Column - A group or value column
    @param {tp.GridColumn} RefValueColumn - A value column used as reference.
    */
    ColumnReordered(Column, RefValueColumn) {

        // arrange column lists
        if (Column.IsGroupColumn) {
            tp.ListRemove(this.GroupColumns, Column);
            Column.RemoveFromDom();
        } else {
            tp.ListRemove(this.ValueColumns, Column);
        }

        tp.ListInsert(this.ValueColumns, RefValueColumn.ValueIndex, Column);

        // 1. remove all from dom 2.render all again
        this.UpdateValueColumns();

        // render the grid
        this.BuildGroups();

    }
    /**
    Internal. A group column is re-ordered among group columns or a value column is added to group columns.
    @param {tp.GridColumn} Column - A group or value column
    @param {tp.GridColumn} [RefGroupColumn=null] Optional. Null or a group column used as reference.
    */
    ColumnGrouped(Column, RefGroupColumn = null) {
        var i, ln;

        // arrange column lists
        if (Column.IsValueColumn) {
            tp.ListRemove(this.ValueColumns, Column);
        } else {
            tp.ListRemove(this.GroupColumns, Column);
        }

        if (RefGroupColumn) {
            tp.ListInsert(this.GroupColumns, RefGroupColumn.GroupIndex, Column);
        } else {
            this.GroupColumns.push(Column);
        }

        // 1. remove all from dom 2. render all again
        this.UpdateColumns();

        // render the grid
        this.BuildGroups();

    }
    /**
    Re-renders the group columns
    @private
    */
    UpdateGroupColumns() {
        var i, ln;

        // remove all from dom
        for (i = 0, ln = this.GroupColumns.length; i < ln; i++) {
            this.GroupColumns[i].RemoveFromDom();
        }

        // render all again
        for (i = 0, ln = this.GroupColumns.length; i < ln; i++) {
            if (this.GroupColumns[i].Visible === true)
                this.GroupColumns[i].AppendToGroupColumns(this.ColumnHeight, this.pnlGroups.Handle, this.pnlColumns.Content, this.pnlFilter.Content, this.pnlFooter.Content);
        }
    }
    /**
    Re-renders the group columns
    @private
    */
    UpdateValueColumns() {
        var i, ln;

        // remove all from dom
        for (i = 0, ln = this.ValueColumns.length; i < ln; i++) {
            this.ValueColumns[i].RemoveFromDom();
        }

        // render all again
        for (i = 0, ln = this.ValueColumns.length; i < ln; i++) {
            if (this.ValueColumns[i].Visible === true)
                this.ValueColumns[i].AppendToValueColumns(this.pnlColumns.Content, this.pnlFilter.Content, this.pnlFooter.Content);
        }
    }
    /**
    Re-renders the group and value columns
    @private
    */
    UpdateColumns() {
        this.UpdateGroupColumns();
        this.UpdateValueColumns();
    }

    /* inplace editor show/hide (internal methods) */
    /**
    Internal. Shows the inplace editor by positioning the control to a specified data cell (HTMLElement)
    @param {HTMLElement} Cell - The {@link HTMLElement} container cell for the editor
    */
    ShowEditor(Cell) {
        this.HideEditor(true);

        let Info = this.GetElementInfo(Cell);
        if (Info.Column) {
            var Column = Info.Column;

            if (this.ReadOnly || !this.Enabled || Column.ReadOnly) {
                return;
            }

            this.Editor = Column.Editor;

            if (this.Editor) {
                this.Editor.Show(Cell);                
            } 
        }
    }
    /**
    Internal. Hides the inplace editor
    @param {boolean} PostChanges - The editor posts any changes to the underlying row ONLY if this flag is true.
    */
    HideEditor(PostChanges) {
        if (this.Editor) {
            this.Editor.Hide(PostChanges); 

            this.Editor = null;
        }
    }

    /* sort - filter (internal methods) */
    /**
    Internal.
    */
    DoSort() {

        var i, ln, Info,
            Column,     // tp.GridColumn
            List = [],
            SortItem;   // tp.DataTableSortItem

        for (i = 0, ln = this.GroupColumns.length; i < ln; i++) {
            Column = this.GroupColumns[i];
            if (Column.Visible && tp.DataType.IsSortableType(Column.DataType) && !tp.IsBlank(Column.SortMode)) {
                List.push(Column);
            }
        }

        for (i = 0, ln = this.ValueColumns.length; i < ln; i++) {
            Column = this.ValueColumns[i];
            if (Column.Visible && tp.DataType.IsSortableType(Column.DataType) && !tp.IsBlank(Column.SortMode)) {
                List.push(Column);
            }
        }

        this.DataSource.SortInfoList.Clear();
        for (i = 0, ln = List.length; i < ln; i++) {
            Column = List[i];
            SortItem = this.DataSource.SortInfoList.Add(Column.DataIndex, Column.SortMode === 'desc');    // NameOrIndexOrColumn, Reverse, GetValueFunc
            if (Column.IsLookUp) {
                SortItem.DataType = tp.DataType.String;
                SortItem.LookUpTable = Column.ListSource.Table;
                SortItem.ListValueField = Column.ListValueField;
                SortItem.ListDisplayField = Column.ListDisplayField;
            }
        }

        if (this.DataSource.SortInfoList.Count > 0) {
            this.DataSource.Sort();
        }
    }
    /**
    Internal.
    */
    DoFilter() {
        this.DataSource.Filter();
    }

    /* node and cell info (internal methods) */
    /**
    Returns a node/cell info object associated to a specified HTMLElement
    @private
    @param {HTMLElement} el - The element to get the associated __tpInfo object from.
    @returns {tp.GridElementInfo} Returns the associated __tpInfo object from a DOM element, if any or null.
    */
    GetElementInfo(el) {

        if (tp.IsHTMLElement(el)) {
            if ('__tpInfo' in el && el['__tpInfo'] instanceof tp.GridElementInfo)
                return el['__tpInfo'];
        }

        return null;
    }
    /**
    Associates a node/cell info object to a HTMLElement
    @private
    @param {HTMLElement} el - The element to create/assign the __tpInfo object to.
    @param {tp.GridNode} Node The {@link tp.GridNode} node
    @param {HTMLElement[]} [Cells] Optional.
    @param {tp.GridColumn} [Column] Optional.
    */
    SetElementInfo(el, Node, Cells, Column) {
        if (tp.IsHTMLElement(el)) {
            let Info = new tp.GridElementInfo(Node);
            Info.Cells = Cells;
            Info.Column = Column;
            el['__tpInfo'] = Info;
        }

    }
    /**
    Returns true if a specified DOM element has defined a __tpInfo property.
    @private
    @param {HTMLElement} el - The element to create/assign the __tpInfo object to.
    @returns {boolean} Returns true if a specified DOM element has defined a __tpInfo property..
    */
    HasElementInfo(el) {
        return tp.GetElementInfo(el) !== null;
    }

    /**
    Returns the index of a node associated to a specified data-row (Node.Row === Row), if any, else -1.
    @private
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    @returns {number} Returns the index of a node associated to a specified data-row (Node.Row === Row), if any, else -1.
    */
    NodeIndexOfRow(Row) {
        var i, ln;
        for (i = 0, ln = this.NodeList.length; i < ln; i++) {
            if (this.NodeList[i].IsRow && (this.NodeList[i].Row === Row)) {
                return i;
            }
        }

        return -1;
    }
    /**
    Returns a {@link tp.GridNode} node associated to a specified data-row (Node.Row === Row), if any, else null.
    @private
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    @returns {tp.GridNode} Returns a {@link tp.GridNode} node associated to a specified data-row (Node.Row === Row), if any, else null.
    */
    NodeOf(Row) {
        var Index = this.NodeIndexOfRow(Row);
        return Index !== -1 ? this.NodeList[Index] : null;
    }
    /**
    Returns the {@link HTMLElement} associated to a specified node, if any, else null.
    @private
    @param {tp.GridNode} Node The {@link tp.GridNode} node
    @returns {HTMLElement} Returns the {@link HTMLElement} associated to a specified node, if any, else null.
    */
    NodeElementOf(Node) {
        var el;
        for (var Index in this.Cache) {
            el = this.Cache[Index];
            if (tp.IsHTMLElement(el)) {
                let Info = this.GetElementInfo(el);
                if (Info.Node === Node) {
                    return el;
                }
            }
        }
        return null;
    }
    /**
    Returns the {@link HTMLElement} associated to a data-row, if any, else null.
    @private
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    @returns {HTMLElement} Returns the {@link HTMLElement} associated to a data-row, if any, else null.
    */
    RowElementOf(Row) {
        var el;
        for (var Index in this.Cache) {
            el = this.Cache[Index];
            if (tp.IsHTMLElement(el)) {
                let Info = this.GetElementInfo(el);
                if (Info.Node.IsRow && Info.Node.Row === Row) {
                    return el;
                }
            }

        }

        return null;
    }
    /**  Returns the (cell) {@link HTMLElement} associated to a data-row and grid column, if any, else null.
     * @private
     * @param {tp.DataRow} Row The {@link tp.DataRow} row
     * @param {tp.GridColumn} GridColumn The {@link tp.GridColumn} column
     * @returns {HTMLElement}  Returns the (cell) {@link HTMLElement} associated to a data-row and grid column, if any, else null.
     */
    CellElementOf(Row, GridColumn) {
        var elRow = this.RowElementOf(Row);
        if (tp.IsHTMLElement(elRow)) {
            let Info = this.GetElementInfo(elRow);
            var List = Info.Cells;
            for (var i = 0, ln = List.length; i < ln; i++) {
                Info = this.GetElementInfo(List[i]);
                if (Info.Column === GridColumn) {
                    return List[i];
                }
            }
        }

        return null;
    }

    /* miscs (internal methods) */
    /**
    Returns true if a specified element is contained by this grid.
    @private
    @param {Element | HTMLElement} el The element
    @returns {boolean} Returns true if a specified element is contained by this grid.
    */
    ContainsHandle(el) {
        if (tp.ContainsElement(this.Handle, el))
            return true;

        if (this.Editor && this.Editor.ContainsHandle(el))
            return true;

        return false;
    }
    /**
    Re-calculates and updates row height, column height, group-panel height and group-cell width. <br />
    The row height is the base. The rest of the values come up as a result.
    @private
    */
    UpdateRowHeight() {
        this.RowHeight = tp.GetLineHeight(this.Handle, 1.8);
        this.ColumnHeight = Math.ceil(this.RowHeight * 1.15);
        this.GroupPanelHeight = Math.ceil(this.ColumnHeight * 1.2);

        this.GroupCellWidth = this.RowHeight;
    }
    /**
    Re-calculates the with of a grid row and adjusts the viewport container width properly.
    @private
    */
    UpdateRowWidth() {
        var i, ln,
            w = this.GroupColumns.length * this.GroupCellWidth;

        // width of the group cells
        for (i = 0, ln = this.Columns.length; i < ln; i++) {
            this.Columns[i].SetGroupCellWidth(this.GroupCellWidth);
        }

        //  width
        for (i = 0, ln = this.ValueColumns.length; i < ln; i++) {
            if (this.ValueColumns[i].Visible) {
                w += this.ValueColumns[i].Width;
            }
        }

        this.RowWidth = Math.ceil(w);
        this.ContentWidth = this.RowWidth + 32;
        this.ColumnContentWidth = this.RowWidth + 40;

        if (this.pnlViewport) {
            this.pnlViewport.Content.style.width = tp.px(this.ContentWidth); // + 'px';
            this.pnlColumns.Content.style.width = tp.px(this.ColumnContentWidth); // + 'px';
            this.pnlFilter.Content.style.width = this.pnlColumns.Content.style.width;
            this.pnlFooter.Content.style.width = this.pnlColumns.Content.style.width;
            this.pnlBottom.Content.style.width = this.pnlColumns.Content.style.width;

            this.pnlBottom.Handle.style.display = this.ColumnContentWidth > tp.BoundingRect(this.pnlViewport.Handle).Width ? '' : 'none';
        }


    }
    /**
    Re-calculates and updates the height of the viewport container.
    @private
    */
    UpdateScrollHeight() {
        if (this.IsDataBound && this.pnlViewport) {
            this.ViewportHeight = tp.BoundingRect(this.pnlViewport.Handle).Height;
            this.VirtualHeight = Math.ceil(this.NodeList.length * this.RowHeight) + 3;
            this.pnlViewport.Content.style.height = tp.px(this.VirtualHeight); // + 'px';
        }
    }

    /**
    Internal. Called when a node is expanded/collapsed
    @private
    @param {tp.GridNode} Node The node
    */
    ToggleNode(Node) {
        this.Root.UpdateNodeList();
        this.UpdateScrollHeight();
        this.ClearCache();
        this.RenderNodes();
    }
    /**
    Called when any of the aggregates changes
    @private
    */
    AggregatesChanged() {
        tp.Async(this.BuildGroups, null, this);
        //this.BuildGroups(); 
    }


    /**
    Removes the focused css class from any marked row element in the grid
    @private
    */
    RemoveFocusedClass() {
        var S = tp.Format('.{0}.{1}', tp.Classes.GridRow, tp.Classes.Focused);
        var elRow = tp.Select(this.Handle, S);
        if (elRow) {
            tp.RemoveClass(elRow, tp.Classes.Focused);
        }
    }
    /**
    Sets a specified data-row as the focused row
    @private
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    */
    SetFocusedRow(Row) {
        if (!this.fSettingPosition) {

            this.fSettingPosition = true;
            try {
                if (Row && (Row !== this.fFocusedRow)) {

                    this.RemoveFocusedClass();

                    var elNode = this.RowElementOf(Row);
                    if (elNode) {
                        tp.AddClass(elNode, tp.Classes.Focused);
                    }

                    this.fFocusedRow = Row;
                    this.ScrollIntoView();

                    this.DataSource.Current = Row;
                }
            } finally {
                this.fSettingPosition = false;
            }
        }

    }


    /* ILocatorLink implementation */
    /**
    Sets the Text to Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    @param {string} Text The text to set
    */
    BoxSetText(Locator, Box, Text)  {
        Box.value = Text;
    }
    /**
    Returns the Text of the Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    @returns {string} Returns the Text of the Box
    */
    BoxGetText(Locator, Box) {
        return Box.value;
    }
    /**
    Selects all text in the Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    */
    BoxSelectAll(Locator, Box) {
        Box.select();
    }

    /* public */
    /**
    Sorts on a column.
    @param {number | string | tp.GridColumn} Column - The column index or the column name to operate on
    @param {string} Mode? - Optional. Empty string cancels any existing sorting. The words 'asc' and 'desc' can be used for sorting ascending and descending respectively.
    */
    Sort(Column, Mode) {
        Column = this.GetColumn(Column);
        if (Column)
            Column.Sort(Mode);
    }
    /**
    Applies an aggregate (summary) on a column. If the specified column already has an aggreate, that aggregate is set to the new one.
    @param {number | string | tp.GridColumn} Column - The column index or the column name to apply the aggregate
    @param {tp.AggregateType} Aggregate - The aggregate type to apply on the column. One of the {@link tp.AggregateType} constants
    */
    AddAggregate(Column, Aggregate) {
        Column = this.GetColumn(Column);

        if (Column.fAggregate !== Aggregate && tp.Bf.In(Aggregate, tp.DataType.ValidAggregates(Column.DataType))) {
            Column.fAggregate = Aggregate;

            this.AggregateColumns = [];
            for (var i = 0, ln = this.Columns.length; i < ln; i++) {
                if (this.Columns[i].fAggregate !== tp.AggregateType.None) {
                    this.AggregateColumns.push(this.Columns[i]);
                }
            }

            this.AggregatesChanged();
        }
    }
    /**
     * Adds and returns a new tool-button to the tool-bar
     * @param {string} Command The button command
     * @param {string} Text The button text
     * @param {string} ToolTip The tool-tip
     * @param {string} IcoClasses Ico css classes
     * @param {string} CssClasses Css classes
     * @param {boolean} ToRight Flag, controls the position of the button.
     * @returns {tp.ControlToolButton} Returns a new {@link tp.ControlToolButton} tool-button to the tool-bar
     */
    AddToolBarButton(Command, Text, ToolTip, IcoClasses, CssClasses, ToRight)  {
        return this.pnlToolBar.AddButton(Command, Text, ToolTip, IcoClasses, CssClasses, ToRight);
    }

    /* columns */
    /**
    Clears all column arrays (value, group, aggregate and column array)
    */
    ClearColumns() {
        this.ValueColumns = [];
        this.GroupColumns = [];
        this.AggregateColumns = [];
        this.Columns = [];

        this.pnlGroups.Clear();
        this.pnlColumns.Clear();
        this.pnlFilter.Clear();
        this.pnlFooter.Clear();

        this.fHasAutoGeneratedColumns = false;
    }
    /**
    Adds grid columns based on a specified data-table and a specified array of column names.
    If the array of column names is null or empty, then all data-table columns are added.
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {string[]} [DataColumnNames] Optional. An array of column names.
    */
    AddTableColumns(Table, DataColumnNames) {

        if (!DataColumnNames || DataColumnNames.length === 0) {
            DataColumnNames = [];
            for (let i = 0, ln = Table.ColumnCount; i < ln; i++) {
                DataColumnNames.push(Table.Columns[i].Name);
            }
        }

        let DataColumn; // tp.DataColumn
        for (let i = 0, ln = DataColumnNames.length; i < ln; i++) {
            DataColumn = Table.FindColumn(DataColumnNames[i]);
            if (DataColumn) {
                this.AddColumn(DataColumnNames[i]);
            }
        }
    }
    /**
     * Adds grid columns based on a specified data-table and a list of {@link tp.SelectSqlColumn} columns.
     * @param {tp.DataTable} Table The {@link tp.DataTable} table
     * @param {tp.SelectSqlColumn[]} SelectSqlColumns The list of {@link tp.SelectSqlColumn} columns.
     */
    AddSelectSqlColumns(Table, SelectSqlColumns) {
        let TableColumn, GridColumn;
        SelectSqlColumns.forEach(SqlColumn => {
            TableColumn = Table.FindColumn(SqlColumn.Name);
            if (tp.IsValid(TableColumn)) {
                GridColumn = this.AddColumn(TableColumn.Name, SqlColumn.Title);
                GridColumn.Visible = SqlColumn.Visible;

                // TODO: further adjust tp.GridColumn based on tp.SelectSqlColumn
            }
        });
    }

    /**
    Adds and returns a new {@link tp.GridColumn} grid column. If a grid column is passed the Text parameters are ignored.
    @param {string} Name - The name of the column
    @param {string} [Text] Optional. The column title.
    @returns {tp.GridColumn} Returns the newly added grid column
    */
    AddColumn(Name, Text) {
        return this.InsertColumn(this.Columns.length, Name, Text);
    }
    /**
    Adds a new grid column at a specified index and returns that column. If a grid column is passed the Text and DataType parameters are ignored.
    @param {number} Index The column index
    @param {string} Name - The name of the column
    @param {string} [Text] Optional. The column title.
    @returns {tp.GridColumn} Returns the newly added grid column
    */
    InsertColumn(Index, Name, Text) {
        let Result = new tp.GridColumn(this, Name, Text || Name);
        tp.ListInsert(this.Columns, Index, Result);
        Result.Bind();
        this.fHasAutoGeneratedColumns = false;
        return Result;
    }

    /**
    Adds and returns a new grid lookup column.   
    @param {string} Name - The name of the column
    @param {string} Text The column title.
    @param {string} ListValueField - The name of the value data column in the ListSource
    @param {string} ListDisplayField - The name of the display data column in the ListSource
    @param {tp.DataTable|tp.DataSource} ListSource - The tp.DataSource to be to be used as the source for the item list. It accepts a {@link tp.DataTable} too and uses that table to create the {@link tp.DataSource}.
    @returns {tp.GridColumn} Returns the newly added grid column
    */
    AddLookUpColumn(Name, Text, ListValueField, ListDisplayField, ListSource) {
        return this.InsertLookUpColumn(this.Columns.length, Name, Text, ListValueField, ListDisplayField, ListSource);
    }
    /**
    Adds a new grid lookup column at a specified index and returns that column.  
    @param {number} Index The column index
    @param {string} Name - The name of the column
    @param {string} Text The column title.
    @param {string} ListValueField - The name of the value data column in the ListSource
    @param {string} ListDisplayField - The name of the display data column in the ListSource
    @param {tp.DataTable|tp.DataSource} ListSource - The tp.DataSource to be to be used as the source for the item list. It accepts a {@link tp.DataTable} too and uses that table to create the {@link tp.DataSource}.
    @returns {tp.GridColumn} Returns the newly added grid column
    */
    InsertLookUpColumn(Index, Name, Text, ListValueField, ListDisplayField, ListSource) {
        let Result = this.InsertColumn(Index, Name, Text);
        Result.ListValueField = ListValueField;
        Result.ListDisplayField = ListDisplayField;
        Result.ListSource = ListSource;
        return Result;
    }

    /**
    Adds and returns a new grid locator column.  
    @param {string} Name - The name of the column
    @param {string} Text The column title.
    @param {string} Locator - The locator descriptor name or the locator descriptor for the column.
    @returns {tp.GridColumn} Returns the newly added grid column
    */
    AddLocatorColumn(Name, Text, Locator) {
        return this.InsertLocatorColumn(this.Columns.length, Name, Text, Locator);
    }
    /**
    Adds and returns a new grid locator column.
    @param {number} Index The column index
    @param {string} Name - The name of the column
    @param {string} Text The column title.
    @param {string} Locator - The locator descriptor name or the locator descriptor for the column.
    @returns {tp.GridColumn} Returns the newly added grid column
    */
    InsertLocatorColumn(Index, Name, Text, Locator) {
        let Result = this.InsertColumn(Index, Name, Text);
        Result.LocatorName = Locator;
        return Result;
    }

    /**
    Finds and returns a grid column either by index or name, if any, else null.
    @param {number | string | tp.GridColumn} Column The column index or column name
    @returns {tp.GridColumn} Returns the grid column or null
    */
    GetColumn(Column) {
        if (Column instanceof tp.GridColumn)
            return Column;
        return tp.IsNumber(Column) ? this.Columns[Column] : (tp.IsString(Column) ? this.ColumnByName(Column) : null);
    }
    /**
    Finds and returns a grid column by its name, if any, else null.
    @param {string} Name The column name
    @returns {tp.GridColumn} Returns the grid column or null
    */
    ColumnByName(Name) {
        return tp.FirstOrDefault(this.Columns, (item) => tp.IsSameText(Name, item.Name));
    }

    /**
    Sets the columns of a specified array of column names to be read-only. All other columns are set to be editable.
    @param {string[]} ColumnNames Array of column names
    */
    SetColumnListReadOnly(ColumnNames) {
        if (!tp.IsArray(ColumnNames) || (ColumnNames.length === 0))
            return;

        var Column, i, ln;

        for (i = 0, ln = this.Columns.length; i < ln; i++) {
            Column = this.Columns[i];
            Column.ReadOnly = tp.ListContainsText(ColumnNames, Column.Name);
        }
    }
    /**
    Sets the columns of a specified array of column names to be editable. All other columns are set to be read-only.
    @param {string[]} ColumnNames Array of column names
    */
    SetColumnListWritable(ColumnNames) {
        if (!tp.IsArray(ColumnNames) || (ColumnNames.length === 0))
            return;

        var Column, i, ln;

        for (i = 0, ln = this.Columns.length; i < ln; i++) {
            Column = this.Columns[i];
            Column.ReadOnly = tp.ListContainsText(ColumnNames, Column.Name) === false;
        }
    }
    /**
    Sets the columns of a specified array of column names to be visible. All other columns are set to be hidden.
    @param {string[]} ColumnNames Array of column names
    */
    SetColumnListVisible(ColumnNames) {
        if (!tp.IsArray(ColumnNames) || (ColumnNames.length === 0))
            return;

        var Column, i, ln;

        for (i = 0, ln = this.Columns.length; i < ln; i++) {
            Column = this.Columns[i];
            Column.Visible = tp.ListContainsText(ColumnNames, Column.Name);
        }
    }
    /**
     * Sets the read-only property of a column according to a specified flag.
     * @param {number | string | tp.GridColumn} Column The column index or name.
     * @param {boolean} Flag The flag to set
     */
    SetColumnReadOnly(Column, Flag ) {
        Column = this.GetColumn(Column);
        if (!tp.IsEmpty(Column))
            Column.ReadOnly = Flag;
    }
    /**
     Sets the visible property of a column according to a specified flag.
     * @param {number | string | tp.GridColumn} Column The column index or name.
     * @param {boolean} Flag The flag to set
    */
    SetColumnVisible(Column, Flag) {
        Column = this.GetColumn(Column);
        if (!tp.IsEmpty(Column))
            Column.Visible = Flag;
    }

    /**
    Returns the index of a column
    @param {tp.GridColumn} Column The {@link tp.GridColumn} column
    @returns {number}  Returns the index of a column
    */
    IndexOfColumn(Column) {
        return this.Columns.indexOf(Column);
    }
    /**
    Returns a column by index
    @param {number} Index The column index
    @returns {tp.GridColumn} Returns a {@link tp.GridColumn} column by index
    */
    ColumnByIndex(Index) {
        return this.Columns[Index];
    }

    /**
    Returns the index of a group column
    @param {tp.GridColumn} Column The {@link tp.GridColumn} column
    @returns {number}  Returns the index of a group column
    */
    IndexOfGroupColumn(Column) {
        return this.GroupColumns.indexOf(Column);
    }
    /**
    Returns a group column by index
    @param {number} Index The column index
    @returns {tp.GridColumn} Returns a {@link tp.GridColumn} group column by index
    */
    GroupColumnByIndex(Index) {
        return this.GroupColumns[Index];
    }

    /**
    Returns the index of a value column
    @param {tp.GridColumn} Column The {@link tp.GridColumn} column
    @returns {number} Returns the index of a value column
    */
    IndexOfValueColumn(Column) {
        return this.ValueColumns.indexOf(Column);
    }
    /**
    Returns a value column by index
     @param {number} Index The column index
    @returns {tp.GridColumn} Returns a {@link tp.GridColumn} value column by index
    */
    ValueColumnByIndex(Index) {
        return this.ValueColumns[Index];
    }

    /**
    Returns the index of an aggregate column
    @param {tp.GridColumn} Column The {@link tp.GridColumn} column
    @returns {number} Returns the index of an aggregate column
    */
    IndexOfAggregateColumn(Column) {
        return this.AggregateColumns.indexOf(Column);
    }
    /**
    Returns an aggregate column by index
    @param {number} Index The column index
    @returns {tp.GridColumn} Returns an aggregate {@link tp.GridColumn} column by index
    */
    AggregateColumnByIndex(Index) {
        return this.AggregateColumns[Index];
    }

    /**
    Removes all group columns
    */
    ClearGroupColumns() {
        if (this.GroupColumns.length > 0) {
            var i, ln;


            let List = []; // tp.GridColumn[]

            for (i = 0, ln = this.GroupColumns.length; i < ln; i++) {
                List.push(this.GroupColumns[i]);
            }

            for (i = 0, ln = List.length; i < ln; i++) {
                tp.ListRemove(this.GroupColumns, List[i]);
                this.ValueColumns.push(List[i]);
            }

            // 1. remove all from dom 2. render all again
            this.UpdateColumns();

            // render the grid
            this.BuildGroups();
        }

    }
    /**
    Sets the group columns. It first clears any existing group columns.
    @param {string[]} ColumnNames - A string array with column names to group on.
    */
    SetGroupColumns(ColumnNames) {
        this.ClearGroupColumns();

        var i, ln;
        let Column;  // tp.GridColumn

        for (i = 0, ln = ColumnNames.length; i < ln; i++) {
            Column = this.ColumnByName(ColumnNames[i]);

            if (Column) {
                if (Column.IsValueColumn) {
                    tp.ListRemove(this.ValueColumns, Column);
                    this.GroupColumns.push(Column);
                }
            }
        }

        // 1. remove all from dom 2. render all again
        this.UpdateColumns();

        // render the grid
        this.BuildGroups();
    }
    /**
    Adds a column to group columns
    @param {number | string | tp.GridColumn} Column - The column index or the column name group on
    */
    AddGroupColumn(Column) {
        Column = this.GetColumn(Column);

        if (Column && Column.IsValueColumn) {
            tp.ListRemove(this.ValueColumns, Column);
            this.GroupColumns.push(Column);

            // 1. remove all from dom 2. render all again
            this.UpdateColumns();

            // render the grid
            this.BuildGroups();
        }
    }
    /**
    Removes a column from group columns.
    @param {number | string | tp.GridColumn} Column - The column index or the column name group on
    */
    RemoveGroupColumn(Column) {
        Column = this.GetColumn(Column);

        if (Column && Column.IsGroupColumn) {
            tp.ListRemove(this.GroupColumns, Column);
            this.ValueColumns.push(Column);

            // 1. remove all from dom 2. render all again
            this.UpdateColumns();

            // render the grid
            this.BuildGroups();
        }
    }

    /**
    Adjusts the width of columns so that columns fit their content in the optimal way.
    If a Column is passed then just that column is considered.
    @param {number | string | tp.GridColumn} [Column] Optional.
    */
    BestFitColumns(Column = null) {

        var i, ln, j, jln, el, v, Text, Row,
            SourceElement,  // HTMLElement
            Node,           // tp.GridNode
            Rows = [],
            DataColumnList = [],
            A = [],
            Info;           // tp.GridElementInfo



        // find a data cell to be used as SourceElement
        for (var Index in this.Cache) {
            el = this.Cache[Index];
            if (tp.IsHTMLElement(el)) {
                Info = this.GetElementInfo(el);
                Node = Info.Node;
                if (Node.IsRow) {
                    el = tp.Select(el, '.' + tp.Classes.GridCell);
                    if (tp.IsHTMLElement(el)) {
                        SourceElement = el;
                        break;
                    }
                }
            }


        }

        // data cell found
        if (tp.IsHTMLElement(SourceElement)) {
            var Plus = 12;

            Column = this.GetColumn(Column);

            // get the data column(s)
            if (Column) {
                DataColumnList.push(Column);
            } else {
                for (i = 0, ln = this.Columns.length; i < ln; i++) {
                    if (this.Columns[i].DataColumn && (this.Columns[i].DataIndex !== -1)) {
                        DataColumnList.push(this.Columns[i]);
                    }
                }
            }


            if (DataColumnList.length > 0) {
                el = tp.TextSizeInfo.CreateRulerElement(SourceElement);

                // measure column captions first
                for (i = 0, ln = DataColumnList.length; i < ln; i++) {
                    v = tp.TextSizeInfo.WidthOf(DataColumnList[i].Text, el);
                    A.push(v);
                }

                // measure data row cells
                Rows = this.DataSource.Rows;

                for (j = 0, jln = Rows.length; j < jln; j++) {
                    Row = Rows[j];

                    for (i = 0, ln = DataColumnList.length; i < ln; i++) {
                        v = Row.GetByIndex(DataColumnList[i].DataIndex);
                        Text = DataColumnList[i].Format(v);
                        A[i] = Math.max(A[i], tp.TextSizeInfo.WidthOf(Text, el));
                    }
                }

                el.parentNode.removeChild(el);

                // apply
                for (i = 0, ln = DataColumnList.length; i < ln; i++) {
                    DataColumnList[i].SetWidth(Math.ceil(A[i]) + Plus);
                }

                //this.UpdateRowWidth();
                this.BuildGroups();
            }


        }



    }

    /* rows */
    /**
    Inserts an empty {@link tp.DataRow} row to the grid and returns the newly added data-row
    @returns {tp.DataRow} Returns the {@link tp.DataRow} row
    */
    InsertEmptyRow()  {
        let Result = null;
        if (!tp.IsEmpty(this.DataSource)) {
            Result = this.DataSource.Table.AddEmptyRow();
            this.SetFocusedRow(Result);
        }
        return Result;
    }
    /**
    Deletes a specified {@link tp.DataRow} data-row
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    */
    DeleteRow(Row) {
        if (!tp.IsEmpty(this.DataSource) && !tp.IsEmpty(Row)) {
            var Index, RowIndex = this.DataSource.Rows.indexOf(Row);
            if (RowIndex !== -1) {
                var WasFocusedRow = Row === this.FocusedRow;
                this.DataSource.Table.RemoveRow(Row);
                if (WasFocusedRow === true) {
                    if (tp.InRange(this.DataSource.Rows, RowIndex)) {
                        this.SetFocusedRow(this.DataSource.Rows[RowIndex]);
                    } else if (tp.InRange(this.DataSource.Rows, RowIndex - 1)) {
                        this.SetFocusedRow(this.DataSource.Rows[RowIndex - 1]);
                    } else if (tp.InRange(this.DataSource.Rows, RowIndex + 1)) {
                        this.SetFocusedRow(this.DataSource.Rows[RowIndex + 1]);
                    }
                }
            }
        }


    }
    /**
    Returns true if a specified  {@link tp.DataRow} data row is (falls) into view.
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    @returns {boolean} Returns true if a specified data row is (falls) into view.
    */
    IsRowVisible(Row) {
        if (!tp.IsEmpty(Row)) {
            var elRow = this.RowElementOf(Row);
            if (tp.IsHTMLElement(elRow)) {
                var top = this.pnlViewport.ScrollTop;
                var bottom = top + tp.BoundingRect(this.pnlViewport.Handle).Height; // this.pnlViewport.OffsetHeight;

                var R = tp.OffsetRect(elRow);
                var Result = R.Y >= top && (R.Y + R.Height) <= bottom;
                return Result;
            }
        }

        return false;
    }
    /**
    Scrolls a specified data row or the focused row into view, only if row is not visible.
    @param {tp.DataRow} [Row] - Optional. The data row to operate on or null to operate on the focused row.
    */
    ScrollIntoView(Row) {

        if (this.VisibleRowIndexFirst === 0 && this.VisibleRowIndexLast === 0)  // RenderNodes() not yet called
            return;

        Row = Row || this.fFocusedRow;

        if (this.IsRowVisible(Row))
            return;

        var Index = this.NodeIndexOfRow(Row);

        if (Index !== -1) {
            var Node = this.NodeList[Index];
            if (Node.IsRow) {

                // do nothing if it is already in view
                if ((Index < this.VisibleRowIndexFirst) || (Index > this.VisibleRowIndexLast)) {
                    if (Index > 5) {
                        Index = Index - 1;
                    }

                    this.pnlViewport.ScrollTop = Index * this.RowHeight;
                    this.RenderNodes();
                }
            }
        }

    }

    /**
    Shows or hides the columns that end with 'Id'.
    */
    ShowIdGridColumns(Flag) {
        let i, ln, Column;

        for (i = 0, ln = this.Columns.length; i < ln; i++) {
            Column = this.Columns[i];
            if (tp.Db.IsIdColumn(Column.Name)) {
                Column.Visible = Flag === true;
            }
        }

        this.fShowIdColumnsFlag = Flag === true;
    }
    /**
    Shows or hides the columns that end with 'Id'.
    */
    ShowHideIdGridColumns() {       
        this.ShowIdGridColumns(!this.fShowIdColumnsFlag);
    }

    /* locators */
    /**
    Finds and returns a locator by locator descriptor name, if any, else null.
    @param {string} Name The locator descriptor name
    @returns {tp.Locator} Returns a {@link tp.Locator} locator or null
    */
    FindLocator(Name) {
        return tp.FirstOrDefault(this.Locators, (item) => { return tp.IsSameText(Name, item.Descriptor.Name); });
    }
    /**
    Finds and returns a locator by field name, if any, else null.
    @param {string} DataField Field name
    @returns {tp.Locator} Returns a  {@link tp.Locator}  locator or null
    */
    FindLocatorByDataField(DataField) {
        return tp.FirstOrDefault(this.Locators, (item) => { return tp.IsSameText(DataField, item.DataField); });
    }
    /**
    Adds a locator to the locators of this grid and returns a {@link Promise} with a {@link tp.Locator} locator.
    For a locator to be created a locator descriptor is required. Locator descriptors are keyp by the tp.Registry.
    If a locator descriptor is not found in the registry, the registry asks the descriptor from the server.
    @param {string} DataField - The field up on the locator is to be created. This is the field where the locator puts its value.
    @param {string | tp.LocatorDef} NameOrDescriptor - A locator descriptor name or a locator descriptor. It is required for creating a locator
    @returns {tp.Locator} Returns a {@link Promise} with a {@link tp.Locator} locator.
    */
    async AddLocatorAsync(DataField, NameOrDescriptor) {

        let Locator = this.FindLocatorByDataField(DataField);

        if (Locator)
            return Locator;

        if (NameOrDescriptor instanceof tp.LocatorDef) {
            Locator = new tp.Locator();
            Locator.DataField = DataField;
            Locator.Descriptor = NameOrDescriptor;
            Locator.Control = this;
            this.Locators.push(Locator);
        }
        else if (tp.IsString(NameOrDescriptor)) {
            NameOrDescriptor = await tp.Locators.GetDefAsync(NameOrDescriptor);
            if (NameOrDescriptor) {
                Locator = new tp.Locator();
                Locator.DataField = DataField;
                Locator.Descriptor = NameOrDescriptor;
                Locator.Control = this;
                this.Locators.push(Locator);
            }
        }
 
        if (!Locator) {
            tp.Throw(`Can not add Locator to grid column/field ${DataField}`);
        }

        return Locator;
    }
    /**
    Removes a locator from the locators of this grid by locator descriptor name.
    @param {string} Name Locator descriptor name
    */
    RemoveLocator(Name) {
        let Locator = this.FindLocator(Name);
        if (Locator) {
            tp.ListRemove(this.Locators, Locator);
        }
    }
    /**
    Removes all locators from this grid.
    */
    ClearLocators() {
        this.Locators.length = 0;
    }


    /* Event triggers */
    /**
    Event trigger.
    If the button Command property is GridRowInsert or GridRowDelete
    and the event args Handled property remains false after calling the listeners,
    then the Command is handled properly.
    @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
    */
    OnToolBarButtonClick(Args) {
        if (tp.IsEmpty(this.DataSource)) {
            return;
        }

        var self = this;
        //let Args = new tp.ToolButtonClickEventArgs(Button); 

        this.Trigger('ToolBarButtonClick', Args);

        if (Args.Handled === false) {
            var Row = null;

            switch (Args.Command) {
                case 'GridRowInsert':
                    if (!this.ReadOnly && this.Enabled && this.AllowUserToAddRows)
                        this.InsertEmptyRow();
                    break;
                case 'GridRowDelete':
                    if (!this.ReadOnly && this.Enabled && this.AllowUserToDeleteRows) {
                        Row = this.FocusedRow;
                        if (!tp.IsEmpty(Row)) {
                            tp.YesNoBox('Delete selected row?', function (Args) {
                                if (Args.Window.DialogResult === tp.DialogResult.Yes) {
                                    self.DeleteRow(Row);
                                }
                            });
                        }
                    }
                    break;
            }

            Args.Handled = true;
        }
    }

    /* Event handlers */
    /**
    Event handler
    */
    Viewport_OnResized() {
        //this.UpdateScrollHeight();
        //this.RenderNodes();
        //this.BuildGroups();

        this.HideEditor(false);
        this.BuildGroups();
    }
    /**
    Event handler
    @param {Event} e The {@link Event} object
    @returns {boolean} Returns false
    */
    Viewport_OnScroll(e) {
        this.HideEditor(false);
        this.RenderNodes();

        return false;
    }
    /**
    @param {Event} e The {@link Event} object
    @returns {boolean} Returns false
    */
    Bottom_OnScroll(e) {
        if (e) {
            this.pnlViewport.ScrollLeft = this.pnlBottom.ScrollLeft;
            this.pnlColumns.ScrollLeft = this.pnlViewport.ScrollLeft;
            this.pnlFilter.ScrollLeft = this.pnlColumns.ScrollLeft;
        }

        return false;
    }
    /**
    @param {MouseEvent} e The {@link MouseEvent} object
    */
    AnyContextMenu(e) {
        var Info, Column = null;
        if (e && tp.IsHTMLElement(e.target)) {
            let el = e.target;
            let Column = null; // tp.GridColumn

            if (tp.HasClass(el, tp.Classes.SummaryCell)) {
                let Info = this.GetElementInfo(el);
                if (Info && Info.Column)
                    Column = Info.Column;

                if (!Column) {
                    Column = tp.GridColumn.GetInfo(el);
                }
            }


            if (Column) {
                e.preventDefault();

                var ValidAggregateTypes = tp.DataType.ValidAggregates(Column.DataType);
                ValidAggregateTypes = tp.Bf.Subtract(ValidAggregateTypes, Column.fAggregate);

                var MenuItem; // tp.MenuItemBase;
                for (var i = 0, ln = this.mnuAggregates.Count; i < ln; i++) {
                    MenuItem = this.mnuAggregates.ByIndex(i);
                    if (MenuItem.Tag !== tp.AggregateType.None) {
                        MenuItem.Visible = tp.Bf.In(MenuItem.Tag, ValidAggregateTypes);
                    }
                }

                this.mnuAggregates.Tag = Column;
                this.mnuAggregates.Show(e);
            }

        }



    }
    /**
    Event handler
    @param {tp.MenuEventArgs} Args The {@link tp.MenuEventArgs} arguments
    */
    mnuAggregates_ItemClick(Args) {
        var self = this;

        setTimeout(function () {
            if (self.mnuAggregates.Tag instanceof tp.GridColumn) {
                var Column = self.mnuAggregates.Tag;
                var Aggregate = Args.MenuItem.Tag;
                self.AddAggregate(Column, Aggregate);
            }
        }, 300);
    }



};

/** Field. The expand symbol.
 * @static
 * @type {string}
 */
tp.Grid.ExpandSymbol = '▸';      // + ▸ ► 
/** Field. The collapse symbol.
 * @static
 * @type {string}
 */
tp.Grid.CollapseSymbol = '▾';    // - ▾ ▼   


 
/* private */
/** Field
 @private
 @type {tp.GridRootNode}
 */
tp.Grid.prototype.Root;
/** Field
 @private
 @type {tp.ControlToolBar}
 */
tp.Grid.prototype.pnlToolBar;
/** Field
 @private
 @type {tp.GridGroupPanel}
 */
tp.Grid.prototype.pnlGroups;
/** Field
 @private
 @type {tp.GridColumnPanel}
 */
tp.Grid.prototype.pnlColumns;
/** Field
 @private
 @type {tp.GridFilterPanel}
 */
tp.Grid.prototype.pnlFilter;
/** Field
 @private
 @type {tp.GridViewportPanel}
 */
tp.Grid.prototype.pnlViewport;
/** Field
 @private
 @type {tp.GridSummariesPanel}
 */
tp.Grid.prototype.pnlFooter;
/** Field
 @private
 @type {tp.GridBottomPanel}
 */
tp.Grid.prototype.pnlBottom;


/** Field
 @protected
 @type {tp.GridColumn[]}
 */
tp.Grid.prototype.Columns;
/** Field
 @protected
 @type {tp.GridColumn[]}
 */
tp.Grid.prototype.HiddenColumns;
/** Field
 @protected
 @type {tp.GridColumn[]}
 */
tp.Grid.prototype.ValueColumns;
/** Field
 @protected
 @type {tp.GridColumn[]}
 */
tp.Grid.prototype.GroupColumns;
/** Field
 @protected
 @type {tp.GridColumn[]}
 */
tp.Grid.prototype.AggregateColumns;
/** Field
 @protected
 @type {tp.GridInplaceEditor}
 */
tp.Grid.prototype.Editor;
/** Field
 @protected
 @type {tp.Locator[]}
 */
tp.Grid.prototype.Locators;
/** Field. Context menu
 @protected
 @type {tp.ContextMenu}
 */
tp.Grid.prototype.mnuAggregates;        
/** Field
 @protected
 @type {tp.ControlToolButton}
 */
tp.Grid.prototype.fButtonInsert;
/** Field
 @protected
 @type {tp.ControlToolButton}
 */
tp.Grid.prototype.fButtonEdit;
/** Field
 @protected
 @type {tp.ControlToolButton}
 */
tp.Grid.prototype.fButtonDelete;
/** Field
 @protected
 @type {tp.ControlToolButton}
 */
tp.Grid.prototype.fButtonFind;

/* fields - internal */
/**
Internal. A flat list of nodes. Each row in the grid is a "node" (group, group footer, or row node)
*/

/** Internal Field.
 @type {tp.GridNode[]}
 */
tp.Grid.prototype.NodeList;
/** Internal Field.
 @type {tp.GridColumn}
 */
tp.Grid.prototype.DraggedColumn;
/** Internal Field.
 @type {tp.DataRow}
 */
tp.Grid.prototype.fFocusedRow;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.RowHeight;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.VirtualHeight;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.ViewportHeight;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.GroupPanelHeight;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.ColumnHeight;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.RowWidth;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.ContentWidth;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.ColumnContentWidth;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.GroupCellWidth;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.LastScrollTop;
/** Internal Field.
 @type {object}
 */
tp.Grid.prototype.Cache;
/** Internal Field. When > 0, prohibits BuildGroups()
 @type {number}
 */
tp.Grid.prototype.fAlteringDataCounter;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.VisibleRowIndexFirst;
/** Internal Field.
 @type {number}
 */
tp.Grid.prototype.VisibleRowIndexLast;
/** Internal Field.
 @type {boolean}
 */
tp.Grid.prototype.IsBinding;
/** Internal Field.
 @type {boolean}
 */
tp.Grid.prototype.fShowIdColumnsFlag;
/** Internal Field.
 @type {boolean}
 */
tp.Grid.prototype.fSettingPosition;
/** Internal Field.
 @type {boolean}
 */
tp.Grid.prototype.fHasAutoGeneratedColumns;
/** Internal Field.
 @type {boolean}
 */
tp.Grid.prototype.fResizing;



/* fields - public */
/**
Gets or sets a boolean value indicating whether to display a confirm dialog when the user deletes a row. Defaults to true.
@type {boolean}
*/
tp.Grid.prototype.ConfirmDelete;
/**
Gets or sets a boolean value indicating whether to allow the user to add new rows. Defaults to false;
@type {boolean}
*/
tp.Grid.prototype.AllowUserToAddRows;
/**
Gets or sets a boolean value indicating whether to allow the user to delete rows. Defaults to false;
@type {boolean}
*/
tp.Grid.prototype.AllowUserToDeleteRows;
/**
Gets or sets a boolean value indicating whether to allow the user to re-order columns. Defaults to true;
@type {boolean}
*/
tp.Grid.prototype.AllowUserToOrderColumns;
/**
Gets or sets a boolean value indicating whether to allow the user to resize columns. Defaults to true;
@type {boolean}
*/
tp.Grid.prototype.AllowUserToResizeColumns;
/**
Gets or sets a boolean value indicating whether to generate columns when the datasource is assigned and no columns defined. Defaults to true;
@type {boolean}
*/
tp.Grid.prototype.AutoGenerateColumns;

/**
Shows or hides the footer panel for all groups. Defaults to true.
@type {boolean}
*/
tp.Grid.prototype.GroupFooterVisible;


tp.Ui.Types.Grid = tp.Grid;         // Adds the grid class to the tripous user interface types.

//#endregion
























