
/* eslint no-constant-condition: ["error", { "checkLoops": false }] */



if (!window.log) {
    window.log = function (v) {
        try {
            if (console) {
                console.log(v);
            }
        } catch (e) {
            //
        }
    };
}


/**
* Selects an element specified by a selector.
* If Selector is a string, then it returns the first found element in document, if any, else null.
* If Selector is already an element, returns that element.
* Else returns null.
* @param {Element|string} Selector - An element or a string selector.
* @return {Element|null} Returns an element, if any, or null.
* @class
*/
var tp = function (Selector) {
    if (tp.IsString(Selector)) {
        return document.querySelector(Selector);
    } else if (tp.IsElement(Selector)) {
        return Selector;
    }

    return null;
};

//---------------------------------------------------------------------------------------
// errors and notifications
//---------------------------------------------------------------------------------------

//#region Errors

/**
 * Throws a tripous exception
 * @param {string} Message The exception message
 */
tp.Throw = function (Message) {
    let Ex = new Error(Message);
    Ex.name = 'Tripous Error';
    throw Ex;
};
 
/** global error handling */
window.addEventListener("error", function (e) {
    if (tp.SysConfig.GlobalErrorHandling) {
        tp.ForceHideSpinner();
        tp.Logger.Error(e);
    }
    return false;
});

/** global error handling */
window.addEventListener('unhandledrejection', function (e) {
    if (tp.SysConfig.GlobalErrorHandling) {
        tp.ForceHideSpinner();
        e.preventDefault();
        tp.Logger.Error(e);
    }
});

/**
Returns a text of an error for display purposes
@param {any} e - An Error, or ErrorEvent, or a string value or any other object with error information
@returns {string} Returns the error text
*/
tp.ExceptionText = function (e) {
    let SB = new tp.StringBuilder();
    let S, o;

    //---------------------------
    function HandleEvent(e) {
        o = { Message: 'Unknown error.' };
        try {
            o = {};

            o.Name = e.error && e.error.name ? e.error.name : '';
            o.Message = e.message;
            o.Type = e.type ? e.type : '';
            o.Number = e.error && e.error.number ? e.error.number : null;
            o.File = e.filename;
            o.Line = e.lineno;
            o.Col = e.colno;
            o.Stack = e.error && e.error.stack ? e.error.stack : '';

            if (e instanceof PromiseRejectionEvent) {
                if (!tp.IsEmpty(e.reason)) {
                    if (e.reason instanceof Error) {
                        o.Stack = e.reason.stack;
                    } else {
                        o.Reason = e.reason.toString();
                    }
                }
            }

        } catch (e) {
            //
        }

        for (let Prop in o) {
            let v = o[Prop];
            if (v && tp.IsSimple(v)) {
                v = v.toString();
                if (!tp.IsBlank(v)) {
                    S = tp.Format('{0}: {1}', Prop,v);
                    SB.AppendLine(S);
                }
            } 
        }
    }
    //---------------------------


    if (tp.IsString(e) && !tp.IsBlank(e)) {
        SB.AppendLine(e);
    } else if (e instanceof Error) {
        SB.AppendLine(e.name + ': ' + e.message);
        SB.AppendLine(e.stack || 'Stack not available.');
    } else if (e instanceof PromiseRejectionEvent) {
        HandleEvent(e);
    } else if (e instanceof ErrorEvent) {
        HandleEvent(e);
    } else if ('ErrorText' in e && !tp.IsBlank(e.ErrorText)) {
        SB.AppendLine(e.ErrorText);
    } else if (e instanceof tp.AjaxArgs) {
        if (!tp.IsBlank(e.ErrorText))
            SB.AppendLine(e.ErrorText);
        else if (!tp.IsBlank(e.ResponseData.ErrorText))
            SB.AppendLine(e.ResponseData.ErrorText);
        else
            SB.AppendLine('Unknown ajax error.');
    } else {
        try {
            S = e.toString();
            SB.AppendLine(S);
        } catch (e) {
            SB.AppendLine('Unknown error.');
        }
    }
    return SB.ToString();
};

//#endregion

//#region Notifications

/** Enum type static clas for notification types 
 @class
 @enum
 */
tp.NotificationType = {
    Information: 1,
    Warning: 2,
    Error: 4,
    Success: 5
};
Object.freeze(tp.NotificationType);

/** Settings static class for the notification boxes 
 @class
 */
tp.NotificationBoxSetup = {
    Information: {
        Title: 'Information2',
        BackColor: '#FFFFD7',
        BorderColor: '#FFEB3B'
    },
    Warning: {
        Title: 'Warning',
        BackColor: '#E7FFE7',
        BorderColor: '#4CAF50'
    },
    Error: {
        Title: 'Error',
        BackColor: '#FFE7E7',
        BorderColor: '#F44336'
    },
    Success: {
        Title: 'Success',
        BackColor: '#E7FFFF',
        BorderColor: '#2196F3'
    },

    ToTop: false,
    DurationSecs: 10,
    Height: 110,
    Width: 350
};

/** helper static class for notification boxes. 
 Provides the bottom of the next notification box. 
 @class
 */
tp.NotificationBoxes = (function () {
    let Boxes = [];

    return {
        /**
         * Adds an element representing a notification box to an internal list.
         * @param {Element} el The notification box element
        * @memberof tp.NotificationBoxes 
        * @static
         */
        Add: function (el) { Boxes.push(el); },
        /**
         * Removes an element representing a notification box from the internal list.
         * @param {Element} el The notification box element
         * @memberof tp.NotificationBoxes
         * @static
         */
        Remove: function (el) { tp.ListRemove(Boxes, el); },

        Place: function (Box) {
            let R, T;

            let RefBox = Boxes.length > 0 ? Boxes[Boxes.length - 1] : null;


            if (tp.NotificationBoxSetup.ToTop === true) {
                T = 5;
                if (RefBox) {
                    R = tp.BoundingRect(RefBox);
                    T = T + R.Y + R.Height + 5;
                }
            }
            else {
                let ViewPortSize = tp.Viewport.GetSize();
                T = ViewPortSize.Height - 10;
                if (RefBox) {
                    R = tp.BoundingRect(RefBox);
                    T = R.Y - 10;
                }

                R = tp.BoundingRect(Box);
                T -= R.Height;
            }

            Box.style.top = tp.px(T);
        },


        /** Returns the value of the bottom property, as a number, of the next notification box. 
         @return {number} Returns the value of the bottom property, as a number, of the next notification box.
         @memberof tp.NotificationBoxes
         @static
         */
        get Bottom() {
            let Result = 5;
            if (Boxes.length > 0) {
                let Box = Boxes[Boxes.length - 1];
                let CS = tp.GetComputedStyle(Box);
                Result = Result + tp.ExtractNumber(CS.bottom) + tp.ExtractNumber(CS.height);
            }

            return Result;
        },
        /** Returns the value of the top property, as a number, of the next notification box. 
         @return {number} Returns the value of the top property, as a number, of the next notification box.
         @memberof tp.NotificationBoxes
         @static
         */
        get Top() {
            let Result = 5;
            if (Boxes.length > 0) {
                let Box = Boxes[Boxes.length - 1];
                let CS = tp.GetComputedStyle(Box);
                Result = Result + tp.ExtractNumber(CS.top) + tp.ExtractNumber(CS.height);
            }

            return Result;
        },
        /** Returns the maximum z-index of all registered boxes 
         @type {integer}
         @memberof tp.NotificationBoxes
         @static
         */
        get MaxZIndex() {
            let Result = tp.MaxZIndexOf(tp.Doc.body);
            let n = 0;
            if (Boxes.length >= 0) {
                for (let i = 0, ln = Boxes.length; i < ln; i++) {
                    n = tp.ZIndex(Boxes[i]);
                    n = tp.ExtractNumber(n);
                    Result = Math.max(Result, n);
                }
            }

            return Result;
        }
    };

})();


/**
Displays a notification message to the user.
@param {string} Message The notification message
@param {tp.NotificationType} Type The type of notification, i.e. Warning, Error, etc
*/
tp.NotifyFunc = function (Message, Type) {
 
    let Setup = tp.NotificationBoxSetup[tp.EnumNameOf(tp.NotificationType, Type)];
 
    let Text = `${Setup.Title}
${Message}
`;
    alert(Text);
     
};
/**
Displays a notificaion message to the user.
@param {string} Message The notification message
@param {tp.NotificationType} Type The type of notification, i.e. Warning, Error, etc
*/
tp.Notify = function (Message, Type) {
    if (tp.NotifyFunc) {
        tp.NotifyFunc(Message, Type);
    } else {
        log(Message);
    }
};
/**
Displays an information message to the user.
@param {string} Message The notification message
*/
tp.InfoNote = function (Message) { tp.Notify(Message, tp.NotificationType.Information); };
/**
Displays an information message to the user.
@param {string} Message The notification message
*/
tp.WarningNote = function (Message) { tp.Notify(Message, tp.NotificationType.Warning); };
/**
Displays an information message to the user.
@param {string} Message The notification message
*/
tp.ErrorNote = function (Message) { tp.Notify(Message, tp.NotificationType.Error); };
/**
Displays an information message to the user.
@param {string} Message The notification message
*/
tp.SuccessNote = function (Message) { tp.Notify(Message, tp.NotificationType.Success); };



//#endregion

//---------------------------------------------------------------------------------------
// Logger
//---------------------------------------------------------------------------------------

//#region Logger

/** A static class that informs registered listeners on Information, Warning, Error and Success events. <br />
 * This class does not persist messages to any medium. It just informs its registered listeners.
 */
tp.Logger = {};
tp.Logger._Propagate = function (v, Type) {
    if (v) {
        try {
            let Text = '';
            if (Type === tp.NotificationType.Error && (v instanceof Error || v instanceof ErrorEvent || v instanceof PromiseRejectionEvent || v instanceof tp.AjaxArgs))
                Text = tp.ExceptionText(v);
            else
                Text = v.toString();

            if (this.DisplayNotificationBoxes === true)
                tp.NotifyFunc(Text, Type);

            let listener;
            for (var i = 0, ln = this.Listeners.length; i < ln; i++) {
                listener = this.Listeners[i];
                listener.Func.call(listener.Context, Text, Type);
            }
        } catch (e) {
            //
        }
    }

};
/**
     * Info
     * @param {string} Text The text
     */
tp.Logger.Info = function (Text) {
    this._Propagate(Text, tp.NotificationType.Information);
};
/**
    * Warning
    * @param {string} Text The text
    */
tp.Logger.Warning = function (Text) {
    this._Propagate(Text, tp.NotificationType.Warning);
};
/**
* Error
* @param {Error|ErrorEvent|tp.AjaxArgs} ExceptionOrText The error object
*/
tp.Logger.Error = function (ExceptionOrText) {
    this._Propagate(ExceptionOrText, tp.NotificationType.Error);
};
/**
* Success
* @param {string} Text The text
*/
tp.Logger.Success = function (Text) {
    this._Propagate(Text, tp.NotificationType.Success);
};
/**
    * Registers a listener to the logger
    * @param {Function} Func A function as <code>void Func(Text: string, Type: tp.NotificationType) </code>
    * @param {Object} [Context] Optional. The this context of the function.
    */
tp.Logger.AddListener = function (Func, Context = null) {
    var Listener = new tp.Listener(Func, Context);
    this.Listeners.push(Listener);
};


/** Field
 * @private
 * @type {tp.Listener[]} */
tp.Logger.Listeners = [];
/** Public field. When true, the default, then any incoming message is displayed to the user, using the notification boxes.
 * @public
 * @type {boolean}
 * */
tp.Logger.DisplayNotificationBoxes = true;



//#endregion


//---------------------------------------------------------------------------------------
// tp functions
//---------------------------------------------------------------------------------------

//#region Type checking
/** Type checking function. Returns true if the specified value is null or undefined.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsEmpty = function (v) { return v === null || v === void 0; };               // null or undefined
/** Type checking function. Returns true if the specified value is not null nor undefined.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsValid = function (v) { return !(v === null || v === void 0); };

/** Type checking function. Returns true if the specified value is an object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsObject = function (v) { return tp.IsValid(v) && typeof v === 'object'; };
/** Type checking function. Returns true if the specified value is an array.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsArray = function (v) { return v instanceof Array || Object.prototype.toString.call(v) === '[object Array]'; };
/** Type checking function. Returns true if the specified value is a function.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsFunction = function (v) { return typeof v === 'function'; };
/** Type checking function. Returns true if the specified value is an arguments object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsArguments = function (v) { return Object.prototype.toString.call(v) === "[object Arguments]"; };

/** Type checking function. Returns true if the specified value is string.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsString = function (v) { return typeof v === 'string'; };
/** Type checking function. Returns true if the specified value is number.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsNumber = function (v) { return typeof v === 'number'; };
/** Type checking function. Returns true if the specified value is integer.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsInteger = function (v) { return typeof v === 'number' && v % 1 === 0; }
/** Type checking function. Returns true if the specified value is decimal number.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsFloat = function (v) { return typeof v === 'number' && n % 1 !== 0; }
/** Type checking function. . Returns true if the specified value is boolean.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsBoolean = function (v) { return typeof v === 'boolean'; };
/** Type checking function. . Returns true if the specified value is a date object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsDate = function (v) { return !tp.IsEmpty(v) && Object.prototype.toString.call(v) === "[object Date]"; };
/** Type checking function. Returns true if the specified value is string, number or boolean.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsPrimitive = function (v) { return typeof v === 'string' || typeof v === 'number' || typeof v === 'boolean'; };
/** Type checking function. Returns true if the specified value is string, number, boolean or date.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsSimple = function (v) { return tp.IsPrimitive(v) || tp.IsDate(v); };

/** Type checking function. Returns true if the specified value is a promise object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsPromise = function (v) { return v instanceof Promise; };
/** Type checking function. Returns true if the specified value is a RegExp object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsRegExp = function (v) { return Object.prototype.toString.call(v) === "[object RegExp]"; };

/** Type checking function. Returns true if the specified value is an object but NOT a DOM element.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsPlainObject = function (v) { return tp.IsObject(v) && !v.nodeType; };
/** Type checking function. Returns true if the specified value is a DOM {@link Node}.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsNode = function (v) { return v instanceof Node; };

/**
 * Returns true if a specified string is a json string.
 * @param {string} Text The text to check
 * @returns {boolean} Returns true if a specified string is a json string.
 */
tp.IsJson = function (Text) {
    if (typeof Text === 'string' && !tp.IsBlank(Text)) {
        try {
            let o = JSON.parse(Text);
            return (o && typeof o === "object");
        } catch (e) {
            //
        }
    }

    return false;
};


/** Type checking function for a DOM Node. Returns true if the specified value is a DOM attribute Node.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsAttribute = function (v) { return !!(v && v.nodeType === Node.ATTRIBUTE_NODE); };
/** Type checking function for a DOM Node. Returns true if the specified value is a DOM {@link Element}.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsElement = function (v) { return v instanceof Element; };
/** Type checking function for a DOM Node. Returns true if the specified value is a DOM {@link HTMLElement}.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsHTMLElement = function (v) { return v instanceof HTMLElement; };
/** Type checking function for a DOM Node. Returns true if the specified value is a DOM text node.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsText = function (v) { return !!(v && v.nodeType === Node.TEXT_NODE); };

/**
Type guard function for an HTMLElement that has a name attribute.
@param {Element} v - The element to check.
@returns {boolean} -
*/
tp.IsNamedHtmlElement = function (v) { return v instanceof HTMLElement && 'name' in v; };
/**
Type guard for an element that provides the querySelector() and querySelectorAll() methods, i.e. is document or Element
@param {Element} v - The element to check.
@returns {boolean} - Returns true if the specified element provides querySelector() and querySelectorAll() methods.
*/
tp.IsNodeSelector = function (v) { return tp.IsValid(v) && 'querySelector' in v && 'querySelectorAll' in v; };
/**
Type-guard function. An element providing checkValidity() and reportValidity() methods passes this test.
@see {@link https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/HTML5/Constraint_validation|Constraint validation}
@param {Element} v - The element to check.
@returns {boolean} - Returns true if the specified element provides checkValidity() and reportValidity() methods.
*/
tp.IsValidatableElement = function (v) { return !tp.IsEmpty(v) && tp.IsFunction(v['checkValidity']) && tp.IsFunction(v['setCustomValidity']); };
/**
* Type-guard, mostly for the required property of form elements
* @param {Element} v - The element to check.
* @returns {boolean} -
*/
tp.IsFormElement = function (v) { return v instanceof HTMLInputElement || v instanceof HTMLSelectElement || v instanceof HTMLTextAreaElement; };

/**
Type guard function for the Cloneable interface
* @param {Element} v - The element to check.
* @returns {boolean} -
*/
tp.IsCloneable = function (v) { return !tp.IsEmpty(v) && tp.IsFunction(v['Clone']); };
/**
Type guard function for the Assignable interface
* @param {Element} v - The element to check.
* @returns {boolean} -
*/
tp.IsAssignable = function (v) { return !tp.IsEmpty(v) && tp.IsFunction(v['Assign']); };

/**
True if a specified value is a DOM element of a certain type (e.g div, span, etc.)
@param {any} v Any value
@param {string} NodeName The node name  (e.g div, span, etc.)
@returns {boolean} Returns true if a specified value is a DOM element of a certain type (e.g div, span, etc.)
*/
tp.ElementIs = function (v, NodeName) { return tp.IsElement(v) && tp.IsSameText(v.nodeName, NodeName); };
//#endregion

//#region Class construction and inheritance
tp.SetBaseClass = function (Class, BaseClass) {
    /// <summary>
    /// Sets BaseClass as the base class of Class.
    /// <para> Actually is a shortcut just to avoid writing the same code lines everywhere. </para>
    /// </summary>

    // new f() 
    //      produces a new object that inherits from
    // f.prototype   

    // see: http://stackoverflow.com/questions/9959727/proto-vs-prototype-in-javascript
    Class.prototype = Object.create(BaseClass.prototype);  // the prototype to be used when creating new instances
    Class.prototype.constructor = Class;                   // the function that used as the constructor
    return BaseClass.prototype;                            // return the base prototype, it is stored in a private variable inside class closures
};
tp.Property = function (Name, Prototype, GetFunc, SetFunc) {
    /// <summary>Defines a named or accessor property in a class prototype</summary>
    var o = {};
    if (tp.IsFunction(GetFunc)) {        // it is a named accessor property
        o.get = GetFunc;
        if (tp.IsFunction(SetFunc)) {    // if not present, it effectively creates a read-only property
            o.set = SetFunc;
        }
    } else {                            // it is a named property
        o.value = GetFunc;
        o.writable = true;
    }
    o.enumerable = true;
    o.configurable = true;
    Object.defineProperty(Prototype, Name, o);
};
tp.Constant = function (Name, Prototype, Value) {
    /// <summary>Defines a constant in a class prototype</summary>
    var o = {
        value: Value,
        writable: false,
        enumerable: false,
        configurable: false
    };

    Object.defineProperty(Prototype, Name, o);
};

/**
Invokes a constructor and returns the new instance
@param {function} Ctor A constructor function
@param {...args} args A rest parameter
@returns {object} Returns the new instance of the specified constructor.
*/
tp.CreateInstance = function (Ctor, ...args) {
    return new Ctor(args);
    /*
         switch (args.length) {
            case 0: return new Ctor();
            case 1: return new Ctor(args[0]);
            case 2: return new Ctor(args[0], args[1]);
            case 3: return new Ctor(args[0], args[1], args[2]);
            case 4: return new Ctor(args[0], args[1], args[2], args[3]);
            case 5: return new Ctor(args[0], args[1], args[2], args[3], args[4]);
            case 6: return new Ctor(args[0], args[1], args[2], args[3], args[4], args[5]);
            case 7: return new Ctor(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
            case 8: return new Ctor(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
    
            default:
                throw 'Unsupported number of Constructor arguments';
        }
     */

};
//#endregion

//#region Reflection

/** Contains information about a property or function 
 @class
 */
tp.PropertyInfo = function () {
    this.Name = '';
    this.Signature = '';
    this.Type = '';
    this.Args = 0;
    this.HasGetter = false;
    this.HasSetter = false;
    this.IsConstructor = false;
    this.IsFunction = false;
    this.IsProperty = false;
    this.IsConfigurable = false;
    this.IsEnumerable = false;
    this.IsWritable = false;
    this.Pointer = null;
};


/**
 * Returns the property descriptor of a specified property, if any, else null.  
 Can be used also for calling inherited property getters/setters. 
 @example
// Here is how to call a base property
// NOTE: In both of the following examples, base is the base prototype
 
return tp.GetPropertyDescriptor(base, 'Name').get.call(this);      // getter call
tp.GetPropertyDescriptor(base, 'Name').set.call(this, v);          // setter call
 
 @param {object} o An object (maybe a prototype object)
 @param {string} PropName The name of the property.
 @returns {PropertyDescriptor} Returns the property descriptor or null.
 */
tp.GetPropertyDescriptor = function (o, PropName) {
    if (o !== null) {
        return o.hasOwnProperty(PropName) ?
            Object.getOwnPropertyDescriptor(o, PropName) :
            tp.GetPropertyDescriptor(Object.getPrototypeOf(o), PropName);
    }

    return null;
};
/** Returns information about a property or function
@param {object} o - The container object
@param {string} Key - The name of the member
@returns {tp.PropertyInfo} Returns an information object.
*/
tp.GetPropertyInfo = function (o, Key) {
    var PD = tp.GetPropertyDescriptor(o, Key);

    var Result = new tp.PropertyInfo();
    Result.Name = Key;
    Result.Signature = Key;


    if (PD) {
        var Pointer = o[Key];
        var ParamList;

        Result.Name = Key;
        if (tp.IsFunction(Pointer)) {
            Result.Type = 'f';
            Result.IsFunction = true;
            Result.Args = Pointer.length || 0;
            ParamList = Result.Args > 0 ? tp.GetFunctionParams(Pointer) : [];
            Result.Signature = 'function ' + Key + '(' + ParamList.join(',') + ')';

        } else if (tp.IsArray(Pointer)) {
            Result.Type = 'a';
        } else {
            Result.Type = 'o';
        }

        Result.HasGetter = Boolean(PD.get);
        Result.HasSetter = Boolean(PD.set);
        Result.IsConstructor = tp.IsSameText('constructor', Key);
        Result.IsProperty = !Result.IsFunction && !Result.IsConstructor;
        Result.IsConfigurable = PD.configurable;
        Result.IsEnumerable = PD.enumerable;
        Result.IsWritable = PD.writable || 'set' in PD;
        Result.Pointer = Pointer;
    }

    return Result;
};
/** Returns true if a specified property is writable (provides a setter and is not read-only)
@param {object} o - The container object
@param {string} Key - The name of the member
@returns {tp.PropertyInfo} Returns true if a specified property is writable (provides a setter and is not read-only)
*/
tp.IsWritableProperty = function (o, Key) {
    let PI = tp.GetPropertyInfo(o, Key);
    return PI.HasSetter || PI.IsWritable;
};
/** Returns information about the properties and functions of a object
@param {object} o - The container object
@returns {tp.PropertyInfo[]} Returns a list of information objects.
*/
tp.GetPropertyInfoList = function (o) {
    var A = [];

    if (o) {
        var P;
        for (var Prop in o) {
            P = tp.GetPropertyInfo(o, Prop);
            if (P)
                A.push(P);
        }
    }

    return A;
};
/** Returns a descriptive text of an object
@param {object} o - The container object
@returns {string} Returns a descriptive text of an object
*/
tp.GetReflectionText = function (o) {

    var A = tp.GetPropertyInfoList(o);

    var S;
    var f = '{0} {1} {2} {3} {4} {5} {6} {7} {8}';
    var SB = new tp.StringBuilder();

    A.forEach(function (P) {
        S = tp.Format(f,
            P.Type,
            P.Args,
            P.IsConstructor ? 'c' : '_',
            P.HasGetter ? 'g' : '_',
            P.HasSetter ? 's' : '_',

            P.IsConfigurable ? 'c' : '_',
            P.IsEnumerable ? 'e' : '_',
            P.IsWritable ? 'w' : '_',
            P.Signature
        );

        SB.AppendLine(S);
    });

    S = SB.ToString();

    return S;
};
/** Returns the definition text of an object, that is the signatures of properties and functions. 
@param {object} o - The container object
@returns {string} Returns the definition text of an object, that is the signatures of properties and functions.
*/
tp.GetObjectDefText = function (o) {
    var A = tp.GetPropertyInfoList(o);

    var Constr = '';
    var Props = [];
    var Funcs = [];

    var i, ln;
    for (i = 0, ln = A.length; i < ln; i++) {
        if (A[i].IsFunction) {
            Funcs.push(A[i].Signature);
        } else if (A[i].IsConstructor) {
            Constr = A[i].Signature;
        } else {
            Props.push(A[i].Signature);
        }
    }

    Props.sort();
    Funcs.sort();

    var SB = new tp.StringBuilder();

    if (Constr !== '') {
        SB.AppendLine('constructor ' + Constr);
    }

    if (Props.length > 0) {
        SB.AppendLine();
        SB.AppendLine('// properties');
        for (i = 0, ln = Props.length; i < ln; i++) {
            SB.AppendLine(Props[i]);
        }
    }

    if (Funcs.length > 0) {
        SB.AppendLine();
        SB.AppendLine('// methods');
        for (i = 0, ln = Funcs.length; i < ln; i++) {
            SB.AppendLine(Funcs[i]);
        }
    }

    return SB.ToString();

};
/** Returns an array of the parameter names of any function passed in. 
@param {function} func The function to operate on
@returns {string[]} Returns an array of the parameter names of any function passed in.
*/
tp.GetFunctionParams = function (func) {
    // http://stackoverflow.com/questions/1007981/how-to-get-function-parameter-names-values-dynamically-from-javascript
    return (func + '').replace(/\s+/g, '')
        .replace('/[/][*][^/*]*[*][/]/g', '')           // strip simple comments  
        .split('){', 1)[0].replace(/^[^(]*[(]/, '')   // extract the parameters  
        .replace('/=[^,]+/g', '')                       // strip any ES6 defaults  
        .split(',').filter(Boolean);                  // split & filter [""]  
};
/**
 * Returns true if a specified object has a specified property (not function).
@param {object} o - The container object
@param {string} Key - The name of the member
@returns {boolean} Returns true if a specified object has a specified property (not function).
 */
tp.HasProperty = function (o, Key) {
    let PI = tp.GetPropertyInfo(o, Key);
    return PI.IsProperty == true && PI.HasSetter === true;
};
/**
 * Returns true if a specified object has a specified writable property (not function).
@param {object} o - The container object
@param {string} Key - The name of the member
@returns {boolean} Returns true if a specified object has a specified writable property (not function).
 */
tp.HasWritableProperty = function (o, Key) {
    let PI = tp.GetPropertyInfo(o, Key);
    return PI.IsProperty == true && (PI.IsWritable === true || PI.HasSetter === true);
};
//#endregion

//#region Merging objects

/**
Merges properties and functions of objects in the Sources array to the Dest object. 
CAUTION: No overload. All argument must have values. 
@param {object} Dest - The destination object. It is returned as the Result of the function.
@param {object|any[]} Sources - The source object or an array of source objects (or arrays)
@param {boolean} [DeepMerge=true] - When DeepMerge is true, then source properties that are objects and arrays, are deeply copied to Dest. If false then only their referencies are copied to Dest.
@param {boolean} [PropsOnly=false] -  When PropsOnly is true, source functions are not copied to Dest.
@returns {object|any[]} - Returns the Dest object.
*/
tp.Merge = function (Dest, Sources, DeepMerge = true, PropsOnly = false) {
    var Flag = tp.IsObject(Dest) || tp.IsArray(Dest);

    if (!Flag) {
        return Dest;
    }

    if (!tp.IsArray(Sources)) {
        let x = Sources;
        Sources = [];
        Sources.push(x);
    }

    var Source;
    var SourceProp;
    var DestProp;
    var Copy;

    for (var i = 0, ln = Sources.length; i < ln; i++) {
        Source = Sources[i];
        if (!tp.IsEmpty(Source) && Dest !== Source) {
            for (var PropName in Source) {
                SourceProp = Source[PropName];

                if (tp.IsFunction(SourceProp) && PropsOnly)
                    continue;

                if (tp.IsSimple(SourceProp)) {
                    Dest[PropName] = SourceProp;
                } else if (SourceProp && SourceProp !== Dest) {
                    if (!DeepMerge) {
                        Dest[PropName] = SourceProp;
                    } else {
                        DestProp = Dest[PropName];
                        if (tp.IsObject(SourceProp)) {
                            DestProp = DestProp && tp.IsPlainObject(DestProp) ? DestProp : {};
                            Dest[PropName] = tp.Merge(DestProp, [SourceProp], DeepMerge, PropsOnly);
                        } else if (tp.IsArray(SourceProp)) {
                            DestProp = DestProp && tp.IsArray(DestProp) ? DestProp : [];
                            Dest[PropName] = tp.Merge(DestProp, [SourceProp], DeepMerge, PropsOnly);
                        } else {
                            if (PropName !== 'constructor') {
                                Dest[PropName] = SourceProp;
                            }
                        }
                    }
                }
            }
        }
    }

    return Dest;
};
/**
Merges properties ONLY of objects in the Sources array to the Dest object. 
CAUTION: No overload. All argument must have values. 
@param {object} Dest - The destination object. It is returned as the Result of the function.
@param {object|any[]} Sources - The source object or an array of source objects (or arrays)
@param {boolean} [DeepMerge=true] - When DeepMerge is true, then source properties that are objects and arrays, are deeply copied to Dest. If false then only their referencies are copied to Dest.
@returns {object|any[]} - Returns the Dest object.
*/
tp.MergeProps = function (Dest, Sources, DeepMerge = true) {
    return tp.Merge(Dest, Sources, DeepMerge, true);
};
/**
Merges properties ONLY of objects in the Sources array to the Dest object.  
It does a deep merge, that is source properties that are objects and arrays, are deeply copied to Dest.
@param {object} Dest - The destination object. It is returned as the Result of the function.
@param {object|any[]} Sources - The source object or an array of source objects (or arrays)
@returns {object|any[]} - Returns the Dest object.
*/
tp.MergePropsDeep = function (Dest, Sources) {
    return tp.Merge(Dest, Sources, true, true);
};
/**
Merges OWN properties ONLY of the Source object to the Dest object. Return the Dest.
@param {object} Dest - The destination object. It is returned as the Result of the function.
@param {object} Source - The source object or an array of source objects (or arrays)
@returns {object} - Returns the Dest object.
*/
tp.MergeQuick = function (Dest, Source) {
    if (!tp.IsEmpty(Dest) && !tp.IsEmpty(Source)) {
        for (var i in Source) {
            if (Source.hasOwnProperty(i) && !tp.IsFunction(Source[i])) {
                Dest[i] = Source[i];
            }
        }
    }
    return Dest;
};

//#endregion

//#region Format, FormatNumber, FormatDateTime

/**
Formats a string the C# way <br />
Number and Date values should be passed as already formatted strings.
@example
var S = tp.Format('String: {0}, Number: {1}, Boolean: {2}', 'tripous', 789, true);
@param {string} s - The format string  
@param {any[]} ...values - The values for the format string. Number and Date values should be passed as already formatted strings. 
@returns {string} Returns the formatted string.
*/
tp.Format = function (s, ...values) {
    if (tp.IsString(s)) {
        let i, ln, Params = [];
        for (i = 1, ln = arguments.length; i < ln; i++) {
            Params.push(arguments[i]);
        }

        for (i = 0; i < Params.length; i++) {
            let regEx = new RegExp("\\{" + (i) + "\\}", "gm");
            s = s.replace(regEx, Params[i]);
        }
    }

    return s;
};
/** 
 * Formats a number into a string the C# way. <br /> 
 * It uses two types of formats: Standard and Custom. <br />
   <pre>
    ------------------------------------------------------------------------------------------                      
                                    Standard Formats                                                                
    ------------------------------------------------------------------------------------------
    Char   Name        Numbers         Groups      Decimals    Examples
    ------------------------------------------------------------------------------------------
    D      Decimal     integers only   no          no          1234 ("D") -> 1234,  -1234 ("D6") -> -001234
    C      Currency    float numbers   yes         yes         123.456 ("C", en-US) -> $123.46
    F      Fixed       float numbers   no          yes         -1234.56 ("F4", en-US) -> -1234.5600
    N      Number      float numbers   yes         yes         -1234.56 ("N3", en-US) -> -1,234.560

    See: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-string
    ------------------------------------------------------------------------------------------
                                    Custom Formats
    ------------------------------------------------------------------------------------------
    0	    Zero	    Replaces the zero with the corresponding digit if one is present; otherwise, zero appears in the result string
    #	    Digit	    Replaces the "#" symbol with the corresponding digit if one is present; otherwise, no digit appears in the result string.
    .	    Decimal	    Determines the location of the decimal separator in the result string
    ,	    Group	    Inserts a localized group separator character between each group.

    Examples
    0     1234.5678 ("00000") -> 01235    0.45678 ("0.00", en-US) -> 0.46
    #     1234.5678 ("#####") -> 1235     0.45678 ("#.##", en-US) -> 0.46
    .     0.45678 ("0.00", en-US) -> 0.46
    ,     2147483647 ("##,#", en-US) -> 2,147,483,647

    See: https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings
    ------------------------------------------------------------------------------------------
  </pre>
 * @param {number} v The number to be formatted as string.
 * @param {string} format The format to use. 
 * @returns {string} Returns the formatted string.
 */
tp.FormatNumber = (v, format) => {
    if (typeof v !== 'number')
        return '';

    let o = {
        style: 'decimal'
    };

    format = format.toUpperCase();

    // standard formats
    if (['C', 'D', 'F', 'N'].indexOf(format[0]) !== -1) {

        let Decimals = format.length > 1 ? format.substring(1) : '0';
        Decimals = parseInt(Decimals, 10);

        // currency
        if (format[0] === 'C') {
            let Culture = tp.Cultures.Find(tp.CultureCode);
            Decimals = Decimals === 0 ? Culture.CurrencyDecimals : Decimals;

            o.style = 'currency';
            o.useGrouping = true;
            o.currency = Culture.CurrencyCode;
            o.currencyDisplay = 'symbol';
        }
        else {  // all others
            if (format[0] === 'D') {
                o.minimumIntegerDigits = Decimals > 0 ? Decimals : 1;
                Decimals = 0;
            } else {
                o.minimumIntegerDigits = 1;
                Decimals = Decimals > 0 ? Decimals : 2;
            }

            o.useGrouping = format[0] === 'N';
            o.minimumFractionDigits = Decimals;
            o.maximumFractionDigits = Decimals;
        }
    }
    else { // custom formats

        let Parts = format.split('.');
        let sFormatIntPart = Parts[0];
        let sFormatDecPart = Parts.length > 1 ? Parts[1] : '';
        let UseGroups = sFormatIntPart.indexOf(',') > -1;

        if (UseGroups) {
            sFormatIntPart = sFormatIntPart.replace(',', '');
        }

        o.useGrouping = UseGroups;                  // .replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        o.minimumIntegerDigits = sFormatIntPart.startsWith('0') ? sFormatIntPart.length : 1;
        o.minimumFractionDigits = sFormatDecPart.length;
        o.maximumFractionDigits = sFormatDecPart.length;
    }

    let Result = v.toLocaleString(tp.CultureCode, o);
    return Result;

};
/** Formats a number into a string.
 Decimals is the number of decimal places into string. 
 If DecimalSep or ThousandSep are not defined, the corresponging tripous globals are used.
@param {number} v - The number to be formatted as string.
@param {number} Decimals - The number of decimal places into string.
@param {character} [DecimalSep] - The decimal separator to use
@param {character} [ThousandSep] - The thousand separator to use
@returns {string} Returns the formatted string.
*/
tp.FormatNumber2 = function (v, Decimals = 0, DecimalSep = null, ThousandSep = null) {
    if (isNaN(v) || !tp.IsValid(v))
        return '';

    DecimalSep = DecimalSep || tp.DecimalSeparator;
    ThousandSep = ThousandSep || tp.ThousandSeparator;

    var S = v.toFixed(~~Decimals);

    var Parts = S.split('.');
    var NumPart = Parts[0];
    var DecPart = Parts[1] ? DecimalSep + Parts[1] : '';

    return NumPart.replace(/(\d)(?=(?:\d{3})+$)/g, '$1' + ThousandSep) + DecPart;
};
/**
Formats a Date value based on a specified format string pattern. <br />
If no format is specified the current culture date format is used.  <br />
Adapted from: https://github.com/UziTech/js-date-format/blob/master/js-date-format.js
@param {Date} v - The Date value to format.  
@param {string} [format] - Optional. The format string pattern. If not specified the current culture date format is used.
@returns {string} Returns the formatted string
*/
tp.FormatDateTime = function (v, format = null) {
    // adapted from: https://github.com/UziTech/js-date-format/blob/master/js-date-format.js

    format = format || tp.GetDateFormat();  

    let Pad = function (value, length) {
        var negative = value < 0 ? "-" : "";
        var zeros = "0";
        for (var i = 2; i < length; i++) {
            zeros += "0";
        }
        return negative + (zeros + Math.abs(value).toString()).slice(-length);
    };

    let Parts = {
        date: v,
        yyyy: function () { return Parts.date.getFullYear(); },
        yy: function () { return Parts.date.getFullYear() % 100; },
        MM: function () { return Pad(Parts.date.getMonth() + 1, 2); },
        M: function () { return Parts.date.getMonth() + 1; },
        dd: function () { return Pad(Parts.date.getDate(), 2); },
        d: function () { return Parts.date.getDate(); },
        HH: function () { return Pad(Parts.date.getHours(), 2); },
        H: function () { return Parts.date.getHours(); },
        hh: function () {
            var hour = Parts.date.getHours();
            if (hour > 12) {
                hour -= 12;
            } else if (hour < 1) {
                hour = 12;
            }
            return Pad(hour, 2);
        },
        h: function () {
            var hour = Parts.date.getHours();
            if (hour > 12) {
                hour -= 12;
            } else if (hour < 1) {
                hour = 12;
            }
            return hour;
        },
        mm: function () { return Pad(Parts.date.getMinutes(), 2); },
        m: function () { return Parts.date.getMinutes(); },
        ss: function () { return Pad(Parts.date.getSeconds(), 2); },
        s: function () { return Parts.date.getSeconds(); },
        fff: function () { return Pad(Parts.date.getMilliseconds(), 3); },
        ff: function () { return Pad(Math.floor(Parts.date.getMilliseconds() / 10), 2); },
        f: function () { return Math.floor(Parts.date.getMilliseconds() / 100); },
        zzzz: function () { return Pad(Math.floor(-Parts.date.getTimezoneOffset() / 60), 2) + ":" + Pad(-Parts.date.getTimezoneOffset() % 60, 2); },
        zzz: function () { return Math.floor(-Parts.date.getTimezoneOffset() / 60) + ":" + Pad(-Parts.date.getTimezoneOffset() % 60, 2); },
        zz: function () { return Pad(Math.floor(-Parts.date.getTimezoneOffset() / 60), 2); },
        z: function () { return Math.floor(-Parts.date.getTimezoneOffset() / 60); }
    };


    let Result = [];
    let IsMatch = false;
    let FormatPart;
    let ResultPart;

    while (format.length > 0) {

        IsMatch = false;
        FormatPart;
        for (var i = format.length; i > 0; i--) {
            FormatPart = format.substring(0, i);
            if (FormatPart in Parts) {
                ResultPart = Parts[FormatPart]();
                Result.push(ResultPart);
                format = format.substring(i);
                IsMatch = true;
                break;
            }
        }

        if (!IsMatch) {
            Result.push(format[0]);
            format = format.substring(1);
        }
    }

    return Result.join("");
};
//#endregion

//#region Strings



/**
 Returns true if a specified string is null, undefined or it just contains white space chars (space, tab, etc). <br />
 Throws an exception if the specified value is other than undefined, null or string.
@param {string} v - A string value. 
@returns {boolean}  Returns true if the string is null, undefined or it just contains white space chars (space, tab, etc)
*/
tp.IsBlank = function (v) {
    if (v === void 0 || v === null)
        return true;

    if (!tp.IsString(v)) {
        tp.Throw('Can not check for null or whitespace a non-string value');        
    }

    return v.trim().length === 0; //v.replace(/\s/g, '').length < 1;
};
/**
 Returns true if a specified string is null, undefined or it just contains white space chars
@param {string} v - A string value.
@returns {boolean}  Returns true if the string is null, undefined or it just contains white space chars
*/
tp.IsNullOrWhitespace = function (v) { return tp.IsBlank(v); };
/**
 * Returns true if a specified string is null, undefined or it just contains white space chars (space, tab, etc). <br />
 * No exception is thrown if the specified value is other than undefined, null or string.
 * @param {string} v - A string value.
 * @returns {boolean}  Returns true if the string is null, undefined or it just contains white space chars (space, tab, etc)
 */
tp.IsBlankString = function (v) {
    return (v === void 0 || v === null) || (tp.IsString(v) && v.trim().length === 0);
};

/** True if a specified character is a white space char (space, tab, etc)  
@param {character} c - A character value. 
@returns {boolean} Returns True if a specified character is a white space char (space, tab, etc)
*/
tp.IsWhitespaceChar = function (c) { return c.charCodeAt(0) <= 32; }; // return ' \t\n\r\v'.indexOf(c) === 0;
/**
Returns true if a specified text looks like html markup.
@see {@link https://stackoverflow.com/questions/15458876/check-if-a-string-is-html-or-not|stackoverflow}
@param {string} Text - The text to test.
@returns {boolean} Returns true if a specified text looks like html markup.
*/
tp.IsHtml = function (Text) { return /<[a-z][\s\S]*>/i.test(Text); };


/**
 True if two string are of the same text, case-insensitively always
@param {string} A - The first string.
@param {string} B - The second string
@returns {boolean} Returns true if the two strings are case-insensitively identicals.
*/
tp.IsSameText = function (A, B) {
    return tp.IsString(A) && tp.IsString(B) && A.toUpperCase() === B.toUpperCase();
};
/**
 True if a  sub-string is contained by another string
@param {string} Text - The string
@param {string} SubText - The sub-string, the string to search for.
@param {boolean} [CI=true] - CI (Case-Insensitive) can be true (the default) or false 
@returns {boolean} Returns true if a substring is contained in the other string.
*/
tp.ContainsText = function (Text, SubText, CI = true) {
    CI = CI === true;
    if (tp.IsString(Text) && !tp.IsBlank(Text)) {
        return CI ? Text.toLowerCase().includes(SubText.toLowerCase()) : Text.includes(SubText);
    }

    return false;
};
/**
Inserts a sub-string in another string at a specified index and returns the new string.
@param {string} SubString - The sub-string to insert.
@param {string} Text - The string
@param {number} Index - The position at the string where the sub-string should be inserted.
@returns {string} Returns the new string.
*/
tp.InsertText = function (SubString, Text, Index) {
    return [Text.slice(0, Index), SubString, Text.slice(Index)].join('');
};

/** FROM: https://vanillajstoolkit.com/polyfills/stringtrim/ */
if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, '');
    };
}
/** FROM: https://vanillajstoolkit.com/polyfills/stringtrimstart/ */
if (!String.prototype.trimStart) {
    String.prototype.trimStart = function () {
        return this.replace(new RegExp('^' + /[\x09\x0A\x0B\x0C\x0D\x20\xA0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u202F\u205F\u3000\u2028\u2029\uFEFF]+/.source, 'g'), '');
    };
}
/** FROM: https://vanillajstoolkit.com/polyfills/stringtrimend/ */
if (!String.prototype.trimEnd) {
    String.prototype.trimEnd = function () {
        return this.replace(new RegExp(/[\x09\x0A\x0B\x0C\x0D\x20\xA0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u202F\u205F\u3000\u2028\u2029\uFEFF]+/.source + '$', 'g'), '');
    };
}

/**
Trims a string (removes blank characters from start and end) and returns the new string.
@param {string} v - The string .
@returns {string} Returns the new string.
*/
tp.Trim = function (v) {
    return tp.IsBlank(v) ? "" : v.trim(); //v.replace(/^\s+|\s+$/g, "");
};
/**
Trims a string by removing blank characters from the start of the string and returns the new string.
@param {string} v - The string .
@returns {string} Returns the new string.
*/
tp.TrimStart = function (v) {
    return tp.IsBlank(v) ? "" : v.trimStart(); //v.replace(/^\s+/, "");
};
/**
Trims a string by removing blank characters from the end of the string and returns the new string.
@param {string} v - The string .
@returns {string}  Returns the new string.
*/
tp.TrimEnd = function (v) {
    return tp.IsBlank(v) ? "" : v.trimEnd(); //v.replace(/\s+$/, "");
};

/**
  True if a string starts with a sub-string.
 @param {string} Text - The string to check.
 @param {string} SubText - The sub-string, the string to search for.
 @param {boolean} [CI=true] - CI (Case-Insensitive) can be true (the default) or false 
 @returns {boolean} Returns true if a string starts with a sub-string.
 */
tp.StartsWith = function (Text, SubText, CI = true) {
    if (tp.IsBlank(SubText) || tp.IsBlank(Text))
        return false;

    if (tp.IsEmpty(CI)) {
        CI = true;
    }   

    let S = Text.substring(0, SubText.length);

    return CI === true ? S.toUpperCase() === SubText.toUpperCase() : S === SubText;

};
/**
 True if a string ends with a sub-string.
@param {string} Text - The string
@param {string} SubText - The sub-string, the string to search for.
@param {boolean} [CI=true] - CI (Case-Insensitive) can be true (the default) or false
@returns {boolean} Returns true if a string ends with a sub-string.
*/
tp.EndsWith = function (Text, SubText, CI = true) {
    if (tp.IsBlank(SubText) || tp.IsBlank(Text))
        return false;

    if (tp.IsEmpty(CI)) {
        CI = true;
    }

    let S = Text.substring(Text.length - SubText.length, Text.length);

    return CI === true ? S.toUpperCase() === SubText.toUpperCase() : S === SubText;
};
/**
Replaces a sub-string by another sub-string, inside a string, and returns the new string.
@param {string} v - The string .
@param {string} OldValue - The old string .
@param {string} NewValue - The new string .
@returns {string} Returns the new string.
*/
tp.Replace = function (v, OldValue, NewValue) {
    return v.replace(OldValue, NewValue);
};
/**
 Replaces all occurences of a sub-string by another sub-string, inside a string, and returns the new string.
@param {string} v - The string .
@param {string} OldValue - The old string.
@param {string} NewValue - The new string.
@param {boolean} [CI=true] - CI (Case-Insensitive) can be true (the default) or false 
@returns {string} Returns the string after the replacement.
*/
tp.ReplaceAll = function (v, OldValue, NewValue, CI = true) {
    OldValue = tp.RegExEscape(OldValue);
    var Flags = CI === true ? 'igm' : 'gm';
    return v.replace(new RegExp(OldValue, Flags), NewValue);
};
/**
Replaces a character found at a specified index inside a string, and returns the new string.
@param {string} v - The string .
@param {character} NewChar - The character that replaces the old character.
@param {number} Index - The index of the character to be replaced
@returns {string} Returns the string after the replacement.
*/
tp.ReplaceCharAt = function (v, NewChar, Index) {
    return [v.slice(0, Index), NewChar, v.slice(Index + 1)].join('');
};

/**
Places single or double quotes around a string (defaults to single quotes), and returns the new string.
@param {string} v - The string .
@param {boolean} [DoubleQuotes=true] When true then double quotes are used.
@returns {string} Returns the result string
*/
tp.Quote = function (v, DoubleQuotes = true) {
    DoubleQuotes = DoubleQuotes === true;

    if (tp.IsValid(v)) {
        if (DoubleQuotes) {
            v = v.replace(/"/gm, '\\"');
            v = '"' + v + '"';
        } else {
            v = v.replace(/'/gm, "\\'");
            v = "'" + v + "'";
        }
    }

    return v;
};
/**
Unquotes a string if it is surrounded by single or double quotes, and returns the new string.
@param {string} v - The string .
@returns {string} Returns the result string
*/
tp.Unquote = function (v) {
    if (tp.IsValid(v)) {
        if (v.charAt(0) === '"') {
            return v.replace(/(^")|("$)/g, '');
        } else if (v.charAt(0) === "'") {
            return v.replace(/(^')|('$)/g, '');
        }
    }

    return v;
};
/**
Trims a specified string and if the last character is the comma character it removes it. Returns the new string.
@param {string} v - The string
@returns {string} Returns the new string.
*/
tp.RemoveLastComma = function (v) {
    if (tp.IsBlank(v))
        return '';
    else {
        v = tp.Trim(v);
        if (v.length > 0 && tp.EndsWith(v, ','))
            v = v.substring(0, v.length - 1);

        return v;
    }
};
/**
Splits a string into chunks according to a specified chunk size and returns an array of strings.
@param {string} v - The string .
@param {number} ChunkSize - The size in characters for each chunk.
@returns {string[]} Returns Array of strings.
*/
tp.Chunk = function (v, ChunkSize) {
    var rg = new RegExp('.{1,' + ChunkSize + '}', 'g');
    var A = v.match(rg);
    return A;
};
/**
Splits a string into an array of strings by separating the string into and an array of substrings.  
The separator is treated as a string or a regular expression. 
If separator is omitted or does not occur in string, the array returned contains one element consisting of the entire string.  
If separator is an empty string, then the string is converted to an array of characters.
@param   {string}  v - The string to operate on
@param   {string} [Separator=' '] - Optional. Specifies the character(s) to use for separating the string.
@param   {boolean} [RemoveEmptyEntries=true] Optional. When true, the default, then empty entries are removed from result.
@returns  {string[]}  Returns an array of strings.
*/
tp.Split = function (v, Separator = ' ', RemoveEmptyEntries = true) {
    v = v || '';
    RemoveEmptyEntries = RemoveEmptyEntries === true;

    if (RemoveEmptyEntries) {
        var Parts = v.split(Separator);
        var A = [];
        for (var i = 0, ln = Parts.length; i < ln; i++) {
            if (!tp.IsBlank(Parts[i]))
                A.push(Parts[i]);
        }
        return A;
    } else {
        return v.split(Separator);
    }

};
/**
 * Splits a string like "ThisIsAString" into a string like "This Is A String".
 * @param {string} v The string to split.
 */
tp.SplitOnUpperCase = function (v) {
    let Result = '';
    if (tp.IsString(v)) {
        Result = v.match(/[A-Z][a-z]+/g).join(' ');
    }
    return Result;
};
/**
Splits a string of a certain format and creates and returns a javascript object.  
The input string MUST have the following format:
  Key0:Value0; Key1:Value1; KeyN:ValueN; 
Key is a string (with single or double quotes, or no quotes at all).
Value could be any value, or string (with single or double quotes, or no quotes at all).
@param  {string} v - The string to operate on
@returns {object} Returns the constructed object.
 */
tp.SplitDescriptor = function (v) {

    var Result = {};

    if (tp.IsString(v)) {
        var Lines = tp.Split(v, ";", true);
        var parts;

        var Key;
        var Value;

        if (Lines) {
            for (var i = 0; i < Lines.length; i++) {
                parts = tp.Split(Lines[i], ":");
                if (parts && parts.length === 2) {
                    Key = tp.Unquote(Trim(parts[0]));
                    Value = tp.Unquote(Trim(parts[1]));

                    if (Key.length && Value.length) {
                        Result[Key] = Value;
                    }
                }
            }
        }
    }

    return Result;
};
/**
Returns a united string by joining a list of values using an optional separator.  
@param {string} Separator - The separator to use.
@param  {...any} values - The values to join
@returns {string} Returns the result string.
 */
tp.Join = function (Separator, ...values) {

    Separator = Separator || '';

    var i, ln, Params = [];
    for (i = 1, ln = arguments.length; i < ln; i++) {
        Params.push(arguments[i]);
    }

    return Params.join(Separator);
};
/**
Returns a united string by joining a list of values using as separator a comma and a space. 
@param {...any} values The values to join
@returns {string} Returns the result string.
 */
tp.CommaText = function (...values) {

    var i, ln, Params = [];
    for (i = 0, ln = arguments.length; i < ln; i++) {
        Params.push(arguments[i]);
    }
    return Params.join(',  ');
};
/**
Returns an array of strings, by splitting a string, considering the line breaks (\n or \r\n) as separator 
@param  {string} v The string to operate on.
@returns {string[]} Returns an array of strings
 */
tp.ToLines = function (v) {
    if (!tp.IsBlank(v)) {
        var SEP = '##__##';
        v = tp.ReplaceAll(v, '\r\n', SEP);
        v = tp.ReplaceAll(v, '\n', SEP);
        return v.split(SEP);
    }
    return [];
};
/**
Returns a html string, by replacing line breaks (\n or \r\n) with <code>&lt;br /&gt;</code> elements. 
@param  {string} v The string to operate on.
@returns {string} Returns the new string.
 */
tp.LineBreaksToHtml = function (v) {
    if (!tp.IsBlank(v)) {
        var SEP = '##__##';
        v = tp.ReplaceAll(v, '\r\n', SEP);
        v = tp.ReplaceAll(v, '\n', SEP);
        v = tp.ReplaceAll(v, SEP, '');
    }

    return v;
};
tp.ReplaceLineBreaks = function (v, sep) {
    if (!tp.IsBlank(v)) {
        v = tp.ReplaceAll(v, '\r\n', sep);
        v = tp.ReplaceAll(v, '\r', sep);
        v = tp.ReplaceAll(v, '\n', sep);
    }
    return v;
};
/**
Creates and returns a string by repeating a string value a certain number of times
@param  {string} v - The string to repeat.
@param {number} Count - How many times to repeat the input string
@returns {string} Returns a string by repeating a string value a certain number of times
 */
tp.Repeat = function (v, Count) {
    var Result = "";

    for (var i = 0; i < Count; i++) {
        Result += v;
    }
    return Result;
};
/**
Pads a string from left side (start) with a specified sub-string until a specified total length
@param  {string} v - The string to operate on.
@param  {string} PadText - The string to be used in padding.
@param {number} TotalLength - The desired total length of the result string
@returns {string} Returns the padded string.  
 */
tp.PadLeft = function (v, PadText, TotalLength) {
    if (!tp.IsValid(v))
        return v;

    TotalLength = ~~TotalLength;
    v = String(v);
    if (v.length < TotalLength) {
        PadText = tp.Repeat(PadText, TotalLength - v.length);
        v = PadText + v;
    }

    return v;
};
/**
Pads a string from right side (end) with a specified sub-string until a specified total length
@param  {string} v - The string to operate on.
@param  {string} PadText - The string to be used in padding.
@param {number} TotalLength - The desired total length of the result string
@returns {string} Returns the padded string.
 */
tp.PadRight = function (v, PadText, TotalLength) {
    if (!tp.IsValid(v))
        return v;

    TotalLength = ~~TotalLength;
    v = String(v);
    if (v.length < TotalLength) {
        PadText = tp.Repeat(PadText, TotalLength - v.length);
        v = v + PadText;
    }

    return v;
};
/**
Truncates a string to a specified length if string's length is greater than the specified length. Returns the truncated string.
@param  {string} v - The string to operate on.
@param {number} NewLength - The length of the result string
@returns {string} Returns the new string.
 */
tp.SetLength = function SetLength(v, NewLength) {
    if (tp.IsBlank(v))
        return "";

    v = String(v);
    NewLength = ~~NewLength;
    if (v.length > NewLength) {
        v = v.slice(0, NewLength);
    }
    return v;
};
/**
Converts a string to an integer and returns that integer.
@param  {string} v - The string to operate on.
@param {number} [Default=0] - The default value to return if the convertion is not possible. Defaults to 0.
@returns {number} Returns the number.
 */
tp.StrToInt = function (v, Default = 0) {
    if (tp.IsBlank(v) || isNaN(v))
        return tp.IsEmpty(Default) ? 0 : Default;

    var n = parseInt(v, 10);

    if (isNaN(n) || !isFinite(n)) {
        return tp.IsEmpty(Default) ? 0 : Default;
    }
    return n;
};
/**
Converts a string to a float and returns that float.
@param  {string} v - The string to operate on.
@param {number} [Default=0] - The default value to return if the convertion is not possible. Defaults to 0.
@returns {number} Returns the number.
 */
tp.StrToFloat = function (v, Default = 0) {
    if (tp.IsBlank(v))
        return tp.IsEmpty(Default) ? 0 : Default;
    var n = parseFloat(v);
    if (isNaN(n) || !isFinite(n)) {
        return tp.IsEmpty(Default) ? 0 : Default;
    }
    return n;
};
/**
Converts a string to a boolean and returns that boolean.  
The input string must be either 'true' or 'false' regardless of case-sensitivity. 
@param  {string} v - The string to operate on.
@param {boolean} [Default=false] - The default value to return if the convertion is not possible. Defaults to false.
@returns {boolean} Returns the boolean value.
 */
tp.StrToBool = function (v, Default = false) {
    Default = Default === true;

    if (tp.IsSameText(v, "true") || tp.IsSameText(v, "yes")) {
        return true;
    } else if (tp.IsSameText(v, "false") || tp.IsSameText(v, "no")) {
        return false;
    }

    return Default;
};

/** Used as the return value by number convertion functions 
 */
tp.NumberConvertionResult = class {

    /**
     * Constructor
     * @param {number} Value The value after the convertion
     * @param {boolean} Result Result of the convertion 
     */
    constructor(Value, Result) {
        this.Value = Value;
        this.Result = Result;
    }

};
/** The value after the convertion
 @type {number}
 */
tp.NumberConvertionResult.prototype.Value;
/** Result of the convertion
 @type {boolean}
 */
tp.NumberConvertionResult.prototype.Result;

/**
Tries to convert a string into an integer.   <br />
Returns a {@link tp.NumberConvertionResult} object as <code>{Value: v, Result: true}</code> where Value is the convertion result, if successful, and Result indicates success or failure.
@param  {string} v - The string to operate on.
@returns {tp.NumberConvertionResult} Returns an {@link tp.NumberConvertionResult} object as <code> {Value: v, Result: true}</code>
 */
tp.TryStrToInt = function (v) {
    var n = parseInt(v, 10);
    return new tp.NumberConvertionResult(n, isNaN(n) ? false : true);
};
/**
Tries to convert a string into a float.  <br />
NOTE: The decimal separator could be point or comma. <br />
Returns a {@link tp.NumberConvertionResult} object as <code>{Value: v, Result: true}</code>  where Value is the convertion result, if successful, and Result indicates success or failure.
@param  {string} v - The string to operate on.
@returns {tp.NumberConvertionResult} Returns a {@link tp.NumberConvertionResult} object as <code> {Value: v, Result: true}</code>
 */
tp.TryStrToFloat = function (v) {
    if (tp.IsString(v)) {
        v = v.replace(',', '.');
    };

    var n = parseFloat(v);
    return new tp.NumberConvertionResult(n, isNaN(n) || !isFinite(n) ? false : true);
}; 


/**
Converts an integer value into a hexadecimal string, and returns that string   
@param  {number} v - The value to operate on.
@returns {string} Returns the hex string.
 */
tp.ToHex = function (v) {
    if (v < 0) {
        v = 0xFFFFFFFF + v + 1; // ensure not a negative number
    }

    var S = v.toString(16).toUpperCase();
    while (S.length % 2 !== 0) {
        S = '0' + S;
    }
    return S;
};
/**
Escapes a string for use in Javascript regex and returns the escaped string   
@param  {string} v - The value to operate on.
@returns {string} Returns the escaped string.
@see {@link https://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex|StackOverflow}
 */
tp.RegExEscape = function (v) {
    //return v.replace(/([.*+?\^=!:${}()|\[\]\/\\])/g, "\\$1");
    return tp.IsBlank(v) ? "" : v.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
};
/**
 Converts a dashed string to camel case, e.g. background-color to backgroundColor and -webkit-user-select to webkitUserSelect and returns the new string  
@param  {string} v - The value to operate on.
@returns {string} Returns the the camel-cased string.
 */
tp.DashToCamelCase = function (v) {
    if (!tp.IsBlank(v)) {
        if (v.length > 1 && v.charAt(0) === '-') {
            v = v.substring(1);
        }

        v = v.replace(/-([\da-z])/gi, function (match, c) {
            return c.toUpperCase();
        });
    }

    return v;
};
/**
 Combines two strings by returning a single url path. Ensures that the required slashes are in place.
@param {string} A The first string.
@param {string} B The second string.
@returns {string} The combination of the two strings.
*/
tp.UrlCombine = function (A, B) {
    if (!tp.EndsWith(A, '/') && !tp.StartsWith(B, '/'))
        A += '/';
    return A + B;
};
/**
 Combines a TableName a dot and a FieldName and returns a string.
@param {string} TableName The table name
@param {string} FieldName The field name
@returns {string} Returns the combined string, e.g. Customer.Name
*/
tp.FieldPath = function (TableName, FieldName) {
    if (!tp.IsBlank(TableName) && !tp.IsBlank(FieldName))
        return TableName + '.' + FieldName;

    return '';
};
/**
 Combines a TableName a double dash (__) and a FieldName and returns a string.
@param {string} TableName The table name
@param {string} FieldName The field name
@returns {string} Returns the combined string, e.g. Customer__Name
*/
tp.FieldAlias = function (TableName, FieldName) {
    if (!tp.IsBlank(TableName) && !tp.IsBlank(FieldName))
        return TableName + '__' + FieldName;

    return '';
};
/**
Returns a new GUID string.
@param {boolean} [UseBrackets=false] Optional. If true, then ther result string is encloded by brackets. Defaults to false.
@returns {string} Returns a GUID string.
@see {@link https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript|StackOverflow}
*/
tp.Guid = function (UseBrackets = false) {

    var d = new Date().getTime();
    if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
        d += performance.now(); //use high-precision timer if available
    }

    var Result = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16).toUpperCase();
    });

    UseBrackets = UseBrackets || false;
    return !UseBrackets ? Result : "{" + Result + "}";
};
/**
Creates and returns a function from a string
@param {string} v - The function source code
@example
var func = tp.CreateFunction('function (a, b) { return a + b; }');
@returns {function} Returns a function  
@see {@link http://stackoverflow.com/questions/7650071/is-there-a-way-to-create-a-function-from-a-string-with-javascript|StackOverflow}
*/
tp.CreateFunction = function (v) {
    var funcReg = /function *\(([^()]*)\)[ \n\t]*\{(.*)\}/gmi;
    var match = funcReg.exec(v.replace(/\n/g, ' '));

    if (match) {
        var Args = match[1].split(',');
        Args.push(match[2]);
        return Function.apply(null, Args);
    }

    return null;
};

/**
Creates a base-64 encoded ASCII string from a string value.
@param {string} v - The value to operate on
@returns {string} Returns a base-64 string.
@see {@link https://stackoverflow.com/questions/30106476/using-javascripts-atob-to-decode-base64-doesnt-properly-decode-utf-8-strings|StackOverflow}
@see {@link https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/Base64_encoding_and_decoding|MDN}
*/
tp.ToBase64 = function (v) {
    return window.btoa(encodeURIComponent(v).replace(/%([0-9A-F]{2})/g, function (match, p1) {
        return String.fromCharCode(Number('0x' + p1.toString()));
    }));
};
/**
Decodes a string of data which has been encoded using base-64 encoding.
@param {string} v - The value to operate on
@returns {string} Returns the plain string.
@see {@link https://stackoverflow.com/questions/30106476/using-javascripts-atob-to-decode-base64-doesnt-properly-decode-utf-8-strings|StackOverflow}
@see {@link https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/Base64_encoding_and_decoding|MDN}
*/
tp.FromBase64 = function (v) {
    return window.atob(v);
};

//#endregion

//#region Numbers

/**
* Rounds a float number (or a string representation of a float number) to specified decimal places (defaults to 2).
* @param   {number|string} v The float number or the string representation of a float number.
* @param   {number} Decimals Defaults to 2. The decimal places to round to.
  @returns {number} Returns the rounded number
*/
tp.Round = function (v, Decimals = 2) {
    Decimals = Decimals || 2;

    if (typeof v === 'string') {
        v = parseFloat(v);
    }

    return parseFloat(v.toFixed(Decimals));
};
/**
Truncates a float number to an integer
@param {number} v - The number to truncate
@returns {number} Returns an integer.
*/
tp.Truncate = function (v) {
    return v | 0;
};
/** Returns a random integer inside a specified range
@param {number} Min The minimum number in the desirable range
@param {number} Max The maximum number in the desirable range
@returns {number} Returns the random integer
*/
tp.Random = function (Min, Max) {
    return Math.floor(Math.random() * (Max - Min + 1)) + Min;
};
/** Returns a random float number inside a specified range
@param {number} Min The minimum number in the desirable range
@param {number} Max The maximum number in the desirable range
@returns {number} Returns the random float
*/
tp.RandomFloat = function (Min, Max) {
    return Math.random() * (Max - Min + 1) + Min;
};


//#endregion

//#region Dates

/** Enum-like class
 @class
 @enum {number}
 */
tp.Day = {
    Sunday: 0,
    Monday: 1,
    Tuesday: 2,
    Wednesday: 3,
    Thursday: 4,
    Friday: 5,
    Saturday: 6
};
Object.freeze(tp.Day);

/** Enum-like class. Indicates the pattern of a date format string.
 @class
 @enum {number}
 */
tp.DatePattern = {
    /** MM-dd-yyyy. Middle-endian (month, day, year), e.g. 04/22/96
     * @type {number} 
     */
    MDY: 0,
    /** dd-MM-yyyy. Little-endian (day, month, year), e.g. 22.04.96 or 22/04/96
     * @type {number}
     */
    DMY: 1,
    /** yyyy-MM-dd. Big-endian (year, month, day), e.g. 1996-04-22
     * @type {number}
     */
    YMD: 2
};
 



/**
 * Parses a date, time or date-time string into a Date value. The string format should be in yyyy/MM/dd HH:mm:ss  
 Using the / date separator the date is parsed to local date-time.  
 Using the - date separator the date is parsed to UTC date-time. 
 @see {@link http://stackoverflow.com/questions/5619202/converting-string-to-date-in-js|stackoverflow}
 @see {@link https://stackoverflow.com/questions/2587345/why-does-date-parse-give-incorrect-results|stackoverflow}
 * @param   {string} v A date string in the format yyyy/MM/dd for local dates and yyyy-MM-dd for UTC dates.
 * @returns  {Date} A date object
 */
tp.ParseDateTime = function (v) {
    let o = tp.TryParseDateTime(v);
    if (o.Result === true) {
        return o.Value;
    }
    return new Date(Date.parse(v));
};
/**
 * Parses a date, time or date-time string into a Date value. The string format should be in yyyy/MM/dd HH:mm:ss  
 Using the / date separator the date is parsed to local date-time. 
 Using the - date separator the date is parsed to UTC date-time. 
 @see {@link http://stackoverflow.com/questions/5619202/converting-string-to-date-in-js|stackoverflow}
 *
 * @param   {string}   v   A date string in the format yyyy/MM/dd for local dates and yyyy-MM-dd for UTC dates.
 * @returns  {object}   Returns an object as { Value: Date, Result: boolean }
 */
tp.TryParseDateTime = function (v, CultureCode = null) {
    var Info = {
        Value: null,
        Result: false
    };
 
    if (!tp.IsString(CultureCode) || tp.IsBlank(CultureCode))
        CultureCode = tp.CultureCode;

    try {
        if (tp.IsString(v) && !tp.IsBlank(v)) {
            let i, ln;
            let Seps = ['-', '/', '.'];
            v = v.trim();

            let DateFormat = tp.GetDateFormat(CultureCode);
            let DatePattern = tp.GetDatePattern(DateFormat);

            let Sep = tp.GetDateSeparator(CultureCode);
            for (i = 0, ln = Seps.length; i < ln; i++) {
                if (v.indexOf(Seps[i]) !== -1) {
                    Sep = Seps[i];
                    break;
                }
            }

            let Today = tp.Today();
            let Year, Month, Day;

            switch (DatePattern) {
                case tp.DatePattern.DMY:
                    Year = 3;
                    Month = 2;
                    Day = 1;
                    break;
                case tp.DatePattern.MDY:
                    Year = 3;
                    Month = 1;
                    Day = 2;
                    break;
                default:
                    Year = 1;
                    Month = 2;
                    Day = 3;
                    break;
            }

            let Parts = v.split(Sep);

            if (tp.IsArray(Parts) && Parts.length > 0) {
                Year = Parts.length >= Year ? Parts[Year - 1] : Today.getFullYear();
                Month = Parts.length >= Month ? Parts[Month - 1] - 1 : Today.getMonth();
                Day = Parts.length >= Day ? Parts[Day - 1] : tp.DayOfMonth(Today);

                let D = new Date(Year, Month, Day);
                Info.Value = D;
                Info.Result = true;
                return Info;
            }
        }

    } catch (e) {
        //
    }

    try { 
        var ms = Date.parse(v);
        if (!isNaN(ms)) {
            Info.Value = new Date(ms);
            Info.Result = true;
        }
    } catch (e) {
        //
    }

    return Info;
};
/**
 * Returns a specified Date value formatted as ISO Date string, i.e. yyyy-MM-dd.
 * @param   {Date} v The Date value to format
 * @returns  {string} The formatted string
 */
tp.ToISODateString = function (v) {
    return tp.FormatDateTime(v, tp.DateFormatISO);
};
/**
 * Formats a Date value to a string using local or ISO format. <br />
 * If a culture code is specified then the value is formatted according to that specified culture code, i.e. 'el-GR'.  <br />
 * If no culture code is specified, then the value is formatted according to the current culture, see: {@link tp.CultureCode}.    <br />
 * If the string 'ISO' is specified as the culture code, then the value is formatted as an ISO Date string, i.e. yyyy-MM-dd.
 * @param   {Date} v The Date value to format
 * @param   {string} [CultureCode=null] If null or empty, the default, then the date is formatted according to {@link tp.CultureCode} culture. Else a culture code, i.e. 'el-GR' or the string 'ISO' is required.
 * @returns  {string} The formatted string
 */
tp.ToDateString = function (v, CultureCode = null) {
    if (!tp.IsString(CultureCode) || tp.IsBlank(CultureCode))
        CultureCode = tp.CultureCode;

    if (CultureCode === 'ISO')
        return tp.ToISODateString(v);

    let format = tp.IsBlank(CultureCode) ? tp.DateFormat : tp.GetDateFormat(CultureCode);
    let Result = tp.FormatDateTime(v, format);
    return Result;
};
/**
 * Formats a Date value to a time string, optionally with seconds and milliseconds.
 * @param   {Date} v The Date value to format
 * @param   {boolean} [Seconds=false] Defaults to false. When true, then seconds are included in the returned string.
 * @param   {boolean} [Milliseconds=false] Defaults to false. When true, then seconds and milliseconds are included in the returned string.
 * @returns  {string} The formatted string
 */
tp.ToTimeString = function (v, Seconds = false, Milliseconds = false) {
    let format = 'HH:mm';

    if (Milliseconds == true)
        format = 'HH:mm:ss.fff';
    else if (Seconds === true)
        format = 'HH:mm:ss';

    return tp.FormatDateTime(v, format);
};
/**
 * Formats a Date value to a date-time string using local or ISO format, and optionally with seconds and milliseconds.
 * If a culture code is specified then the value is formatted according to that specified culture code, i.e. 'el-GR'.  <br />
 * If no culture code is specified, then the value is formatted according to the current culture, see: {@link tp.CultureCode}.    <br />
 * If the string 'ISO' is specified as the culture code, then the value is formatted as an ISO Date string, i.e. yyyy-MM-dd.
 * @param   {Date} v The Date value to format
 * @param   {string} [CultureCode=null] If null or empty, the default, then the date is formatted according to {@link tp.CultureCode} culture. Else a culture code, i.e. 'el-GR' or the string 'ISO' is required.
 * @param   {boolean} [Seconds=false] Defaults to false. When true, then seconds are included in the returned string.
 * @param   {boolean} [Milliseconds=false] Defaults to false. When true, then seconds and milliseconds are included in the returned string.
 * @returns  {string} The formatted string
 */
tp.ToDateTimeString = function (v, CultureCode = null, Seconds = false, Milliseconds = false) {
    return tp.ToDateString(v, CultureCode) + ' ' + tp.ToTimeString(v, Seconds, Milliseconds);
};
/**
Returns a Date value with the current date and time.
@returns {Date} The current date and time.
*/
tp.Now = function () {
    return new Date();
};
/**
Returns a Date value with the current date. Time part is zero-ed
@returns {Date} The current date
*/
tp.Today = function () {
    return tp.ClearTime(new Date());
};
/**
Returns a Date value with the current time. Time part is zero-ed
@returns {Date} The current time 
*/
tp.Time = function () {
    return tp.ClearDate(new Date());
};
/** 
 * Returns a number between 0..6 representing the day of the week.
 * @param {Date} v - The Date to operaton on.
 * @returns {number} The number of the day of the week (0..6).
 */
tp.DayOfWeek = function (v) {
    return v.getDay();
};
/** 
 * Returns  a number between 1..31 representing the day of the month.
 * @param {Date} v - The Date to operaton on.
   @returns {number} The number of day of the month (1..31)
 */
tp.DayOfMonth = function (v) {
    return v.getDate();
};
/**
 * Adds a specified number of Years to a Date value. Years could be negative.  
 Returns the modified Date.  
 CAUTION: The passed Date value is modified after this call.
 * @param {Date} v - The value to operate on
   @param {number} Years - How many years to add
   @returns {Date} The modified Date
 */
tp.AddYears = function (v, Years) {
    v.setFullYear(v.getFullYear() + Years);
    return v;
};
/**
 * Adds a specified number of Months to a Date value. Months could be negative.  
 Returns the modified Date.  
 CAUTION: The passed Date value is modified after this call.
 * @param {Date} v - The value to operate on
   @param {number} Months - How many months to add
   @returns {Date} The modified Date
 */
tp.AddMonths = function (v, Months) {
    v.setMonth(v.getMonth() + Months);
    return v;
};
/**
 * Adds a specified number of Days to a Date value. Days could be negative.  
 Returns the modified Date. 
 CAUTION: The passed Date value is modified after this call.
 * @param {Date} v - The value to operate on
   @param {number} Days - How many days to add
   @returns {Date}  The modified Date
 */
tp.AddDays = function (v, Days) {
    v.setDate(v.getDate() + Days);
    return v;
};
/**
 * Adds a specified number of Weeks to a Date value. Weeks could be negative.   
 Returns the modified Date. 
 CAUTION: The passed Date value is modified after this call.
 * @param {Date} v - The value to operate on
   @param {number} Weeks - How many weeks to add
   @returns {Date}  The modified Date
 */
tp.AddWeeks = function (v, Weeks) {
    return tp.AddDays(v, Weeks * 7);
};
/**
 * Adds a specified number of Hours to a Date value. Hours could be negative.  
 Returns the modified Date. 
 CAUTION: The passed Date value is modified after this call.
 * @param {Date} v - The value to operate on
   @param {number} Hours - How many hours to add
   @returns {Date} The modified Date
 */
tp.AddHours = function (v, Hours) {
    v.setTime(v.getTime() + Hours * 60 * 60 * 1000);
    return v;
};
/**
 * Adds a specified number of Minutes to a Date value. Minutes could be negative.  
 Returns the modified Date.  
 CAUTION: The passed Date value is modified after this call.
 * @param {Date} v - The value to operate on
   @param {number} Minutes - How many minutes to add
   @returns {Date} The modified Date
 */
tp.AddMinutes = function (v, Minutes) {
    v.setTime(v.getTime() + Minutes * 60 * 1000);
    return v;
};
/**
 * Adds a specified number of Seconds to a Date value. Seconds could be negative. 
 Returns the modified Date. 
 CAUTION: The passed Date value is modified after this call.
 * @param {Date} v - The value to operate on
   @param {number} Seconds - How many seconds to add
   @returns {Date} The modified Date
 */
tp.AddSeconds = function (v, Seconds) {
    v.setTime(v.getTime() + Seconds * 1000);
    return v;
};
/**
 * Sets the date part of Date value to zero
 * @param {Date} v - The value to operate on
   @returns {Date} The modified Date
 */
tp.ClearDate = function (v) {
    v.setFullYear(0);
    v.setMonth(0);
    v.setDate(0);
    return v;
};
/**
 * Sets the time part of Date value to zero
 * @param {Date} v - The value to operate on
   @returns {Date} The modified Date
 */
tp.ClearTime = function (v) {
    v.setHours(0);
    v.setMinutes(0);
    v.setSeconds(0);
    v.setMilliseconds(0);
    return v;
};
/**
 * Returns true if a specified Year is a leap year.
 * @param {number} Year The year to check
   @returns {boolean} True if is a leap year.
 */
tp.IsLeapYear = function (Year) {
    return Year % 4 === 0 && Year % 100 !== 0 || Year % 400 === 0;
};
/**
 * Returns the number of days in a Month of a specified Year.
 * @param {number} Year The year to check
   @param {number} Month The month to check
   @returns {number} The number of days in the month.
 */
tp.DaysInMonth = function (Year, Month) {
    return [31, (tp.IsLeapYear(Year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][Month];
};
/**
 * Compares two Date values. Returns 1 if A is greater, -1 if A is less and 0 when the values are equal.
 * @param {Date} A The first value
 * @param {Date} B The second value
   @returns {number} Returns 1 if A is greater, -1 if A is less and 0 when the values are equal.
 */
tp.DateCompare = function (A, B) {
    if (A === tp.Undefined) { A = null; }
    if (B === tp.Undefined) { B = null; }

    return A > B ? 1 : (A < B ? -1 : 0);
};
/**
Returns true if a specified Date value lays between the A and B Date values.
@param {Date} v - The date to check
@param {Date} A - The down limit
@param {Date} B - the upper limit
@returns {boolean} True if the specified date lays between the A and B Date values.
*/
tp.DateBetween = function (v, A, B) {
    if (A === tp.Undefined) { A = null; }
    if (B === tp.Undefined) { B = null; }

    return v >= A && v <= B;
};
/**
Clones a date and returns the new date
@param {Date} v - The date to clone
@returns {Date} The new date.
*/
tp.DateClone = function (v) {
    return new Date(v.getTime());
};

/**
Returns the start date-time of a specified Date value, i.e. yyyy-MM-dd 00:00:00
@param {Date} v - The date to clone
@returns {Date} Returns the start date-time of a specified Date value, i.e. yyyy-MM-dd 00:00:00
*/
tp.StartOfDay = function (v) {
    return tp.ClearTime(v);
};
/**
Returns the end date-time of a specified Date value, i.e yyyy-MM-dd 23:59:59
@param {Date} v - The date to clone
@returns {Date} Returns the end date-time of a specified Date value, i.e yyyy-MM-dd 23:59:59
*/
tp.EndOfDay = function (v) {
    v = tp.ClearTime(v);
    v = tp.AddDays(v, 1);
    v = tp.AddSeconds(v, -1);
    return v;
};

 

//#endregion

//#region Arrays 

/**
Type guard function. Returns true if a specified object provides a length property.
@param {any} v - The object to check.
@returns {boolean} - Returns true if a specified object provides a length property.
*/
tp.IsArrayLike = function (v) {
    return tp.IsValid(v) && 'length' in v;
};
/**
Returns true if a specified index is a inside the length of the specified array or array-like object.
@param {ArrayLike} List - The array to operate on
@param {number} Index The index to check
@returns {boolean} True if the specified index is a valid array index.
*/
tp.InRange = function (List, Index) { return Index >= 0 && Index <= List.length - 1; };
/**
Converts an array-like object, 
such as {@link https://developer.mozilla.org/en-US/docs/Web/API/NodeList|NodeList} 
or {@link https://developer.mozilla.org/en-US/docs/Web/API/HTMLCollection|HTMLCollection} 
or {@link https://developer.mozilla.org/en-US/docs/Web/API/HTMLFormControlsCollection|HTMLFormControlsCollection}, 
into a javascript array, by copying elements from the specified source object to a result array. Returns the array. 
@param  {object} List - The array-like object to operate on.
@returns {any[]}  An array.
 */
tp.ToArray = function (List) {
    var A = [];
    for (var i = 0, ln = List.length; i < ln; i++)
        A.push(List[i]);
    return A;
};

/**
True if a specified array or array-like object contains an item with a certain value. 
@param {object} List - The array-like object to operate on.
@param {any} v - The value to check
@returns {boolean}  True if the values exists in the array-like object
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
*/
tp.ListContains = function (List, v) {
    var i = List.length;
    while (i--) {
        if (List[i] === v) {
            return true;
        }
    }
    return false;
};
/**
True if a specified array or array-like object contains an item with a certain string value (case insensitive match). 
@param {object} List - The array-like object to operate on.
@param {string} v The value to check
@returns {boolean} True if the values exists in the array-like object
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
*/
tp.ListContainsText = function (List, v) {
    var S, i = List.length;
    while (i--) {
        S = !tp.IsEmpty(List[i]) ? List[i].toString() : '';
        if (tp.IsSameText(S, v)) {
            return true;
        }
    }
    return false;
};
/**
Returns the index of an item with a certain string value (case insensitive match) into a specified array or array-like object. 
@param {object} List - The array-like object to operate on.
@param {string} v The value to check
@returns {number} Returns the item index or -1 if not found.
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
*/
tp.ListIndexOfText = function (List, v) {
    var S;
    for (var i = 0, ln = List.length; i < ln; i++) {
        S = !tp.IsEmpty(List[i]) ? List[i].toString() : '';
        if (tp.IsSameText(S, v))
            return i;
    }
    return -1;
};
/**
Inserts an item in a specified index in an array.
@param {array} List The array to operate on  
@param {number} Index Where to insert the new item.
@param {any} v The item to insert
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
*/
tp.ListInsert = function (List, Index, v) {
    List.splice(Index, 0, v);
};
/**
Removes an item from a specified array. 
@param {array} List The array to operate on
@param {any} v The item to remove
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
*/
tp.ListRemove = function (List, v) {
    var i = List.indexOf(v);
    if (i !== -1) {
        List.splice(i, 1);
    }
};
/**
Removes an item found at a specified index in an array.
@param {array} List The array to operate on
@param {number} Index The position of the item to be removed.
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
*/
tp.ListRemoveAt = function (List, Index) {
    List.splice(Index, 1);
};
/**
Removes all items from an array
@param {array} List The array to clear.
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
*/
tp.ListClear = function (List) {
    List.length = 0;
};
/**
Clones an array
@param {array} List The array to clone.
@param {boolean} [Deep=true] Optional. If true a deep copy is performed. Defaults to true.
@returns {array} The array clone.
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
*/
tp.ListClone = function (List, Deep = true) {

    Deep = Deep === true;

    var i, ln, item, o, Result = [];

    if (Deep) {
        for (i = 0, ln = List.length; i < ln; i++) {
            item = List[i];
            if (tp.IsSimple(item)) {
                Result.push(item);
            } else {
                o = tp.Merge({}, item, true, true);
                Result.push(o);
            }
        }
    } else {
        for (i = 0, ln = List.length; i < ln; i++) {
            Result.push(List[i]);
        }
    }

    return Result;
};


/**
 * Returns true if the condition callback returns true for any of the elements in an array.
 *
 * @param {any[]} List The array to operate up on
 * @param {function} Func function(value, index, array), to be called on Context for each element.
 * @param {object} [Context=null] Optional. The context (this) to use when calling Func. Could be null.
 * @returns {boolean} Returns true if any of the array elements passes the check.
 */
tp.Any = function (List, Func, Context = null) { return List.some(Func, Context); };
/**
 * Returns true if all elements in the array pass the test implemented by the provided function.
 *
 * @param {any[]} List The array to operate up on
 * @param {function} Func function(value, index, array), to be called on Context for each element.
 * @param {object} [Context=null] Optional. The context (this) to use when calling Func. Could be null.
 * @returns {boolean} Returns true if all array elements pass the check.
 */
tp.All = function (List, Func, Context = null) { return List.every(Func, Context); };
/**
 * Returns a new array with the results of calling a provided function on every element in this array.
 *
 * @param {any[]} List The array to operate up on
 * @param {function} Func function(value, index, array), to be called on Context for each element.
 * @param {object} [Context=null] Optional. The context (this) to use when calling Func. Could be null.
 * @returns {any[]} Returns a new array with the results of calling a provided function on every element in this array.
 */
tp.Transform = function (List, Func, Context = null) { return List.map(Func, Context); };
/**
 *  Selects all objects found in an array having a distinct value in a specified property.
 * @param {array} List The array to operate up on
 * @param {string} Prop The name of the property to look for distinct values.
 * @returns {array} A new array with objects having a distinct value in the specified property or an empty array.
 */
tp.Distinct = function (List, Prop) {
    var Unique = {};
    var Result = [];

    var o;
    for (var i = 0, ln = List.length; i < ln; i++) {
        o = List[i];
        if (!Unique[o[Prop]]) {
            Result.push(o);
            Unique[o[Prop]] = true;
        }
    }

    return Result;
};
/**
 * Returns a new array containing any object found in a specified array having a specified property with a specified value.
 * @param {array} List The array to operate up on
 * @param {string} Prop The name of the property to match
 * @param {any} v The value the property must have. NOTE: If v is an object (that is NOT string, number, boolean, null, undefined) then a reference equality check takes place.
 * @returns {array} May be an empty array.
 */
tp.Where = function (List, Prop, v) {
    var Result = [];

    var o;
    for (var i = 0, ln = List.length; i < ln; i++) {
        o = List[i];
        if (o[Prop] === v) {
            Result.push(o);
        }
    }

    return Result;
};
/**
 * Returns a new array containing any object found in an array having the properties and the values of a specified object.
 * @param {array} List The array to operate up on
 * @param {object} Props An object with one or more properties to check for matchings
 * @returns {array} May be an empty array.
 */
tp.WhereAll = function (List, Props) {
    var Result = [];

    var o;
    for (var i = 0, ln = List.length; i < ln; i++) {
        o = List[i];
        if (tp.Equals(o, Props)) {
            Result.push(o);
        }
    }

    return Result;
};
/**
 * Returns the first element of the sequence that satisfies the Func condition or null if no such element is found.
 * This method it is actually FirstOrNull()
 * @param {any[]} List The array to operate up on
 * @param {function} Func function(value, index, array), to be called on Context for each element.
 * @param {object} [Context=null] Optional. The context (this) to use when calling Func. Could be null.
 * @returns {any} The first element in the array that passes the condition, if any, or a null value.
 */
tp.FirstOrDefault = function (List, Func, Context = null) {
    var Result = null;

    for (var i = 0, ln = List.length; i < ln; i++) {
        if (Func.call(Context, List[i], i, List) === true) {
            Result = List[i];
            break;
        }
    }

    return Result;
};






/**
Information object for use with sorting algorithms. Used when sorting arrays.
@class
@param {string|number} Prop - Property name or property index of a property in a plain javascript object
@param {boolean} Reverse - When true the property is sorted in reverse order
@param {function} GetValueFunc - A callback that is passed an object being sorted and returns the value of one of its properties
 */
tp.SortInfo = function (Prop, Reverse, GetValueFunc) {
    this.Prop = Prop;
    this.Reverse = Reverse;
    this.GetValueFunc = GetValueFunc;
};
/** @property {string|number} Prop - Property name or property index of a property in a plain javascript object */
tp.SortInfo.prototype.Prop = '';
/** @property {boolean} Reverse - When true the property is sorted in reverse order  */
tp.SortInfo.prototype.Reverse = false;
/**
A callback that is passed an object being sorted and returns the value of one of its properties
@param {ArrayLike|any} Row - The object that is currently sorted
@param {tp.SortInfo} Info - Information about the property
@returns {any} Returns the value of the property
*/
tp.SortInfo.prototype.GetValueFunc = function (Row, Info) {
    return null;
};

/**
 Sorts an array, in place, by multiple properties. 
 Accepts two parameters: 
 1. the array to sort and 
 2. an array of objects containing information of how to sort.

 The array to sort could be an array of plain objects or an array of arrays of primitive values. 
 The array of information objects is a heterogenous collection and may contain elements such as:
 1. string (denoting a property name)
 2. integer (denoting an array index)
 3. SortInfo object ( {Prop: string|integer, Reverse: Boolean, GetValueFunc: function (ArrayRow, Info) } )
 @example
// array of primitive values
var List = [[true, 4], [false, 1], [true, 2]];

// sorts based on property index: a) by integer in reverse order, and then b) by boolean,
tp.ListSort(List, [{ Prop: 1, Reverse: true }, 0]); // results in [[true, 4], [true, 2], [false, 1]]
 @example
// array of objects
var List = [{ Name: 'John', Age: 30 }, { Name: 'Jane', Age: 31 }, { Name: 'Jack', Age: 20 }, ];

// sorts just by Age property in reverse order
tp.ListSort(List, [{ Prop: 'Age', Reverse: true }]); // results in  [{ Name: 'Jane', Age: 31 }, { Name: 'John', Age: 30 }, { Name: 'Jack', Age: 20 }, ]

// sorts by Name property
tp.ListSort(List, ['Name']); // results in [{ Name: 'Jack', Age: 20 }, { Name: 'Jane', Age: 31 }, { Name: 'John', Age: 30 } ]

@param {any[]} List - A collection of plain objects, or a collection of arrays of primitive values
@param {any[]} SortInfos - A collection. Each element of the collection could be either 
1. a string (denoting a property name), or
2. an integer (denoting a array index), or
3. a sort information object as  {Prop: string|integer, Reverse: Boolean, GetValueFunc: function (ArrayRow, Info) } 
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
 */
tp.ListSort = function (List, SortInfos) {

    let InfoList = []; // array of SortInfo

    //----------------------------
    /**
     * Default implementation of the GetValueFunc. 
     * Returns the value of a property of an element of the array being sorted.
     * @param {ArrayLike|any} Row - An element of the array being sorted.
     * @param {SortInfo} Info - Information on how to get the value of the property
     * @returns {any} Returns the value of a property of Row
     */
    function GetValueFunc(Row, Info) {
        return Row[Info.Prop];
    }
    //----------------------------
    /**
     * Compare function, compares two items.
     * @param {any} A - The first item in comparison
     * @param {any} B - The second item in comparison
     * @returns {number} Returns 1 if A greater thant B, -1 if B greater than A, and 0 when equal.
     */
    function CompareFunc(A, B) {
        let i, ln, a, b, Result = 0;
        let Info;

        for (i = 0, ln = InfoList.length; i < ln; i++) {
            Info = InfoList[i];

            // get the values
            a = Info.GetValueFunc(A, Info);
            b = Info.GetValueFunc(B, Info);

            // compare
            Result = a === b ? 0 : (Info.Reverse ? (a > b ? -1 : 1) : (a < b ? -1 : 1));

            if (Result !== 0) {
                break;
            }
        }

        return Result;
    }
    //----------------------------

    // prepare the sort info items
    let i, ln, Info;

    for (i = 0, ln = SortInfos.length; i < ln; i++) {
        if (tp.IsNumber(SortInfos[i]) || tp.IsString(SortInfos[i])) {       // it's string or integer, so 
            Info = {                                                        // create a SortInfo instance
                Prop: SortInfos[i],
                Reverse: false,
                GetValueFunc: GetValueFunc
            };

        } else {
            Info = SortInfos[i];
            if (!tp.IsFunction(Info.GetValueFunc)) {
                Info.GetValueFunc = GetValueFunc;
            }
        }

        InfoList.push(Info);
    }


    // sort
    List.sort(CompareFunc);

};


/**
A static helper class for performing comparisons in primitives (string, number or boolean) or dates. 
Provides a list of filter comparison operator constants and a Compare() method.
@class
*/
tp.FilterOp = {
    None: 0,

    GT: 1,          // greater  than
    GE: 2,          // greater or equeal
    EQ: 4,          // equal
    NE: 8,          // not equal
    LT: 0x10,       // less than
    LE: 0x20,       // less or equal
    CO: 0x40,       // contains
    SW: 0x80,       // starts with
    EW: 0x100,      // ends width


    /**  Greater */
    get Greater() { return tp.FilterOp.GT; },
    /**  GreaterOrEqual */
    get GreaterOrEqual() { return tp.FilterOp.GE; },
    /**  Equal */
    get Equal() { return tp.FilterOp.EQ; },
    /**  NotEqual */
    get NotEqual() { return tp.FilterOp.NE; },
    /**  Less */
    get Less() { return tp.FilterOp.LT; },
    /**  LessOrEqual */
    get LessOrEqual() { return tp.FilterOp.LE; },

    /**  Contains */
    get Contains() { return tp.FilterOp.CO; },
    /**  StartsWith */
    get StartsWith() { return tp.FilterOp.SW; },
    /**  EndsWith */
    get EndsWith() { return tp.FilterOp.EW; },

    /**  
    Compares two values (primitives or dates) using one of the comparison operator constants and
    returns a boolean result indicating whether the operands pass the test condition the comparison operator represents.
    @param {number} Operator - One of the comparison operator constants of this class.
    @param {string|number|boolean|date} A - The first value to compare
    @param {string|number|boolean|date} B - The second value to compare
    @returns {Boolean} Returns a boolean value depending on the specified comparison operator.
    */
    Compare(Operator, A, B) {

        if (A === tp.Undefined) { A = null; }
        if (B === tp.Undefined) { B = null; }

        if (A instanceof Date) { A = A.valueOf(); }
        if (B instanceof Date) { B = B.valueOf(); }

        switch (Operator) {
            case tp.FilterOp.Greater: return A > B;
            case tp.FilterOp.GreaterOrEqual: return A >= B;
            case tp.FilterOp.Equal: return A === B;
            case tp.FilterOp.NotEqual: return A !== B;
            case tp.FilterOp.Less: return A < B;
            case tp.FilterOp.LessOrEqual: return A <= B;

            case tp.FilterOp.Contains: return tp.ContainsText(A, B, true);
            case tp.FilterOp.StartsWith: return tp.StartsWith(A, B, true);
            case tp.FilterOp.EndsWith: return tp.EndsWith(A, B, true);
        }

        return false;
    }

};
Object.freeze(tp.FilterOp);


/**
* Information object for use with filtering algorithms. Used when filtering arrays.
* @class
* @param {string|number} Prop - Property name or property index of a property in a plain javascript object
* @param {any} Value - The filter value
* @param {number} Operator - The filter operator. One of the tp.FilterOp constants
* @param {function} FilterFunc - A callback that is passed an object being filtered and returns a boolean value indicating whether the row passes the filter condition.
*/
tp.FilterInfo = function (Prop, Value, Operator, FilterFunc) {
    this.Prop = Prop;
    this.Value = Value;
    this.Operator = Operator;
    this.FilterFunc = FilterFunc;
};
/** @property {string|number} Prop - Property name or property index of a property in a plain javascript object */
tp.FilterInfo.prototype.Prop = '';
/** @property {any} Value - The filter value */
tp.FilterInfo.prototype.Value = null;
/** @property {number} Operator - The filter operator. One of the tp.FilterOp constants */
tp.FilterInfo.prototype.Operator = tp.FilterOp.EQ;
/**
A callback that is passed an object being filtered and returns a boolean value indicating whether the row passes the filter condition.
@param  {ArrayLike|any} Row - The object that is currently sorted
@param {tp.FilterInfo} Info - Information about the property
@returns {boolean} Returns boolean value indicating whether the row passes the filter condition.
*/
tp.FilterInfo.prototype.FilterFunc = function () {
    return false;
};


/**
Filters an array by multiple properties and returns the new filtered array.
The array could be an array of plain objects or an array of arrays of primitive values. 
@example 
// array of primitive values
var List = [[true, 4], [false, 1], [true, 2]];
 
// filters based on index
List = tp.ListFilter(List, [{ Prop: 0, Operator: tp.FilterOp.Equal, Value: true }], false); // results in [[true, 4], [true, 2]]
 
@example 
// array of objects
var List = [{ Name: 'John', Age: 30 }, { Name: 'Jane', Age: 31 }, { Name: 'Jack', Age: 20 }, ];
 
// filter by 'Name' property when it contains the letter 'o'
List = tp.ListFilter(List, [{ Prop: 'Name', Operator: tp.FilterOp.Contains, Value: 'o', }], false); // results in [{ Name: 'John', Age: 30 }]
 
@param {any[]} List - A collection of plain objects, or a collection of arrays of primitive values
@param {tp.FilterInfo[]} FilterInfos - A collection of objects as <code>  { Prop: String | Integer, Operator: tp.FilterOp, Value: Any, FilterFunc: function (ArrayRow, Info) } </code>
@param {boolean} [OrLogic=false] Optional. True for OR logic, false for AND logic. Defaults to false. 
@returns {any[]} A new array after filtering the passed input array.
@see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array|Array in MDN}
@see {@link http://kangax.github.io/compat-table/es5/|kangax tables}
 */
tp.ListFilter = function (List, FilterInfos, OrLogic = false) {

    OrLogic = OrLogic === true;
    let InfoList = [];

    //----------------------------
    /**
     * Gets the property value from a specified Row item (element of the filtered array). 
     * Compares that value to a tp.FilterInfo.Value.
     * Returns a boolean value indicating whether the row passes the filter condition.
     * @param {ArrayLike|any} Row - An element of the array being filtered.
     * @param {tp.FilterInfo} Info - Information on how to get the value of the property and filter it.
     * @returns {boolean} Returns a boolean value indicating whether the row passes the filter condition.
     */
    function FilterFunc(Row, Info) {
        var Value = Row[Info.Prop];
        return tp.FilterOp.Compare(Info.Operator, Value, Info.Value);
    }
    //----------------------------
    /**
     * This callback is passed to Array.filter().
     * This function is a predicate, to test each element of the array. Returns true to keep the element, false otherwise.
     * @param {any} Row - The current element being processed in the array.
     * @returns {boolean} Returns true to keep the element, false otherwise.
     */
    function ArrayFilterFunc(Row) {
        var i, ln, Info,
            Result = OrLogic ? false : true;

        for (i = 0, ln = InfoList.length; i < ln; i++) {
            Info = InfoList[i];

            Result = Info.FilterFunc(Row, Info);
            if (!OrLogic) {
                if (!Result) {
                    break;
                }
            } else if (Result) {
                break;
            }

        }


        return Result;
    }
    //----------------------------

    // prepare the info items
    var i, ln, Info;

    for (i = 0, ln = FilterInfos.length; i < ln; i++) {
        Info = FilterInfos[i];
        Info.Operator = Info.Operator || tp.FilterOp.Equal;
        if (!tp.IsFunction(Info.FilterFunc)) {
            Info.FilterFunc = FilterFunc;
        }

        InfoList.push(Info);
    }


    // filter
    return List.filter(ArrayFilterFunc);

};

/**
 * Node-like object for use with grouping algorithms. Used when grouping arrays.
 * @class
 * @param {any} Key -  The key of this group. It is empty for the root node.
 * @param {tp.GroupByNode[]} NodeList - A list with the child group nodes
 * @param {any[]} DataList - A list with the data rows of this group (rows that are not nodes)
 */
tp.GroupByNode = function (Key, NodeList, DataList) {
    this.Key = Key;
    this.NodeList = NodeList || [];
    this.DataList = DataList || [];
};
/** @property {any} Key -  The key of this group. It is empty for the root node. */
tp.GroupByNode.prototype.Key = {};
/** @property {tp.GroupByNode[]} NodeList - A list with the child group nodes */
tp.GroupByNode.prototype.NodeList = [];
/** @property {any[]} DataList - A list with the data rows of this group (rows that are not nodes) */
tp.GroupByNode.prototype.DataList = [];


/**
Groups an array of objects based on an array of property names.
Returns an object like { Key: 'key value', NodeList: [child group nodes], DataList: [rows in this group that are not nodes] }
@example
// array of objects
var List = [{ Name: 'John', Country: 'GR', Dep: 'IT' },
    { Name: 'Jane', Country: 'US', Dep: 'Sales' },
    { Name: 'Jack', Country: 'US', Dep: 'IT' },
    { Name: 'Josh', Country: 'US', Dep: 'Sales' },
    { Name: 'Jess', Country: 'GR', Dep: 'Sales' }];

// group by Country
 var Result = tp.GroupBy(List, ['Country']);

// group by Department, Country
Result = tp.GroupBy(List, ['Dep', 'Country']); 
 
@param {any[]} List - A collection of plain objects.
@param {string[]} PropNames The property names to be used in grouping
@param {boolean} [AlwaysIncludeDataLists=false] Defaults to false. If true then each returned key object (group node) contains its own data-list, except of the root node.
@returns {object} An object like { Key: 'key value', NodeList: [child group nodes], DataList: [rows in this group that are not nodes] }
 */
tp.GroupBy = function (List, PropNames, AlwaysIncludeDataLists = false) {

    let RootKey = '___ROOT___';
    /**
     * Groups a data list (array of objects) based on the index of a property name in the PropNames array
     * @param {tp.GroupByNode} ParentNode The parent group node
     * @param {any[]} DataList A list with the data rows of this group (rows that are not nodes)
     * @param {number} PropNameIndex The index of the property name in the PropNames array.
     */
    function _GroupBy(ParentNode, DataList, PropNameIndex) {
        var PropName = PropNames[PropNameIndex];

        var i, ln, Data, Key, Node, Groups = {};

        for (i = 0, ln = DataList.length; i < ln; i++) {
            Data = DataList[i];
            Key = Data[PropName];
            if (Key in Groups === false) {
                Groups[Key] = [];
            }

            Groups[Key].push(Data);
        }

        var Keys = Object.keys(Groups);

        for (i = 0, ln = Keys.length; i < ln; i++) {
            Key = Keys[i];
            Node = {
                Key: Key,
                NodeList: PropNameIndex === PropNames.length - 1 ? null : [],
                DataList: PropNameIndex === PropNames.length - 1 || AlwaysIncludeDataLists === true ? Groups[Key] : null
            };

            ParentNode.NodeList.push(Node);
        }


        if (PropNameIndex < PropNames.length - 1) {
            for (i = 0, ln = ParentNode.NodeList.length; i < ln; i++) {
                Node = ParentNode.NodeList[i];
                _GroupBy(Node, Groups[Node.Key], PropNameIndex + 1);
            }
        }

    }

    var RootNode = {
        Key: RootKey,
        NodeList: [],
        DataList: null
    };

    _GroupBy(RootNode, List, 0);

    return RootNode;
};


//#endregion

//#region Units


/**
* list of all units and their identifying string
@see {@link http://www.w3schools.com/cssref/css_units.asp|w3schools}
@class
@static
*/
tp.UnitMap = {
    pixel: "px",
    percent: "%",
    inch: "in",
    cm: "cm",
    mm: "mm",
    point: "pt",
    pica: "pc",
    em: "em",
    ex: "ex"
};
Object.freeze(tp.UnitMap);


/**
 Extracts and returns the unit suffix of a string value, i.e. px, %, em, etc. 
 taken from: http://upshots.org/javascript/javascript-get-current-style-as-any-unit
 
 @param {string} v The value, i.e. 2px, 100%, etc.
 @returns {string} Returns the unit suffix of a string value, i.e. px, %, em, etc.
 */
tp.ExtractUnit = function (v) {
    if (typeof v === 'string') {
        var unit = v.match(/\D+$/);                           // get the existing unit
        var s = unit === null ? tp.UnitMap.pixel : unit[0];   // if its not set, assume px - otherwise grab string
        return s;
    }

    return '';
};
/**
 * Returns the number out of a string value, that is extracts the number found in a string value like 2px or 100%
 * taken from: http://stackoverflow.com/questions/3530127/convert-css-width-string-to-regular-number
 * @param   {string} v The value, i.e. 2px, 100%, etc.
   @returns {number} Returns the number out of a string value
 */
tp.ExtractNumber = function (v) {
    if (typeof v === 'number')
        return v;
    return !tp.IsBlank(v) ? Number(v.replace(/[^\d\.\-]/g, '')) : 0;
};

/**
Returns true if a specified string value is a pixel value, e.g. 10px
@param {string} v - The value to check, e.g. 10px
@returns {boolean} Returns true if the specified string passes the test.
*/
tp.IsPixel = function (v) { return tp.UnitMap.pixel === tp.ExtractUnit(v); };
/**
Returns true if a specified string value is a em value, e.g. 2em
@param {string} v - The value to check, e.g. 2em
@returns {boolean} Returns true if the specified string passes the test.
*/
tp.IsEm = function (v) { return tp.UnitMap.em === tp.ExtractUnit(v); };
/**
Returns true if a specified string value is a percent value, e.g. 10%
@param {string} v - The value to check, e.g. 10%
@returns {boolean} Returns true if the specified string passes the test.
*/
tp.IsPercent = function (v) { return tp.UnitMap.percent === tp.ExtractUnit(v); };
/** Converts a number or string into a pixel unit value 
@param {number|string} v - The to convert
@returns {string} Returns true if the specified string passes the test.
*/
tp.px = function (v) { return v.toString() + "px"; };


//#endregion

//#region Css

/**
For handling css sheets
@class
@static
*/
tp.Css = {
    /**
     * Creates a style sheet element under Id
     * @param {string} Id The id of the new sheet
     * @returns {StyleSheet} Returns the sheet
     */
    CreateSheet: function (Id) {

        // Create the <style> tag
        var style = document.createElement("style");
        style.id = Id;

        // WebKit hack :(
        style.appendChild(document.createTextNode(""));

        // Add the <style> element to the page
        document.head.appendChild(style);

        //var sheet = style.sheet;
        var sheet = style.sheet ? style.sheet : style['styleSheet'];

        return sheet;
    },
    /**
     * Returns a sheet object  by Id. <br />
     * <strong>WARNING</strong>: No # prefix is used.
     * @param {string} Id The id of the sheet
     * @returns {StyleSheet} Returns the sheet
     */
    SheetById: function (Id) {

        var Sheets = document.styleSheets;
        var NodeId;
        for (var i = 0; i < Sheets.length; i++) {
            NodeId = Sheets[i].id || Sheets[i].ownerNode.id;
            if (NodeId === Id) {
                return Sheets[i];
            }
        }
        return null;
    },
    /**
     * Creates a css Class with ClassDef in Sheet. <br />
     * <strong>WARNING</strong>: No surrounding brackets in ClassDef parameter.
     * @param {StyleSheet} Sheet The sheet
     * @param {string} ClassName The name of the class to create
     * @param {string} ClassDef The class definition
     */
    CreateClass: function (Sheet, ClassName, ClassDef) {
        if (Sheet.insertRule) {   // all browsers, except IE before version 9
            var S = tp.Format("{0} {{ 1}}", ClassName, ClassDef);
            Sheet.insertRule(S, 0);
        }
        /*
    else {  // Internet Explorer before version 9
        if (Sheet.addRule) {
                Sheet.addRule(ClassName, ClassDef);
            }
        }
        */
    },
    /**
     * Returns the definition of a css class
     * @param {string} ClassName The name of the class
     * @returns {string} Returns the definition of a css class
     */
    GetClassDef: function (ClassName) {
        for (let i = 0; i < document.styleSheets.length; i++) {
            let sheet = document.styleSheets[i];
            let classes = sheet.rules || sheet.cssRules;
            for (let x = 0; x < classes.length; x++) {
                if (classes[x].selectorText && -1 !== classes[x].selectorText.indexOf(ClassName)) {
                    return classes[x].cssText || classes[x].style.cssText;
                }
            }
        }
        return '';
    },
    /**
     * Returns true if ClassName is a css class defined in any of the css sheets of the page. <br />
     *  <strong>WARNING</strong>: For single selector classes ONLY.
     * @param {string} ClassName The name of the class
     * @returns {boolean}  Returns true if ClassName is a css class defined in any of the css sheets of the page.
     */
    IsClassDefined: function (ClassName) {
        let Sheets = document.styleSheets;

        for (let i = 0; i < Sheets.length; i++) {
            let sheet = Sheets[i];
            let classes = sheet.rules || sheet.cssRules;
            for (let j = 0; j < classes.length; j++) {
                if (classes[j].selectorText === ClassName) {
                    return true;
                }
            }
        }
        return false;
    }
};

//#endregion

//#region Url handling
/**
 * Navigates to a specified url
 * @param {string} Url Url to navigate to
 */
tp.NavigateTo = function (Url) {
    if (!tp.IsBlank(Url))
        window.location.href = Url;
};
/**
Returns the base url, e.g http://server.com/
@returns {string} Returns the base url
*/
tp.GetBaseUrl = function () { return window.location.protocol + "//" + window.location.host + "/"; };
/**
 * Returns a query string parameter by name, if any, else null
 * @param {string} Name - The name of the parameter
 * @param {string} [Url] - Optional. If not specified then the current url is used
   @returns {string} Returns a query string parameter by name, if any, else null
 */
tp.ParamByName = function (Name, Url = null) {
    if (!Url)
        Url = window.location.href;

    Name = Name.replace(/[\[\]]/g, "\\$&");

    var regex = new RegExp("[?&]" + Name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(Url);

    if (!results)
        return null;

    if (!results[2])
        return '';

    return decodeURIComponent(results[2].replace(/\+/g, " "));
};

/**
 * Returns a plain object where each property is a query string parameter.
 * @param {string} [Url] - Optional. If not specified then the current url is used
   @returns {object} Returns a plain object where each property is a query string parameter.
 */
tp.GetParams = function (Url = null) {
    if (!Url)
        Url = window.location.href;

    var Result = {};

    var Index = Url.indexOf('?');

    if (Index !== -1) {
        var Parts,
            List = Url.slice(Index + 1).split('&');

        for (var i = 0; i < List.length; i++) {
            Parts = List[i].split('=');

            Result[Parts[0]] = decodeURIComponent(Parts[1]);
        }
    }

    return Result;
};

//#endregion

//#region Selecting and finding elements

/**
Selects and returns a direct or non-direct child element, if any, or null, in a specified parent or the document. <br />
NOTE: If only a single parameter is passed then it is considered as the element selector to select in the whole document.
@param {string|Node} ParentElementOrSelector - The parent element where the element is a direct or non-direct child. If not specified (i.e. passed as null) then the document is used.
@param {string|Node} ElementOrSelector - The child element to select.
@returns {Element} Returns a child element, if any, or null.
*/
tp.Select = function (ParentElementOrSelector, ElementOrSelector) {
    let Parent = null,
        el = null;

    if (arguments.length === 2) {
        Parent = tp.IsString(arguments[0]) ? document.querySelector(arguments[0]) : arguments[0];
        el = arguments[1];
    } else if (arguments.length === 1) {
        Parent = document;
        el = arguments[0];
    }

    if (tp.IsNodeSelector(Parent) && tp.IsString(el))
        el = Parent.querySelector(el);

    if (tp.IsHTMLElement(el))
        return el;

    return null;

};
/**
Selects and returns a NodeList of all direct or non-direct child elements, in a specified parent, or an empty NodeList.<br />
NOTE: If only a single parameter is passed then it is considered as the element selectors to select in the whole document.
@param {string|Node} ParentElementOrSelector -  Optional. The parent element where the elements are direct or non-direct children. If not specified the document is used.
@param {string} Selectors - A comma separated list of selectors, e.g. input, select, textarea, button
@returns {NodeList} - Returns a NodeList of all direct or non-direct child elements, or an empty array.
*/
tp.SelectAll = function (ParentElementOrSelector, Selectors) {
    let Parent = null,
        sSelectors = null;

    if (arguments.length === 2) {
        Parent = tp.IsString(arguments[0]) ? document.querySelector(arguments[0]) : arguments[0];
        sSelectors = arguments[1];
    } else if (arguments.length === 1) {
        Parent = document;
        sSelectors = arguments[0];
    }

    if (tp.IsNodeSelector(Parent) && tp.IsString(sSelectors))
        return Parent.querySelectorAll(sSelectors);

    return [];
};
/**
Returns the closest ancestor (parent node) of a specified element which matches a specified selector. 
If there isn't such an ancestor, it returns null.
@param {Element} el - The element the closest ancestor is to be found.
@param {string} Selector - A selector for the closest ancestor
@returns {HTMLElement} - 
@see {@link https://developer.mozilla.org/en-US/docs/Web/API/Element/closest}
*/
tp.Closest = function (el, Selector) {
    var Result = el.closest(Selector);
    return Result instanceof HTMLElement ? Result : null;
};
/**
Returns the FIRST text node of an element, if any, else null.
@param {Element} el - The element.
@returns {any} - Returns the FIRST Text node or null
*/
tp.FindTextNode = function (el) {
    if (tp.IsElement(el) && el.hasChildNodes()) {
        var List = el.childNodes;
        for (let i = 0, ln = List.length; i < ln; i++) {
            if (List[i].nodeType === Node.TEXT_NODE) {
                return List[i];
            }
        }
    }

    return null;
};
/**
* Returns the index of an element in its parent's children collection, if any, else -1.
* @param {Element} elParent - The parent element  
* @param {Element} el - The element to find.
* @returns {number} Returns the index of the element in its parent, or -1.
*/
tp.ChildIndex = function (elParent, el) {
    if (tp.IsElement(el) && tp.IsElement(elParent)) {
        var List = elParent.children; // children is an HTMLCollection, it provides no methods at all
        for (var i = 0, ln = List.length; i < ln; i++) {
            if (List[i] === el)
                return i;
        }
    }

    return -1;
};
/**
Returns ONLY the direct HTMLElement children of a specified element.
NOTE: HTMLElement.children property returns an HTMLCollection which is a collection of Element elements.
That is it filters out any non-Element nodes such as #text or #comment nodes, etc.
But there is at least one Element, the svg, which is not HTMLElement.
So it is not always safe to assume that the HTMLElement.children will contain just HTMLElement elements.
 
@param {Element|string} ElementOrSelector - The parent dom element
@return {HTMLElement[]} Returns an array with the direct HTMLElement children of a specified element
*/
tp.ChildHTMLElements = function (ElementOrSelector) {
    var el = tp.Select(ElementOrSelector);
    var Result = [];

    if (el instanceof HTMLElement) {

        /*
        NOTE: HTMLElement.children property returns an HTMLCollection which is a collection of Element elements.
        That is it filters out any non-Element nodes such as #text or #comment nodes, etc.
        But there is at least one Element, the svg, which is not HTMLElement.
        So it is not always safe to assume that the HTMLElement.children will contain just HTMLElement elements.
        */
        let List = el.children;

        for (var i = 0, ln = List.length; i < ln; i++) {
            if (List[i] instanceof HTMLElement) {
                Result.push(List[i]);
            }
        }

    }

    return Result;
};
/**
Returns true when an element is contained directly or indirectly by a parent element.
@param {Element} Parent - The parent DOM element
@param {Element} el - The element to check.
@returns {boolean} -
*/
tp.ContainsElement = function (Parent, el) {
    if (tp.IsValid(Parent) && 'contains' in Parent) {
        return Parent.contains(el);
    } else if (tp.IsValid(el)) {
        var Node = el.parentNode;
        while (!tp.IsEmpty(Node)) {
            if (Node === Parent) {
                return true;
            }
            Node = Node.parentNode;
        }
    }

    return false;
};
/**
Returns true if the target (sender) of an event is a specified element or any other element contained by the specified element as direct or nested child.
@param {HTMLElement} el - The container element to check
@param {EventTarget} target - The sender of the event
@returns {boolean} Returns a boolean value indicating whether the target is the specified element or is contained by the specified element.
*/
tp.ContainsEventTarget = function (el, target) {
    return el === target || target instanceof HTMLElement && tp.ContainsElement(el, target);
};

/**
Returns an element if the id of that element ends width a specified id, else null.
@param {string} IdEnding - The ending of the Id 
@param {Element} [ParentElement=null] - Optional. If null, document is used as parent.
@returns {any} Returns the found html element or null.
*/
tp.FindElementWithIdEnding = function (IdEnding, ParentElement = null) {
    ParentElement = ParentElement || document;

    var NodeList, Result = [], i, len, rgx;

    NodeList = ParentElement.getElementsByTagName('*');
    len = NodeList.length;
    rgx = new RegExp(IdEnding + '$');
    var el;
    for (i = 0; i < len; i++) {
        if (NodeList[i] instanceof HTMLElement) {
            el = NodeList[i];
            if (rgx.test(el.id)) {
                Result.push(el);
            }
        }
    }

    if (Result.length > 0) {
        return Result[0];
    }

    return null;
};


//#endregion

//#region DOM handling

/**
Adds a element of a specified node type to a parent element and returns the newly created element.  
@param {string|Element} ParentOrSelector Selector or element.
@param {string} [TagName='div'] The tag type name. Defaults to 'div'.
@returns {HTMLElement} Returns the newly created element.
*/
tp.el = function (ParentOrSelector, TagName = 'div') {
    ParentOrSelector = tp.Select(ParentOrSelector);

    if (tp.IsHTMLElement(ParentOrSelector)) {
        TagName = TagName || 'div';
        var Result = ParentOrSelector instanceof Document ? ParentOrSelector.createElement(TagName) : ParentOrSelector.ownerDocument.createElement(TagName);
        ParentOrSelector.appendChild(Result);
        return Result;
    }

    return null;
};
/**
Adds a div to a parent element and returns the div element.
@param {string|Element} ParentOrSelector Selector or element.
@returns {HTMLDivElement} Returns the newly created element.
*/
tp.Div = function (ParentOrSelector) { return tp.el(ParentOrSelector, 'div'); };
/**
Adds a span to a parent element and returns the span element.
@param {string|Element} ParentOrSelector Selector or element.
@returns {HTMLSpanElement} Returns the newly created element.
*/
tp.Span = function (ParentOrSelector) { return tp.el(ParentOrSelector, 'span'); };
/**
Adds a specified text as a paragraph to a parent element and returns the paragraph element.
@param {string|Element} ParentOrSelector Selector or element.
@param {string} [Text=''] The text of the paragraph. Defaults to empty string. 
@returns {HTMLParagraphElement} Returns the newly created element.
*/
tp.Paragraph = function (ParentOrSelector, Text = '') {
    let el = tp.el(ParentOrSelector, 'p');
    if (el && !tp.IsBlank(Text)) {
        el.innerText = Text;
    }
    return el;
};
/**
Adds a text break to an element.
@param {string|Element} ParentOrSelector Selector or element.
*/
tp.Break = function (ParentOrSelector) {
    ParentOrSelector = tp.Select(ParentOrSelector);
    if (tp.IsHTMLElement(ParentOrSelector))
        tp.Append(ParentOrSelector, '');
};

/**
Gets or sets the value of an element. 
To be used mainly with input, select and textarea elements.
NOTE: Passing both arguments, sets the value of an element.
@param {string|Element}  el - A selector or an element
@param {any} [v=null] - The value to set to the element. If null then the function returns the element's value
@returns {any} If a value is not specified then the function returns the element's value, else returns the passed value.
*/
tp.val = function (el, v = null) {
    el = tp.Select(el);
    if (!el) return null;

    var i, ln, o;
    var NodeName = el.nodeName.toLowerCase();
    var IsInput = NodeName === 'input';
    var IsTextInput = NodeName === 'textarea' || IsInput && tp.ListContainsText(['text', 'password', 'button', 'submit', 'reset'], el.type);
    var IsCheckInput = IsInput && tp.ListContainsText(['check', 'radio'], el.type);
    var IsSelect = NodeName === 'select';
    var IsButton = NodeName === 'button';

    if (tp.IsEmpty(v)) {
        // get
        if (IsSelect) {
            return tp.InRange(el.options, el.selectedIndex) ? el.options[el.selectedIndex].value : null;
        } else if (IsTextInput) {
            return el.value;
        } else if (IsCheckInput) {
            return el.checked;
        } else if (IsInput) {
            return el.value;
        } else if (IsButton) {
            return el.innerHTML;
        }
        return 'textContent' in el ? el.textContent : el.innerHTML;
    } else {
        // set
        if (IsSelect) {
            let Flag = false;
            for (i = 0, ln = el.options.length; i < ln; i++) {
                if (el.options[i].value === v) {
                    el.selectedIndex = i;
                    Flag = true;
                    break;
                }
            }

            if (!Flag && tp.IsNumber(v) && tp.InRange(el.options, v)) {
                el.selectedIndex = v;
            }
        } else if (IsTextInput) {
            el.value = v;
        } else if (IsCheckInput) {
            el.checked = v;
        } else if (IsInput) {
            el.value = v;
        } else if (IsButton) {
            el.innerHTML = v;
        } else if ('textContent' in el) {
            el.textContent = v;
        } else {
            el.innerHTML = v;
        }

        return v;
    }
};
/**
Clears the value of an element. 
To be used mainly with input, select and textarea elements.
 @param   {string|Element}  el - A selector or an element
*/
tp.ClearValue = function (el) {
    el = tp.Select(el);
    if (!el) return;

    var i, ln;
    var NodeName = el.nodeName.toLowerCase();
    var IsInput = NodeName === 'input';
    var IsTextInput = NodeName === 'textarea' || IsInput && tp.ListContainsText(['text', 'password', 'button', 'submit', 'reset'], el.type);
    var IsCheckInput = IsInput && tp.ListContainsText(['check', 'radio'], el.type);
    var IsSelect = NodeName === 'select';
    var IsButton = NodeName === 'button';

    if (IsTextInput) {
        el.value = '';
    } else if (IsCheckInput) {
        el.checked = false;
    } else if (IsInput) {
        el.value = '';
    } else if (IsSelect) {
        el.selectedIndex = -1;
    } else if (IsButton) {
        el.innerHTML = '';
    } else if ('textContent' in el) {
        el.textContent = '';
    } else {
        el.innerHTML = '';
    }
};
/**
 Gets or sets the inner html of an element.  
 NOTE: Passing both arguments, sets the value of an element.
 @param   {string|Element}  el - A selector or an element
 @param   {string} [v=''] - The value to set to the element. If null then the function returns the element's value
 @returns  {string} If a value is not specified then the function returns the element's inner html, else returns the passed value.
 */
tp.Html = function (el, v = '') {
    el = tp.Select(el);

    if (el instanceof HTMLElement) {
        if (typeof v === 'string') {
            el.innerHTML = v;
        } else {
            return el.innerHTML;
        }
    }

    return '';
};

/**
Safely removes an HTMLElement from the DOM, that is from its parent node, if any.
@param {string|Node} ElementOrSelector The element to remove from DOM
*/
tp.Remove = function (ElementOrSelector) {
    let el = tp.Select(ElementOrSelector);
    if (tp.IsHTMLElement(el) && el.parentNode) {
        el.parentNode.removeChild(el);
    }
};
/**
Removes all child nodes/elements from a parent element.
@param {string|Node} ParentOrSelector The element to operate on
*/
tp.RemoveChildren = function (ParentOrSelector) {
    ParentOrSelector = tp.Select(ParentOrSelector);
    if (ParentOrSelector instanceof HTMLElement) {
        while (ParentOrSelector.firstChild) {
            ParentOrSelector.removeChild(ParentOrSelector.lastChild);
        }
    }
};

/**
Appends an element or html markup to a parent element as its last child. Returns the last node of the specified parent.
@param {string|Element} ParentOrSelector - Selector or element
@param {string|Element} ElementOrHtml - The element or the html markup text that should be added
@returns {Node} Returns the last node of the specified parent.
 */
tp.Append = function (ParentOrSelector, ElementOrHtml) {
    ParentOrSelector = tp.Select(ParentOrSelector);

    if (ParentOrSelector instanceof HTMLElement || ParentOrSelector instanceof Document) {
        var v = ElementOrHtml;
        if (tp.IsNode(v)) {
            return ParentOrSelector.appendChild(v);
        } else if (tp.IsString(v) && !tp.IsBlank(v)) {
            if ('insertAdjacentHTML' in ParentOrSelector) {
                ParentOrSelector.insertAdjacentHTML('beforeend', v);
            } else {
                ParentOrSelector.innerHTML += v;
            }

            return ParentOrSelector.childNodes[ParentOrSelector.childNodes.length - 1];
        }
    }

    return null;
};
/**
Inserts an element or html markup to a parent element as its first child. Returns the first node of the specified parent.
@param {string|Element}  ParentOrSelector - Selector or element
@param {string|Element}  ElementOrHtml - The element or the html markup text that should be added
@returns {Node} Returns the first node of the specified parent.
 */
tp.Prepend = function (ParentOrSelector, ElementOrHtml) {
    ParentOrSelector = tp.Select(ParentOrSelector);

    if (ParentOrSelector instanceof HTMLElement) {
        var v = ElementOrHtml;
        if (tp.IsNode(v)) {
            if (ParentOrSelector.childNodes.length === 0)
                return ParentOrSelector.appendChild(v);
            else
                return ParentOrSelector.insertBefore(v, ParentOrSelector.childNodes[0]);
        } else if (tp.IsString(v) && !tp.IsBlank(v)) {
            if ('insertAdjacentHTML' in ParentOrSelector) {
                ParentOrSelector.insertAdjacentHTML('afterbegin', v);
            } else {
                ParentOrSelector.innerHTML = v + ParentOrSelector.innerHTML;
            }

            return ParentOrSelector.childNodes[0];
        }
    }

    return null;
};

/**
Creates an element of a specified type, appends that element to a parent element, and returns the newly created element. 
@param {string|Element}  ParentOrSelector - Selector or element
@param {string} TagName - The node type, e.g. div, span, input etc.
@returns {Element} Returns the newly created element, or null if any of the passed arguments is not valid.
*/
tp.AppendElement = function (ParentOrSelector, TagName) {
    ParentOrSelector = tp.Select(ParentOrSelector);
    if (tp.IsNode(ParentOrSelector) && tp.IsString(TagName)) {
        var Result = ParentOrSelector.ownerDocument.createElement(TagName);
        ParentOrSelector.appendChild(Result);
        return Result;
    }
    return null;
};
/**
Creates an element of a specified type, insert that element to a parent element at a specified index, and returns the newly created element. 
@param {string|Element}  ParentOrSelector - Selector or element
@param {number} Index The index position, among child elements, where to place the newly create element
@param {string} TagName - The node type, e.g. div, span, input etc.
@returns {Element} Returns the newly created element, or null if any of the passed arguments is not valid.
*/
tp.InsertElement = function (ParentOrSelector, Index, TagName) {
    ParentOrSelector = tp.Select(ParentOrSelector);
    if (ParentOrSelector instanceof HTMLElement) {
        let Result = ParentOrSelector.ownerDocument.createElement(TagName);
        let List = tp.ChildHTMLElements(ParentOrSelector);
        if (List.length === 0 || Index === List.length - 1)
            ParentOrSelector.appendChild(Result);
        else
            ParentOrSelector.insertBefore(Result, List[Index]);
        return Result;
    }
    return null;
};

/**
Appends a specified node as the last node of a parent.
@param {string|Node} ParentOrSelector Selector or Node. The parent where to append the specified Node.
@param {Node} Node The Node to be appended
*/
tp.AppendNode = function (ParentOrSelector, Node) {
    ParentOrSelector = tp.Select(ParentOrSelector);
    if (tp.IsNode(ParentOrSelector)) {
        ParentOrSelector.appendChild(Node);
    }
};
/**
Inserts a specified node at a specified index in the child nodes of a parent.
@param {string|Node} ParentOrSelector Selector or Node. The parent where to append the specified Node.
@param {number} Index The index position, among child nodes, where to place the specified Node.
@param {Node} Node The Node to be inserted.
*/
tp.InsertNode = function (ParentOrSelector, Index, Node) {
    ParentOrSelector = tp.Select(ParentOrSelector);
    if (tp.IsNode(ParentOrSelector)) {
        let List = ParentOrSelector.childNodes;
        if (List.length === 0 || Index === List.length - 1)
            ParentOrSelector.appendChild(Node);
        else
            ParentOrSelector.insertBefore(Node, List[Index]);
    }
};

/**
 * Adds a list of items as options to a list control (HTMLSelectElement).
 * This function accepts an optional callback parameter to be called on every item as <code> function(Item) { Text: , Value: }</code> <br />
 * When the callback returns an object of <pre>{ Text: , Value: }</pre> this function adds the option element to the list control. <br />
 * When the callback returns null/undefined, then the callback should add the option element.
 * @param {string|HTMLSelectElement} ListControl Selector or HTMLSelectElement
 * @param {object[]} List The list of items to add as options. Could be a list of <code>{ Text: , Value: }</code> items. In such a case no call-back is needed.
 * @param {object} [FirstItem=null] Optional. Defaults to null. When not null then it is added as the first option to the element. Must be a  <pre>{ Text: , Value: }</pre> object.
 * @param {string} [SelectedValue=null] Optional. The combobox selected value.
 * @param {function} [ItemFunc=null] Optional. A callback to be called on every item of <code> function(Item) { Text: , Value: }</code> <br />
 */
tp.AddOptionList = function (ListControl, List, FirstItem = null, SelectedValue = null, ItemFunc = null) {

    ListControl = tp(ListControl);

    let i, ln, Item, Data;

    ListControl.options.length = 0;

    function AddOption(Text, Value) {
        let o = ListControl.ownerDocument.createElement('option');
        o.text = Text;
        o.value = Value;
        ListControl.add(o);
    }

    if (tp.IsValid(FirstItem)) {
        AddOption(FirstItem.Text, FirstItem.Value);
    }

    for (i = 0, ln = List.length; i < ln; i++) {
        Item = List[i];
        if (tp.IsFunction(ItemFunc)) {
            Data = tp.Call(ItemFunc, null, Item);
            if (tp.IsValid(Data))
                AddOption(Data.Text, Data.Value);
        }
        else {
            AddOption(Item.Text, Item.Value);
        }
    }

    if (tp.IsValid(SelectedValue)) {
        ListControl.value = SelectedValue;
    }
};

/**
Sets multiple mutliple attributes of an element, at once, based on a specified object
@example
tp.SetAttributes(el, { id: 'img0', src: 'image.jpg' );   // set multiple attributes at once
@param {string|Element}  el -  Selector or element
@param {object} o - An object with key/value pairs where key may be a string
*/
tp.SetAttributes = function (el, o) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el) && tp.IsPlainObject(o)) {
        for (var Prop in o) {
            if (!tp.IsFunction(o[Prop]))
                tp.Attribute(el, Prop, o[Prop]);
        }
    }
};
/**
Gets or sets the value of an attribute
NOTE: Passing both arguments, sets the value.
@example
// get
var v = tp.Attribute(el, 'id');
@example
// set a single attribute
tp.Attribute(el, 'id', 'div0');
@param {string|Element} el Selector or element
@param {string} Name Denotes the attribute name.  
@param {object} [v=null] The value of the attribute to set.
@returns {any} When acts as a get then it returns the value of the attribute. When acts as a set, it always returns null.
 */
tp.Attribute = function (el, Name, v = null) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el)) {

        if (tp.IsString(Name)) {

            if (tp.IsEmpty(v)) {                        // get
                return el.getAttribute(Name);
            } else {                                    // set  
                if (Name in el.style) {
                    el[Name] = v;
                } else {
                    el.setAttribute(Name, v);
                }

                return v;
            }

        }

    }

    return null;
};
/**
Removes an attribute from an element.
@param {string|Element} el Selector or element
@param {string} Name Denotes the attribute name.
*/
tp.RemoveAttribute = function (el, Name) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el) && el.hasAttribute(Name))
        el.removeAttribute(Name);
};
/**
Returns true if a specified element has an attribute
@param {string|Element} el Selector or element
@param {string} Name Denotes the attribute name.
@returns {boolean} Returns true if a specified element has an attribute
*/
tp.HasAttribute = function (el, Name) {
    el = tp.Select(el);
    return tp.IsHTMLElement(el) && el.hasAttribute(Name);
};

/**
Sets multiple properties of the style property of an element, at once, based on a specified object
@example
tp.SetStyle(el, {'width': '100px', height: '100px', 'background-color', 'yellow', backgroundColor: 'red' });   // set multiple style properties at once
@param {string|Element} el Selector or element
@param {object} o - An object with key/value pairs where key may be a string
*/
tp.SetStyle = function (el, o) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el) && tp.IsPlainObject(o)) {
        for (var Prop in o) {
            if (!tp.IsFunction(o[Prop]))
                tp.StyleProp(el, Prop, o[Prop]);
        }
    }
};
/**
Gets or sets the value of a style property    
NOTE: Passing both arguments, sets the value.
@example
// get
var v = tp.StyleProp(el, 'width')
@example
// set a single style property
tp.StyleProp(el, 'width', '100px');
@param {string|Element} el Selector or element
@param {string} Name Denotes the property name 
@param {oject} [v=null] The value of the property to set.
@returns {any} When acts as a get then it returns the value of the property. When acts as a set, it always returns null.
 */
tp.StyleProp = function (el, Name, v = null) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el) && tp.IsString(Name)) {
        if (tp.IsEmpty(v)) {                        // get
            var Style = tp.GetComputedStyle(el);
            return Style.getPropertyValue(Name);
        } else {                                    // set  
            if (Name in el.style) {
                el.style[Name] = v;
            } else {
                el.style.setProperty(Name, v, null);
            }
            return v;
        }
    }

    return null;

};
/**
 Gets or sets the css style text of an element. 
 @param {string|Element}  el - A selector or an element
 @param {string} [v=''] - The value to set to the element. If null or empty then the function returns the element's value
 @returns {string} When acts as a get then it returns the the css style text of an element. When acts as a set, it always returns empty string.
*/
tp.StyleText = function (el, v = '') {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el)) {
        if (tp.IsBlank(v))
            return el.style.cssText;
        else
            el.style.cssText = v;
    }

    return '';
};
/**
Returns the currently active style of an element.
@param {string|Element} el - A selector or an element
@returns {CSSStyleDeclaration} - Returns the style object property of an element, which updates itself automatically when the element's style is changed.
*/
tp.GetComputedStyle = function (el) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el))
        return el.ownerDocument.defaultView.getComputedStyle(el, '');

    return null;
};

/**
Gets or sets the value of a data-* attribute or sets the values of multiple data-* attributes. 
To get the value of a data-* attribute pass 1. the element and 2. the attribute name.
To set the value of a single data-* attribute pass 1. the element 2. the attribute name and 3. the value.
To set the value of multiple data-* attributes pass 1. the element and 2. a plain object.
@example
// get
var v = tp.Data(el, 'field');

@example
// set a single data-* attribute
tp.Data(el, 'field', 'UserName');

// set multiple data-* attributes at once
tp.Data(el, {'field': 'UserName', level: 'guest', points: '456', 'color', 'yellow', index: '0' });
 
@param {HTMLElement|string} el Selector or element
@param {string|object} o When string denotes the attribute name. Else it's an object with key/value pairs where key may be a string
@param {object} [v=null] The value to set.
@returns {string} When acts as a get then it returns the value of the data-* attribute. When acts as a set, it always returns empty string.
 */
tp.Data = function (el, o, v = null) {

    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        if (tp.IsEmpty(v) && tp.IsString(o)) {        // get
            return el.getAttribute('data-' + o);
        } else {
            if (tp.IsString(o)) {                    // set  
                el.setAttribute('data-' + o, v);
            } else if (tp.IsPlainObject(o)) {
                for (var Prop in o) {
                    if (!tp.IsFunction(o[Prop]))
                        el.setAttribute('data-' + Prop, o[Prop]);
                }
            }
        }
    }

    return '';

};

/**
 * Converts a string into a class (i.e. function), e.g. 'tp.DataView'. <br />
 * Returns the class on success. Returns the passed in string on failure. <br />
 * A flag controls whether to throw an exception on failure.
 * @param {string} S The string to operate on
 * @param {boolean} [ThrowIfNot=false] Optional. Defaults to false. If true then it throws an exception on failure.
 */
tp.StrToClass = function (S, ThrowIfNot = false) {
    let Result = S;

    if (tp.IsString(S)) {

        // here we convert a string to a class.
        // for this to succeed the class should be exist in a javascript file, otherwise we get undefined.
        let o = eval(S);

        // on success assign the constructor to result, otherwise leave it a string (as it was)
        if (tp.IsFunction(o))
            Result = o;
    }

    if (!tp.IsFunction(Result) && ThrowIfNot === true)
        tp.Throw(`Cannot create a class out of ${S}`);

    return Result;
};

/**
 * Returns the value of the data-setup attribute of a specified element if any, else empty string.
 * @param {HTMLElement|string} el The element to operate on.
 */
tp.GetDataSetup = function (el) { return tp.Data(el, 'setup'); };
/**
 * Returns the data setup script object associated to an element, if any, else null. <br />
 * This function returns the __DataSetup script object, if already exists as a property to the specified element. <br />
 * Else tries to get the value of the data-setup attribute, if exists, and creates the object.  <br /> 
 * It also merges any properties found in the {@link tp.GlobalCreateParams} for that element with the object. <br />
 * Finally associates the object to the element and returns the object. <br />
 * Returns null if no attribute exists on element.
 * @param {HTMLElement|string} el The element to operate on.
 * @returns {object} Returns the javascript object created using the data-setup attribute, or null, if no attribute exists on element.
 */
tp.GetDataSetupObject = function (el) {
    el = tp(el);

    // return the object if already there
    if ('__DataSetup' in el)
        return el['__DataSetup'];

    // no object, so get the data-setup attribute, if exists, and create the object
    let Result = null;
    let S = tp.GetDataSetup(el);
    if (tp.IsString(S) && !tp.IsBlank(S)) {
        Result = eval("(" + S + ")");

        if ('ClassType' in Result)
            Result.ClassType = tp.StrToClass(Result['ClassType']); 

        // options placed in tp.CreateParams for this element 
        if (tp.IsString(el.id) && !tp.IsBlank(el.id)) {
            if (el.id in tp.GlobalCreateParams) {
                let o = tp.GlobalCreateParams[el.id];

                tp.MergeQuick(Result, o);
            }
        }

        el['__DataSetup'] = Result;
    }

    return Result;
};
 


/**
Gets or sets the value of a data-role attribute of an element  
NOTE: Passing both arguments, sets the value
@param {string|Element} el Selector or element
@param {string} [v=null] The value to set.
@returns {string} When acts as a get then it returns the value of the data-role attribute. When acts as a set, it always returns empty string.
*/
tp.Role = function (el, v = null) { return tp.Data(el, 'role', v); };
/**
Gets or sets the value of a data-field attribute of an element  
NOTE: Passing both arguments, sets the value
@param {string|Element} el Selector or element
@param {string} [v=null] The value to set.
@returns {string} When acts as a get then it returns the value of the data-field attribute. When acts as a set, it always returns empty string.
*/
tp.Field = function (el, v = null) { return tp.Data(el, 'field', v); };
/**
Gets or sets the value of the display style property of an element  
NOTE: Passing both arguments, sets the value.
@param {string|Element} el Selector or element
@param {string} [v=null] The value to set.
@returns {string} When acts as a get then it returns the value. When acts as a set, it always returns empty string.
*/
tp.Display = function (el, v = null) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        if (tp.IsEmpty(v)) {        // get
            return tp.GetComputedStyle(el).display;
        } else {                    // set
            el.style.display = v;
            return v;
        }
    }

    return '';
};

/**
 * Type guard function. Returns true if a specified element provides a disabled property.
 * @param {string|Element} el Selector or element
 * @returns {boolean} Returns true if a specified element provides a disabled property.
 */
tp.HasDisabledProperty = function (el) {
    el = tp.Select(el);
    return el instanceof HTMLButtonElement
        || el instanceof HTMLInputElement
        || el instanceof HTMLTextAreaElement
        || el instanceof HTMLSelectElement
        || el instanceof HTMLOptionElement
        || el instanceof HTMLOptGroupElement
        || el instanceof HTMLFieldSetElement
        || el instanceof HTMLLinkElement
        ;
};


/**
Enables or disables an element by setting a proper value to the disabled attribute.  
NOTE: For the function to act as a get, just let the last argument unspecified.  
@param {string|Element} el Selector or element
@param {boolean} [v=null] True enables the element, false disables the element. Leave it unspecified to get the current state.
@returns {boolean} When acts as a get then it returns a value indicating whether the element is enabled.  
*/
tp.Enabled = function (el, v = null) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        if (tp.IsEmpty(v)) {
            if (tp.HasDisabledProperty(el))
                return !el.disabled;

            return !tp.HasClass(el, 'tp-Disabled');
        } else {
            if (tp.HasDisabledProperty(el)) {
                el.disabled = v === false;
                return el.disabled;
            } else {
                if (v === true) {
                    tp.RemoveClass(el, 'tp-Disabled');
                } else {
                    tp.AddClass(el, 'tp-Disabled');
                }

                return v;
            }
        }
    }
};
/**
Get or sets the visibility state of an element according to a specified flag, by settting a proper value to the visibility style property 
NOTE: For the function to act as a get, just let the last argument unspecified. 
@param {string|Element} el Selector or element
@param {boolean} [v=null] True shows the element where false hides the element. Let it unspecified to get the current state.
@returns {boolean} When acts as a get then it returns a value indicating whether the element is visible. When acts as a set, it always returns false.
*/
tp.Visibility = function (el, v = null) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        if (tp.IsEmpty(v)) {        // get
            return tp.IsSameText(tp.StyleProp(el, 'visibility'), 'visible');
        } else {                    // set
            el.style.visibility = v ? "visible" : "hidden";
            return v;
        }
    }

    return true;
};
/**
Shows or hides a specified element by setting the display CSS property.
Gets or sets the display CSS property of an element.
NOTE: When setting it uses the inline style display property. It sets display to none (for non-visible) or empty string (for visible).
@param {string|Element} el Selector or element
@param {boolean} [v=null] True shows the element where false hides the element. Let it unspecified to get the current state.
@returns {boolean} When acts as a get then it returns a value indicating whether the element is visible. When acts as a set, it always returns false.
*/
tp.Visible = function (el, v = null) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        if (tp.IsEmpty(v)) {        // get
            return tp.GetComputedStyle(el).display !== 'none';
        } else {                    // set
            el.style.display = v === true ? '' : 'none';
        }
    }

    return v;
};
/**
Gets or sets the readonly attribute of an input or textarea element.
@param {string|Element} el Selector or element
@param {boolean} [v=null] True sets the element to read-only state. Let it unspecified to get the current state.
@returns {boolean} Returns true if the element is read-only.
*/
tp.ReadOnly = function (el, v = null) {
    el = tp.Select(el);

    if (el instanceof HTMLInputElement || el instanceof HTMLTextAreaElement) {
        if (tp.IsEmpty(v)) { // get
            return el.readOnly;
        } else {
            el.readOnly = v === true;
        }
    }

    return v === true;
};



/**
Returns true if an element has a specified css class.
@param {string|Element} el  Selector or element
@param {string} Name - The css class name
@returns {boolean} Returns true if an element has a specified css class.
*/
tp.HasClass = function (el, Name) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        return tp.IsBlank(Name) ? false : el.classList.contains(Name);
    }

    return false;
};
/**
Adds a specified css class to an element, if not already there.
@param {string|Element}  el   Selector or element
@param {string} Name - The css class name
*/
tp.AddClass = function (el, Name) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el) && !tp.IsBlank(Name) && !el.classList.contains(Name)) {
        el.classList.add(Name);
    }
};
/**
Removes a specified css class from an element.
@param {string|Element}  el   Selector or element
@param {string} Name - The css class name
*/
tp.RemoveClass = function (el, Name) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el) && !tp.IsBlank(Name) && el.classList.contains(Name)) {
        el.classList.remove(Name);
    }
};
/**
Toggles a specified css class from an element, i.e. adds the class if not there, removes the class if there.
@param {string|Element}  el   Selector or element
@param {string} Name - The css class name
*/
tp.ToggleClass = function (el, Name) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el) && !tp.IsBlank(Name)) {
        el.classList.toggle(Name);
    }
};
/**
Adds one or more css classes to an element.
@param {string|Element}  el   Selector or element
@param {string[]} Names - Rest parameter. One or more css class names.
*/
tp.AddClasses = function (el, ...Names) {

    el = tp.Select(el);

    let i, ln;
    if (tp.IsHTMLElement(el)) {
        if (tp.IsArray(Names)) {
            if (Names.length === 1) {
                let S = Names[0];
                let Parts = S.split(' ');
                for (i = 0, ln = Parts.length; i < ln; i++) {
                    if (!tp.IsBlank(Parts[i]))
                        tp.AddClass(el, Parts[i]);
                }
            } else {
                for (i = 0, ln = Names.length; i < ln; i++) {
                    if (!tp.IsBlank(Names[i]))
                        tp.AddClass(el, Names[i]);
                }
            }
        }
    }
};
/**
Removes one or more css classes from an element.
@param {string|Element} el Selector or element
@param {string[]} Names - Rest parameter. One or more css class names.
*/
tp.RemoveClasses = function (el, ...Names) {
    el = tp.Select(el);

    let i, ln;
    if (tp.IsHTMLElement(el)) {
        if (tp.IsArray(Names)) {
            if (Names.length === 1) {
                let S = Names[0];
                if (!tp.IsBlank(S)) {
                    var Parts = S.split(' ');
                    for (i = 0, ln = Parts.length; i < ln; i++) {
                        if (!tp.IsBlank(Parts[i]))
                            tp.RemoveClass(el, Parts[i]);
                    }
                }

            } else {
                for (i = 0, ln = Names.length; i < ln; i++) {
                    if (!tp.IsBlank(Names[i]))
                        tp.RemoveClass(el, Names[i]);
                }
            }
        }
    }

};
/**
Clears all csss classes from an element
@param {string|Element} el Selector or element
*/
tp.ClearClasses = function (el) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el))
        el.className = '';
};
/**
Concatenates css class names into a single string.
Each argument could be just a single class name, or more names space separated
@param {string[]} Names - Rest parameter. One or more css class names.
@returns {string} A string with class names delimited with spaces.
*/
tp.ConcatClasses = function (...Names) {

    var A = [];
    var Parts = null;
    var S, i, ln;

    for (i = 0, ln = Names.length; i < ln; i++) {
        S = Names[i];
        if (!tp.IsBlank(S)) {
            Parts = tp.Split(S, ' ', true);
            A = A.concat(Parts);
        }
    }

    return A.join(' ');
};

/**
Returns true if a specified element is the focused element in the document
@param {string|Element} el Selector or element
@returns {boolean} Returns true if a specified element is the focused element in the document
*/
tp.IsFocused = function (el) {
    el = tp.Select(el);
    return tp.IsHTMLElement(el) && tp.Doc.activeElement === el;
};
/**
Returns true if a specified element is the focused element in the document OR contains the focused element
@param {string|Element} el Selector or element
@returns {boolean} Returns true if a specified element is the focused element in the document OR contains the focused element
*/
tp.HasFocused = function (el) {
    el = tp.Select(el);
    return tp.IsHTMLElement(el) && (tp.Doc.activeElement === el || tp.ContainsElement(el, tp.Doc.activeElement));
};
/**
Gets or sets the tabIndex attribute of a specified element. Returns the tabIndex or NaN if the specified element is not a HTMLElement.
Tab index   < 0         - can be focused either by a mouse click, or the focus() function
Tab index   >= 0        - can be focused either by a mouse click, or the focus() function, or the tab key, according to its tab-order
Tab index   unspecifed  - IE sets it to 0 and can be focused either by a mouse click, or the focus() function
                          Chrome and Firefox set it to -1 and it can NOT be focused at all

Clicking on a nested div (element) with unspecified tab-index
    IE gives focus always to the child
    Chrome and Firefox give focus only to the parent, and only if the parent has its tab-index specified explicitly

Conclusion:
    It seems that setting tab-index explicitly to -1 is the best choice when the purpose is to control the focus
    either by mouse clicks or by the focus() function
@see {@link https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/tabindex|mdn}
@see {@link https://developer.mozilla.org/en-US/docs/Web/Accessibility/Keyboard-navigable_JavaScript_widgets|mdn: Keyboar navigatable widgets}
@param {string|Element} el Selector or element
@param {number} [v=null] - When specified then the function is a setter, else it is a getter
@returns {number} Returns the tabIndex or NaN if the specified element is not a HTMLElement.
*/
tp.TabIndex = function (el, v = null) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el)) {
        if (tp.IsNumber(v)) {
            el.tabIndex = v;
            return v;
        } else {
            return el.tabIndex;
        }
    }

    return NaN;
};

/**
Returns a point with the location of an element, relative to the Top/Left of the fully rendered page (document)
@param {string|Element} el Selector or element
@returns {tp.Point} Returns the location of the element, i.e { X: number, Y: number }.
*/
tp.ToDocument = function (el) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el)) {
        var R = el.getBoundingClientRect();
        var po = tp.Viewport.GetPageOffset();
        var clientPoint = tp.ToViewport(el);

        var X = Math.round(R.left + po.X - clientPoint.X);
        var Y = Math.round(R.top + po.Y - clientPoint.Y);
    }

    return new tp.Point(0, 0);
};
/**
Returns a point with the location of an element, relative to the Top/Left of the browser window (viewport)
@param {string|Element} el Selector or element
@returns {tp.Point} Returns the location of the element, i.e { X: number, Y: number }.
*/
tp.ToViewport = function (el) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        var R = el.getBoundingClientRect();
        var X = Math.round(R.left);
        var Y = Math.round(R.top);
        return new tp.Point(X, Y);
    }

    return new tp.Point(0, 0);
};
/**
Returns a point with the location of a block (NOT inline) element, relative to the Top/Left of its immediate parent element
@param {string|Element} el Selector or element
@returns {tp.Point} Returns the location of the element, i.e { X: number, Y: number }.
*/
tp.ToParent = function (el) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        var X = Math.round(el.offsetLeft);
        var Y = Math.round(el.offsetTop);
        return new tp.Point(X, Y);
    }

    return new tp.Point(0, 0);
};

/**
Returns the size of an element. <br />
It also measures the size of an {@link HTMLElement} without a parent.
If the argument is an {@link HTMLElement} without a parent, then the function temporarily adds a DIV to the document and the element to that DIV, and then measures its size. 
@param {string|Element} el Selector or element
@returns {tp.Size} Returns the size of the element, i.e { Width: number, Height: number }.
*/
tp.SizeOf = function (el) {
    var Result = new tp.Size(0, 0);

    if (el) {
        let DIV = null;

        if (tp.IsString(el)) {
            el = tp.Select(el);
        }

        if (tp.IsHTMLElement(el)) {

            if (tp.IsEmpty(el.parentElement)) {
                DIV = el.ownerDocument.createElement("div");
                el.ownerDocument.body.appendChild(DIV);
                DIV.appendChild(el);
            }

            var R = el.getBoundingClientRect();
            var W = Math.round(R.width);
            var H = Math.round(R.height);

            Result = new tp.Size(W, H);

            if (DIV) {
                DIV.removeChild(el);
                DIV.parentElement.removeChild(DIV);
            }
        }
    }


    return Result;

    /*
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        var R = el.getBoundingClientRect();
        var W = Math.round(R.width);
        var H = Math.round(R.height);
        return new tp.Size(W, H);
    }
    return new tp.Size(0, 0);
     */

};
/**
Returns the rectangle of an element in viewport, that is relative to the Top/Left of the viewport (uses the getBoundingClientRect).
@param {string|Element} el Selector or element
@returns {tp.Rect} Returns the rectangle of the element, i.e { X: number, Y: number, Width: number, Height: number }.
*/
tp.BoundingRect = function (el) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        var R = el.getBoundingClientRect();
        var X = Math.round(R.left);
        var Y = Math.round(R.top);
        var W = Math.round(R.width);
        var H = Math.round(R.height);
        return new tp.Rect(X, Y, W, H);
    }

    return new tp.Rect(0, 0, 0, 0);
};
/**
Returns the rectangle of an element relative to the Top/Left of its parent element.
@param {string|Element} el Selector or element
@returns {tp.Rect} Returns the rectangle of the element, i.e { X: number, Y: number, Width: number, Height: number }.
*/
tp.OffsetRect = function (el) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        var Pos = tp.ToParent(el);
        var Size = tp.SizeOf(el);

        var X = Pos.X;
        var Y = Pos.Y;
        var W = Size.Width;
        var H = Size.Height;

        return new tp.Rect(X, Y, W, H);
    }

    return new tp.Rect(0, 0, 0, 0);

};
/**
Returns the height of a "line" or "row" based on font size  of an element and a user provider multiplication Factor
@param {string|Element} el Selector or element. The element to be used for the calculation
@param {number} [Factor=1.8] - A multiplication factor (FontSize * Factor)
@returns {number} Returns the height of a line/row
*/
tp.GetLineHeight = function (el, Factor = 1.8) {

    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        Factor = Factor || 1.8;

        var FontSize = tp.StyleProp(el, 'font-size');

        if (tp.IsEm(FontSize)) {
            var FontSize2 = tp.StyleProp(el.ownerDocument.body, 'font-size');
            if (!tp.IsPixel(FontSize2))
                tp.Throw('document.body font-size is NOT defined in pixels');
            FontSize2 = tp.ExtractNumber(FontSize2);
            FontSize = FontSize * FontSize2;
        } else {
            FontSize = tp.ExtractNumber(FontSize);
        }

        var Result = Math.ceil(FontSize * Factor);

        return Result > 0 ? Result : 24;
    }

    return 0;
};


/**
Gets or sets the z-index of an element.  
NOTE: Passing both arguments, sets the value.
@see {@link http://philipwalton.com/articles/what-no-one-told-you-about-z-index/|article}
@see {@link http://www.w3.org/TR/CSS2/zindex.html|w3.org}
@param {string|Element} el Selector or element
@param {string|number} [v=null] - A numberic string or a number
@returns {number} When acts as a get, then returns the numeric value of the z-index of an element, else 0
*/
tp.ZIndex = function (el, v = null) {
    /* z-Index - integer 
        see:    http://philipwalton.com/articles/what-no-one-told-you-about-z-index/
        http://www.w3.org/TR/CSS2/zindex.html        */

    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        if (tp.IsEmpty(v)) {
            var Result = tp.GetComputedStyle(el).zIndex;
            return tp.StrToInt(Result, 0);
            //return Number(isNaN(Result) ? '0' : Result);
        } else {
            el.style.zIndex = v.toString();
        }
    }

    return 0;

};
/**
Returns the max z-index in a container element.
@param {string|Element} [Container=null] - Optional. Selector or element or null. Defaults to document.
@returns {number} Returns the maximum z-index.
*/
tp.MaxZIndexOf = function (Container = null) {
    Container = tp.IsString(Container) ? tp.Select(Container) : Container || document;

    var Result, List, el, i, ln, v;

    Result = 0;

    List = Container.querySelectorAll('*');
    ln = List.length;

    for (i = 0; i < ln; i++) {
        el = List[i];
        v = el.ownerDocument.defaultView.getComputedStyle(el, '').getPropertyValue('z-index');
        if (v === 'auto') {
            v = i;
        }

        v = tp.ExtractNumber(v);
        Result = Math.max(Result, v);
    }

    return Result;
};
/**
Returns the min z-index in a container element.
@param {string|Element} [Container=null] - Optional. Selector or element or null. Defaults to document.
@returns {number} Returns the minimum z-index.
*/
tp.MinZIndexOf = function (Container = null) {
    Container = tp.IsString(Container) ? tp.Select(Container) : Container || document;

    var Result, List, el, i, ln, v;

    Result = 0;

    List = Container.querySelectorAll('*');
    ln = List.length;

    for (i = 0; i < ln; i++) {
        el = List[i];
        v = el.ownerDocument.defaultView.getComputedStyle(el, '').getPropertyValue('z-index');
        if (v === 'auto') {
            v = i;
        }
        if (v && tp.IsNumber(v)) {
            if (v < Result) {
                Result = v;
            }
        }
    }

    return Result;
};
/**
Brings an element in front of all of its siblings (child elements in the same parent element).  
Returns the z-index of the element after the placement.
@param {string|Element} el - Selector or element
@returns {number} Returns the z-index of the element after the placement.
*/
tp.BringToFront = function (el) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el)) {
        let v = tp.MaxZIndexOf(el.parentNode);
        let v2 = tp.ZIndex(el);
        if (v2 < v) {
            v++;
            el.style.zIndex = v.toString();
        } else {
            v = v2;
        }

        return v;
    }
    return 0;
};
/**
Sends an element to back of all of its siblings (child elements in the same parent element).  
Returns the z-index of the element after the placement.
@param {string|Element} el - Selector or element
@returns {number} Returns the z-index of the element after the placement.
*/
tp.SendToBack = function (el) {
    el = tp.Select(el);

    if (tp.IsHTMLElement(el)) {
        if (!el) return null;

        var v = tp.MinZIndexOf(el.parentNode);
        v--;
        el.style.zIndex = v.toString();
        return v;
    }

    return 0;
};


/**
Sets the start and end positions of the current text selection in an input (mostly textbox) element
@param {string|Element} el - Selector or element. The input (textbox) element
@param {number} Start - The index of the first selected character
@param {number} End - The index of the character after the last selected character
*/
tp.TextBoxSelectText = function (el, Start, End) {
    el = tp.Select(el);

    if (el) {

        if (tp.IsEmpty(End)) {
            End = el.value.length;
        }

        if ('setSelectionRange' in el) {
            el.setSelectionRange(Start, End);
        } else if ('createTextRange' in el) {
            var Range = el.createTextRange();
            Range.moveStart("character", Start);
            Range.moveEnd("character", -el.value.length + End);
            Range.select();
        }

        //el.focus();
    }

};
/**
Replaces the selected text in an input (textbox) element with a specified text.
@param {string|Element} el - Selector or element. The input (textbox) element.
@param {string} Text - The text to use in replacing the currently selected text in the element.
*/
tp.TextBoxReplaceSelectedText = function (el, Text) {
    el = tp.Select(el);
    if (el) {
        if ('setSelectionRange' in el) {
            var Start = el.selectionStart;
            el.value = el.value.substring(0, Start) + Text + el.value.substring(el.selectionEnd, el.value.length);
            el.setSelectionRange(Start + Text.length, Start + Text.length);
        } else if ('selection' in el.ownerDocument) {
            var Range = el.ownerDocument.selection.createRange();
            Range.text = Text;
            Range.collapse(true);
            Range.select();
        }

        //el.focus();
    }

};

/**
Selects the text of an element
@see {@link http://stackoverflow.com/questions/985272/selecting-text-in-an-element-akin-to-highlighting-with-your-mouse|stackoverflow}
@param {string|Element} el - Selector or element.
*/
tp.ElementSelectText = function (el) {
    el = tp.Select(el);

    if (el) {
        var win = el.ownerDocument.defaultView; //win || window;
        var doc = win.document, sel, range;
        if (win.getSelection && doc.createRange) {
            sel = win.getSelection();
            range = doc.createRange();
            range.selectNodeContents(el);
            sel.removeAllRanges();
            sel.addRange(range);
        } else if (doc.body.createTextRange) {
            range = doc.body.createTextRange();
            range.moveToElementText(el);
            range.select();
        }
    }
};


/**
Turns a block element into a flex container panel that centers its childrent both in the x and y axis
@param {string|Element} el - Selector or element
*/
tp.MakeCenterChildren = function (el) {
    el = tp.Select(el);
    if (tp.IsHTMLElement(el)) {
        var S = "position: relative; height:100%; width:100%; display: flex; justify-content: center; align-items: center; flex-wrap: wrap;";
        el.style.cssText = S;
    }
};
/**
Removes the border from an HTMLIFrameElement
@see {@link http://stackoverflow.com/questions/1516803/how-to-remove-border-from-iframe-in-ie-using-javascript|stackoverflow}
@param {string|Element} el - Selector or element
*/
tp.FrameRemoveBorder = function (el) {
    el = tp.Select(el);
    if (el instanceof HTMLIFrameElement) {
        el.frameBorder = '0';
        el.setAttribute('frameBorder', "0"); //  For other browsers (just a fall-back for the above).
        //el.contentWindow.document.body.style.border = "none";   //  For IE.
    }
};

/**
A full static class for comparing the position of a node against another node.
@class
@static
*/
tp.DocumentPosition = {

    /** Elements are identical. */
    Identical: 0,
    /** The nodes are in different documents (or one is outside of a document). */
    Disconnected: 1,
    /** B is before A (could be its ancestor too, though) */
    Preceding: 2,
    /** A is before B (could be its ancestor too, though)  */
    Following: 4,
    /** Ancestor */
    Ancestor: 8,
    /** Descendant */
    Descendant: 16,
    /** For private use by the browser. */
    ImplementationSpecific: 32,

    /**
    Compares the position of a node against another node in any other document and returns a bit-mask. 
    Return values 
        tp.DocumentPosition.Identical = 0;                // Elements are identical.
        tp.DocumentPosition.Disconnected = 1;             // The nodes are in different documents (or one is outside of a document).
        tp.DocumentPosition.Preceding = 2;                // B is before A (could be its ancestor too, though)
        tp.DocumentPosition.Following = 4;                // A is before B (could be its ancestor too, though)
        tp.DocumentPosition.Ancestor = 8;                 //
        tp.DocumentPosition.Descendant = 16;              //
        tp.DocumentPosition.ImplementationSpecific = 32;  // For private use by the browser.
    @see The <a href="https://developer.mozilla.org/en-US/docs/Web/API/Node/compareDocumentPosition">Node.compareDocumentPosition() in MDN</a>.
    @see <a href="http://ejohn.org/blog/comparing-document-position/">John Resig blog</a>

    @param {Node} elA The element to be used as base.
    @param {Node} elB The element to compare against base element.
    @returns {number} The return value is a bitmask
    @static
    */
    Compare: function (elA, elB) {
        return elA.compareDocumentPosition(elB);
    },

    /* methods */
    /** True if A contains B  
    @param {Node} elA The element to be used as base.
    @param {Node} elB The element to compare against base element.
    @returns {boolean} True if A contains B
    @static
    */
    Contains: function (elA, elB) {
        var Res = tp.DocumentPosition.Compare(elA, elB);
        return tp.Bf.In(tp.DocumentPosition.Ancestor, Res);
    },
    /** True if B contains A 
    @param {Node} elA The element to be used as base.
    @param {Node} elB The element to compare against base element.
    @returns {boolean} True if B contains A
    @static
    */
    ContainedBy: function (elA, elB) {
        var Res = tp.DocumentPosition.Compare(elA, elB);
        return tp.Bf.In(tp.DocumentPosition.Descendant, Res);
    },
    /** True if B is before A (but B could be an ancestor of A at the same time too, though)
    @param {Node} elA The element to be used as base.
    @param {Node} elB The element to compare against base element.
    @returns {boolean}  True if B is before A (but B could be an ancestor of A at the same time too, though)
    @static
    */
    IsBefore: function (elA, elB) {
        var Res = tp.DocumentPosition.Compare(elA, elB);
        return tp.Bf.In(tp.DocumentPosition.Preceding, Res);
    },
    /** True if A is before B (but A could be an ancestor of B at the same time too, though)
    @param {Node} elA The element to be used as base.
    @param {Node} elB The element to compare against base element.
    @returns {boolean} True if A is before B (but A could be an ancestor of B at the same time too, though)
    @static
    */
    IsAfter: function (elA, elB) {
        var Res = tp.DocumentPosition.Compare(elA, elB);
        return tp.Bf.In(tp.DocumentPosition.Following, Res);
    }
};


/**
 * Alignment helper.
 * @class
 * @static
 * @hideconstructor
 * */
tp.Alignment = {

    /** Near
     @const 
     @returns {number} 1
     */
    Near: 1,
    /** Mid
     @const
     @returns {number} 2
     */
    Mid: 2,
    /** Far
     @const
     @returns {number} 4
     */
    Far: 4,

    /** 
     @readonly
     @returns {tp.Alignment.Mid} -
     */
    get Justify() { return tp.Alignment.Mid; },

    /** Top
    @readonly
    @returns {tp.Alignment.Near} -
    */
    get Top() { return tp.Alignment.Near; },
    /** Center
    @readonly
    @returns {tp.Alignment.Mid} -
    */
    get Center() { return tp.Alignment.Mid; },
    /** Bottom
     @readonly
    @returns {Alignment.Far} -
    */
    get Bottom() { return tp.Alignment.Far; },

    /** Left
    @readonly
    @returns {tp.Alignment.Near} -
    */
    get Left() { return tp.Alignment.Near; },
    /** Right
    @readonly
    @returns {tp.Alignment.Far} -
    */
    get Right() { return tp.Alignment.Far; },

    /**
    Converts n <code>tp.Alignment</code> constant value and returns <code>flex</code> property (<code>justify-content</code>, <code>align-items</code> or <code>align-content</code>) value. <br />
    Returns <br />
    <ul>
        <li><code>flex-start</code> for <code>tp.Alignment.Near</code>  </li>
        <li><code>flex-end</code> for <code>tp.Alignment.Far</code> </li>
        <li><code>center</code> in all other cases. </li>
    </ul>

    @param {number} v - One of the <code>tp.Alignment</code> constant values
    @param {boolean} [Reverse=false] - Defaults to false. True makes the function to return the opposite value, e.g. <code>flex-end</code> instead of <code>flex-start</code>.
    @returns {string} A <code>flex</code> property (<code>justify-content</code>, <code>align-items</code> or <code>align-content</code>) value.
    */
    ToFlex: function (v, Reverse = false) {
        if (v === this.Near) {
            return Reverse === true ? 'flex-end' : 'flex-start';
        } else if (v === this.Far) {
            return Reverse === true ? 'flex-start' : 'flex-end';
        } else {
            return 'center';
        }
    },
    /**
    Converts n <code>tp.Alignment</code> constant value and returns a <code>text-align</code> css property value  <br />
    Returns <br />
    <ul>
        <li><code>left</code> for <code>tp.Alignment.Near</code>  </li>
        <li><code>right</code> for <code>tp.Alignment.Far</code> </li>
        <li><code>center</code> in all other cases. </li>
    </ul>
    @param {number} v - One of the <code>tp.Alignment</code> constant values.
    @param {boolean} [Reverse=false] - Defaults to false. True makes the function to return the opposite value, e.g. <code>right</code> instead of <code>left</code>.
    @returns {string} A css <code>text-align</code> property value
    */
    ToText: function (v, Reverse = false) {
        // to text-align css property
        if (v === this.Near) {
            return Reverse === true ? 'right' : 'left';
        } else if (v === this.Far) {
            return Reverse === true ? 'left' : 'right';
        } else {
            return 'center';
        }
    }

};

/**
Text metrics helper static class
@class
@static
*/
tp.TextSizeInfo = {
    /**
    Measures the size of an element after setting its innerHTML. Returns a { Width: xxx, Height: xxx } object.  
    Instantly creates a hidden element, passes all css font properties affecting the size of the rendered text from
    a specified source element to the hidden element, and records the size of the hidden element.
     @param {string} Text The text to measure.
     @param {Element} SourceElement The element to be used as source. Provides css font properties affecting the size of the rendered text
     @returns {tp.Size} A { Width: xxx, Height: xxx }  object
     */
    MeasureText: function (Text, SourceElement) {
        var el = tp.TextSizeInfo.CreateRulerElement(SourceElement);
        var Result = tp.TextSizeInfo.SizeOf(Text, el);
        el.parentNode.removeChild(el);
        return Result;
    },
    /**
     Creates and returns a hidden copy of a source element to be used in measuring text.         
     @param {Element} SourceElement The element to be used as source. Provides css font properties affecting the size of the rendered text
     @returns {Element} A copy of a source element to be used in measuring text.
     */
    CreateRulerElement: function (SourceElement) {
        var div = document.createElement('div');

        div.style.position = 'absolute';
        div.style.visibility = 'hidden';
        div.style.height = 'auto';
        div.style.width = 'auto';

        document.body.appendChild(div);

        var Style = div.ownerDocument.defaultView.getComputedStyle(SourceElement, ''); // get the computed style

        if (SourceElement !== document.body) {
            div.style.marginTop = Style.marginTop;
            div.style.marginRight = Style.marginRight;
            div.style.marginBottom = Style.marginBottom;
            div.style.marginLeft = Style.marginLeft;

            div.style.borderTop = Style.borderTop;
            div.style.borderRight = Style.borderRight;
            div.style.borderBottom = Style.borderBottom;
            div.style.borderLeft = Style.borderLeft;

            div.style.paddingTop = Style.paddingTop;
            div.style.paddingRight = Style.paddingRight;
            div.style.paddingBottom = Style.paddingBottom;
            div.style.paddingLeft = Style.paddingLeft;
        }

        var v;
        var FontProps = ['font-size', 'font-style', 'font-weight', 'font-family', 'line-height', 'text-transform', 'letter-spacing'];
        for (var i = 0, ln = FontProps.length; i < ln; i++) {
            v = Style.getPropertyValue(FontProps[i]);
            div.style.setProperty(FontProps[i], v);
        }
        return div;
    },

    /**
     Measures the size of an element after setting its innerHTML. Returns a { Width: xxx, Height: xxx } object.
     @param {string} Text The text to measure.
     @param {Element} el The element where the text is going to be displayed.
     @returns {tp.Size} A  { Width: xxx, Height: xxx }  object
     */
    SizeOf: function (Text, el) {
        el.innerHTML = Text;

        return new tp.Size(el.offsetWidth, el.offsetHeight);
    },
    /**
     Measures the width of an element after setting its innerHTML.  
     @param {string} Text The text to measure.
     @param {Element} el The element where the text is going to be displayed.
     @returns {number} Returns the width of an element after setting its innerHTML.
     */
    WidthOf: function (Text, el) {
        el.innerHTML = Text;
        return el.offsetWidth;
    },
    /**
     Measures the height of an element after setting its innerHTML.  
     @param {string} Text The text to measure.
     @param {Element} el The element where the text is going to be displayed.
     @returns {number} Returns the height of an element after setting its innerHTML.
     */
    HeightOf: function (Text, el) {
        el.innerHTML = Text;
        return el.offsetHeight;
    }
};







//#endregion





//---------------------------------------------------------------------------------------

//#region Events

/** Enum-like object for event groups 
 @class
 @enum {number}
 */
tp.EventGroup = {
    None: 0,
    Click: 1,
    Mouse: 2,
    Keyboard: 4,
    Focus: 8,
    //Size: 0x10,
    Change: 0x20,
    Scroll: 0x40,
    Text: 0x80,
    DragSource: 0x100,
    DropTarget: 0x200,
    ContextMenu: 0x400,
    Clipboard: 0x800
};
Object.freeze(tp.EventGroup);

/** A helper class for events 
 @class
 @static
 */
tp.Events = (function () {
    return {
        Unknown: 'Unknown',
        Click: 'Click',
        DoubleClick: 'DoubleClick',
        MouseDown: 'MouseDown',
        MouseUp: 'MouseUp',
        MouseEnter: 'MouseEnter',
        MouseMove: 'MouseMove',
        MouseLeave: 'MouseLeave',
        KeyDown: 'KeyDown',
        KeyPress: 'KeyPress',
        KeyUp: 'KeyUp',
        MouseWheel: 'MouseWheel',
        Scroll: 'Scroll',
        ContextMenu: 'ContextMenu',
        Load: 'Load',
        Resize: 'Resize',
        Activate: 'Activate',
        Focus: 'Focus',
        LostFocus: 'LostFocus',
        InputChanged: 'InputChanged',
        TextSelected: 'TextSelected',
        Change: 'Change',
        DragStart: 'DragStart',
        Drag: 'Drag',
        DragEnd: 'DragEnd',
        DragEnter: 'DragEnter',
        DragOver: 'DragOver',
        DragLeave: 'DragLeave',
        DragDrop: 'DragDrop',
        Cut: 'Cut',
        Copy: 'Copy',
        Paste: 'Paste',
        Custom: 'Custom'
    };
})();

/** An array that maps DOM event names to Tripous event names */
tp.Events.Map = [
    { dom: 'click', tp: tp.Events.Click },
    { dom: 'dblclick', tp: tp.Events.DoubleClick },
    { dom: 'mousedown', tp: tp.Events.MouseDown },
    { dom: 'mouseup', tp: tp.Events.MouseUp },
    { dom: 'mouseover', tp: tp.Events.MouseEnter },
    { dom: 'mousemove', tp: tp.Events.MouseMove },
    { dom: 'mouseout', tp: tp.Events.MouseLeave },
    { dom: 'keydown', tp: tp.Events.KeyDown },
    { dom: 'keypress', tp: tp.Events.KeyPress },
    { dom: 'keyup', tp: tp.Events.KeyUp },
    { dom: 'scroll', tp: tp.Events.Scroll },
    { dom: 'mousewheel', tp: tp.Events.MouseWheel },
    { dom: 'DOMMouseScroll', tp: tp.Events.MouseWheel },
    { dom: 'contextmenu', tp: tp.Events.ContextMenu },
    { dom: 'load', tp: tp.Events.Load },
    { dom: 'resize', tp: tp.Events.Resize },
    { dom: 'activate', tp: tp.Events.Activate },
    { dom: 'DOMActivate', tp: tp.Events.Activate },
    { dom: 'focus', tp: tp.Events.Focus },
    { dom: 'blur', tp: tp.Events.LostFocus },
    { dom: 'change', tp: tp.Events.Change },
    { dom: 'input', tp: tp.Events.InputChanged },
    { dom: 'select', tp: tp.Events.TextSelected },
    { dom: 'dragstart', tp: tp.Events.DragStart },
    { dom: 'drag', tp: tp.Events.Drag },
    { dom: 'dragend', tp: tp.Events.DragEnd },
    { dom: 'dragenter', tp: tp.Events.DragEnter },
    { dom: 'dragover', tp: tp.Events.DragOver },
    { dom: 'dragleave', tp: tp.Events.DragLeave },
    { dom: 'drop', tp: tp.Events.DragDrop },
    { dom: 'copy', tp: tp.Events.Copy },
    { dom: 'cut', tp: tp.Events.Cut },
    { dom: 'paste', tp: tp.Events.Paste }
];


/**
* Returns the index of a event in the tp.Events.Map array by specifing its DOM name
* @param {string} DomEventName The DOM name of the event.
* @returns {number} Returns the index of the event in the tp.Events.Map array.
*/
tp.Events.DomIndex = function (DomEventName) {
    if (tp.IsString(DomEventName)) {
        for (var i = 0, ln = tp.Events.Map.length; i < ln; i++) {
            if (tp.IsSameText(tp.Events.Map[i].dom, DomEventName))
                return i;
        }
    }
    return -1;
};
/**
 * Returns the index of a event in the tp.Events.Map array by specifing its Tripous name
 * @param {string} TripousEventName The Tripous name of the event.
 * @returns {number} Returns the index of the event in the tp.Events.Map array.
 */
tp.Events.TripousIndex = function (TripousEventName) {
    if (tp.IsString(TripousEventName)) {
        for (var i = 0, ln = tp.Events.Map.length; i < ln; i++) {
            if (tp.IsSameText(tp.Events.Map[i].tp, TripousEventName))
                return i;
        }
    }
    return -1;
};
/**
 * Converts a Tripous event name to a DOM event name. 
 * @param {string} TripousEventName The Tripous name of the event.
 * @returns {string} Returns the DOM event name.
 */
tp.Events.ToDom = function (TripousEventName) {
    var Index = tp.Events.TripousIndex(TripousEventName);
    if (Index > -1) {
        return tp.Events.Map[Index].dom;
    }
    return tp.Events.Unknown;
};
/**
 * Converts a DOM event name to a Tripous event name.
 * @param {string} DomEventName The DOM name of the event.
 * @returns {string} Returns the Tripous event name.
 */
tp.Events.ToTripous = function (DomEventName) {
    var Index = tp.Events.DomIndex(DomEventName);
    if (Index > -1) {
        return tp.Events.Map[Index].tp;
    }
    return tp.Events.Unknown;
};
Object.freeze(tp.Events);



/**
 * Gets and returns the event target (sender) of a specified argument.
 * @param {string|EventTarget} Sender The sender (event target) to check.
 * @returns {EventTarget} Returns the actual event target.
 */
tp.GetEventTarget = function (Sender) {

    let target = null;

    if (tp.IsString(Sender)) {
        target = tp.Select(Sender);
    } else if (Sender instanceof HTMLElement) {
        target = Sender;
    } else if (Sender === window) {
        target = window;
    } else if ('addEventListener' in Sender) {
        target = Sender;
    }

    return target;
};
/**
Adds Listener to Sender (target of the event) for the EventName event.
@param {string|EventTarget} Sender - Selector or element. The dom element upon which the event happens, i.e. a button that provides a click event.
@param {string} EventName - The 'name' of the event. Could be a dom name, i.e. 'click' or a tp.Events.XXXX constant.
@param {Function|Object} FuncOrListener - The listener object to notify or the function to call, when the event happens. 
A listener object must implement the handleEvent() function if it is to be called with the context (this) pointing to that listener.
@param {boolean} [UseCapture=false] - Optional. Defaults to false. When false (default) the event propagation uses the bubble mode, else the capture mode.   
 WARNING: Only certain events can bubble. Events that do bubble have the Event.bubbles property set to true.
@see {@link http://www.w3.org/TR/DOM-Level-2-Events/events.html#Events-EventListener|EventListener}
@see {@link http://stackoverflow.com/questions/4616694/what-is-event-bubbling-and-capturing|Bubble and Capture}
@see {@link https://en.wikipedia.org/wiki/DOM_events|DOM Events} 
*/
tp.On = function (Sender, EventName, FuncOrListener, UseCapture = false) {

    let target = tp.GetEventTarget(Sender);

    if (target) {
        var S = tp.Events.ToDom(EventName);              // could be a tp.Events.XXXX constant
        if (!tp.IsSameText(S, tp.Events.Unknown)) {      // it is a dom event
            EventName = S;
        }
        UseCapture = UseCapture === true;
        target.addEventListener(EventName, FuncOrListener, UseCapture);
    }

};
/**
Removes Listener from  Sender (target of the event) for the EventName event.
@param {string|Element} Sender - Selector or element. The dom element upon which the event happens, i.e. a button that provides a click event.
@param {string} EventName - The 'name' of the event. Could be a dom name, i.e. 'click' or a tp.Events.XXXX constant.
@param {Function|Object} FuncOrListener - The listener object to notify or the function to call, when the event happens. 
A listener object must implement the handleEvent() function if it is to be called with the context (this) pointing to that listener.
@param {boolean} [UseCapture=false] - Optional. Defaults to false. When false (default) the event propagation uses the bubble mode, else the capture mode.  
 WARNING: Only certain events can bubble. Events that do bubble have the Event.bubbles property set to true.
@see {@link http://www.w3.org/TR/DOM-Level-2-Events/events.html#Events-EventListener|EventListener}
@see {@link http://stackoverflow.com/questions/4616694/what-is-event-bubbling-and-capturing|Bubble and Capture}
@see {@link https://en.wikipedia.org/wiki/DOM_events|DOM Events}
*/
tp.Off = function (Sender, EventName, FuncOrListener, UseCapture = false) {

    let target = tp.GetEventTarget(Sender);

    if (target) {
        var S = tp.Events.ToDom(EventName);              // could be a tp.Events.XXXX constant
        if (!tp.IsSameText(S, tp.Events.Unknown)) {      // it is a dom event
            EventName = S;
        }
        UseCapture = UseCapture === true;
        target.removeEventListener(EventName, FuncOrListener, UseCapture);
    }
};
/**
Triggers (fires) an event
@param {string|Element} Sender - Selector or element. The dom element that fires the event.
@param {Event|string} ev - The event to trigger. Could be a {@link Event} instance or a string, i.e. 'click' or 'change'.
@returns {boolean} The return value is false if event is cancelable and at least one of the event handlers which handled this event called Event.preventDefault(). Otherwise it returns true.
*/
tp.Trigger = function (Sender, ev) {
    let target = tp.GetEventTarget(Sender);
    if (tp.IsString(ev))
        ev = new Event(ev);
    return target ? target.dispatchEvent(ev) : false;
};

/**
Cancels an event.
@param {Event} e - The event
@returns {boolean} Returns false always.
*/
tp.CancelEvent = function (e) {

    if ('stopPropagation' in e) {
        e.stopPropagation();
    }

    if ('cancelBubble' in e) {
        e.cancelBubble = true;
    }

    if ('cancel' in e) {
        e.cancel = true;
    }

    if ('returnValue' in e) {
        e.returnValue = false;
    }


    return false;
};
/**
Creates and returns an event object
@param {string} EventName The name of the event.
@returns {Event} Returns the event object.
*/
tp.CreateEventObject = function (EventName) {
    var Result = null;
    Result = tp.Doc.createEvent(EventName);
    return Result;
};
/**
Creates and returns a custom event
@param {string} EventName The name of the event.
@param {boolean} [Bubbles=true] True if the event bubbles
@param {boolean} [Cancelable=true] True if the event is cancelable.
@param {object} [Detail=null] A details object (plain object) to attach to the event.
@returns {CustomEvent} Returns the CustomEvent object.
*/
tp.CreateCustomEvent = function (EventName, Bubbles = true, Cancelable = true, Detail = null) {
    Detail = Detail || {};
    Detail['IsCustom'] = true;
    Detail['bubbles'] = Bubbles === true;
    Detail['cancelable'] = Cancelable === true;
    var ev = new CustomEvent(EventName, Detail);
    return ev;
};

//#endregion

//#region tp.EventArgs
/** Represents event arguments and can be used with native events or tripous script events */
tp.EventArgs = class {
    /** 
    * constructor
    * @param {string|Event|object} EventName The name of the event.
    * When only a single argument is passed then it could be any of the following:
    * 1. String: Denotes the event name.
    * 2. Event: The DOM event caused this event
    * 3. Object: Any script object, such as a plain object. That object is merged with this instance.
    * @param {object} [Sender=null] The sender of the event, e.g. a button that is clicked.
    * @param {Event} [e=null] The DOM event  caused this event
    */
    constructor(EventName, Sender = null, e = null) {
        this.e = null;
        this.el = null;
        this.EventName = '';
        this.Sender = null;
        this.Handled = false;
        this.Cancel = false;
        this.Command = '';

        if (arguments.length === 1) {
            if (arguments[0] instanceof Event) {
                this.e = arguments[0];
            } else if (arguments[0] instanceof Object) {
                tp.MergeQuick(this, arguments[0]);
            }
        } else {
            if (arguments.length > 0)
                this.EventName = arguments[0] || '';

            if (arguments.length > 1)
                this.Sender = arguments[1];

            if (arguments.length > 2)
                this.e = arguments[2];

        }

        if (this.e instanceof Event && tp.IsBlank(this.EventName)) {
            var S = tp.Events.ToTripous(this.e.type);
            this.EventName = tp.IsSameText(S, tp.Events.Unknown) ? this.e.type : S;
        }
    }

    get IsDomEvent() { return this.e instanceof Event; }
    get IsTripousEvent() { return !this.IsDomEvent; }
};
/** The DOM Event object, if any, else null.
 @type {Event}
 */
tp.EventArgs.prototype.e = null;                // Event;
/** The element of the event, if any, else null.
 @type {HTMLElement}
 */
tp.EventArgs.prototype.el = null;               // HTMLElement;
/** The event name
 @type {string}
 */
tp.EventArgs.prototype.EventName = '';
/** The sender of the event, if any, else null 
 @type {object}
 */
tp.EventArgs.prototype.Sender = null;
/** 
 Indicates whether the event is handled by an event handler function.
 @type {boolean}
 */
tp.EventArgs.prototype.Handled = false;         // true if this event is already handled 
/**
 Indicates whether the event should be cancelled
 @type {boolean}
 */
tp.EventArgs.prototype.Cancel = false;
/** A string command associated to the event
 @type {string}
 */
tp.EventArgs.prototype.Command = '';


//#endregion

//#region Encoding

/**
Encodes an argument (a Key/Value pair) for use with GET/POST ajax operations and returns the encoded string. 
@param {string} Key The key of the argument
@param {any} Value The value of the argument
@returns {string} Returns the encoded string.
*/
tp.EncodeArg = function (Key, Value) {
    Value = tp.IsEmpty(Value) ? "" : Value;
    var Result = encodeURIComponent(Key) + "=" + encodeURIComponent(Value);
    Result = Result.replace(/%20/g, '+');
    return Result;
};
/**
Encodes arguments for use with GET/POST ajax operations and returns the encoded string. 
@param  {object|object[]|HTMLElement[]} v - The value to operate on. Could be 
1. a plain object, 
2. array of values, 
3. array of DOM elements
@returns {string} Returns the encoded string.
 */
tp.EncodeArgs = function (v) {

    var i, ln, Name, Value, Data = [];

    if (v instanceof Array && v.length > 0) {
        if (tp.IsHTMLElement(v[0])) {                       // is an array of HTMLElement
            for (i = 0, ln = v.length; i < ln; i++) {
                if ('name' in v[i] && 'value' in v[i]) {
                    Name = v[i]['name'];
                    Value = v[i]['value'];
                    Data[Data.length] = tp.EncodeArg(Name, Value);
                }
            }
        } else {                                            // is an array of values
            for (i = 0, ln = v.length; i < ln; i++) {
                Data[Data.length] = tp.EncodeArg("v" + i.toString(), v);
            }
        }
    } else if (tp.IsPlainObject(v)) {                       // is a plain object
        for (var Prop in v) {
            Data[Data.length] = tp.EncodeArg(Prop, v[Prop]);
        }
    }

    var S = Data.join('&');
    S = S.replace(/%20/g, '+');

    return S;
};

//#endregion

//#region Serialization

/**
Reads the value of an element (input, select, textarea, button) and adds a property to a specified plain javascript object. 
The new property is named after element's name or element's id (in this order).

That is for an element such as 
    &lt;input type='text' id='UserName' value='John' /&gt;
a property/value is added as 
    { UserName: 'John' }

WARNING: input elements of type file or image, are IGNORED.
NOTE: A select element of type select-multiple generates an array property.
@param {HTMLElement} el - The element to get the value from. Must have a name attribute defined (or at least an id defined).
@param {object} Model - A plain object where the new property/value is added. Caller code is responsible to provide this object.
*/
tp.ElementToProperty = function (el, Model) {
    if (el.name || el.id) {
        var A, j, jln, Name = el.name || el.id;

        if (!tp.IsBlank(Name)) {
            var NodeName = el.nodeName.toLowerCase();
            var Type = el.type ? el.type.toLowerCase() : '';

            switch (NodeName) {
                case 'input':
                    switch (Type) {
                        case 'hidden':
                        case 'text':
                        case 'password':
                        case 'color':
                        case 'date':
                        case 'datetime-local':
                        case 'email':
                        case 'month':
                        case 'number':
                        case 'range':
                        case 'search':
                        case 'tel':
                        case 'time':
                        case 'url':
                        case 'week':
                            Model[Name] = el.value;
                            break;
                        case 'checkbox':
                            Model[Name] = el.checked ? true : false;
                            break;
                        case 'radio':
                            if (el.checked) {
                                Model[Name] = el.value;
                            }
                            break;
                        case 'button':
                        case 'submit':
                        case 'reset':
                            Model[Name] = el.value;
                            break;
                        case 'file':
                            break;
                        case 'image':
                            break;
                    }
                    break;
                case 'button':
                    switch (Type) {
                        case 'button':
                        case 'submit':
                        case 'reset':
                            Model[Name] = el.value;
                            break;
                    }
                    break;
                case 'select':
                    switch (Type) {
                        case 'select-one':
                            Model[Name] = el.value;
                            break;
                        case 'select-multiple':
                            A = [];
                            for (j = 0, jln = el.options.length; j < jln; j++) {
                                if (el.options[j].selected) {
                                    A.push(el.options[j].value);
                                }
                            }
                            Model[Name] = A;
                            break;
                    }
                    break;
                case 'textarea':
                    Model[Name] = el.value;
                    break;
            }
        }

    }

};
/**
Serializes a form, or any other container, into a javascript object, by adding a property for each input, select, textarea or button child element, to that object.  
The new property is named after child element's name or id (in this order). 

That is for an element such as
    &lt;input type='text' id='UserName' value='John' /&gt;
a property/value is added as
    { UserName: 'John' }

WARNING: input elements of type file or image, are IGNORED.
NOTE: A select element of type select-multiple generates an array property.
@param {Element|string} ElementOrSelector - A selector or n html form or any other container element, that contains input, select, textarea and button elements.  
@param {object} [Model=null] - Optional. A plain object where the new properties/values are added.
@returns {object} Returns the model where the new properties/values are added.
*/
tp.ContainerToModel = function (ElementOrSelector, Model = null) {
    if (!Model) {
        Model = {};
    }

    var parent = tp.Select(ElementOrSelector);

    if (parent instanceof HTMLElement) {
        var i, ln, el, elements = parent.nodeName.toLowerCase() === 'form' ? parent.elements : tp.SelectAll(parent, 'input, select, textarea, button');
        for (i = 0, ln = elements.length; i < ln; i++) {
            el = elements[i];
            if (!tp.IsBlank(el.name || el.id))
                tp.ElementToProperty(el, Model);
        }
    }

    return Model;
};
/**
Serializes a form, or any other container, into a javascript object, by adding a property for each input, select, textarea or button child element, to that object.  
The new property is named after child element's name or id (in this order).

That is for an element such as
    &lt;input type='text' id='UserName' value='John' /&gt;
a property/value is added as
    { UserName: 'John' }

WARNING: input elements of type file or image, are INCLUDED.
NOTE: A select element of type select-multiple generates an array property.
@param {boolean} ShowSpinner - True to show the global spinner while processing files.
@param {Element|String} ElementOrSelector - A selector or n html form or any other container element, that contains input, select, textarea and button elements.
@returns {Promise} Returns a promise
*/
tp.ContainerToModelAsync = async function (ShowSpinner, ElementOrSelector) {

    let Model = {};
    let Result = Promise.resolve(Model);
    let parent = tp.Select(ElementOrSelector);

    if (parent instanceof HTMLElement) {

        // collect the elements in two lists, one for the input[type='file'] and one for the rest of the elements
        let i, ln, el, elements = parent.nodeName.toLowerCase() === 'form' ? parent.elements : tp.SelectAll(parent, 'input, select, textarea, button');
        let IsInputFileElement;
        let FileElementList = [];
        let ElementList = [];
        let PromiseList = [];

        for (i = 0, ln = elements.length; i < ln; i++) {
            el = elements[i];
            if (!tp.IsBlank(el.name || el.id)) {
                IsInputFileElement = el instanceof HTMLInputElement && tp.IsSameText(el.type, 'file');

                if (IsInputFileElement) {
                    FileElementList.push(el);
                } else {
                    ElementList.push(el);
                }
            }
        }


        // input[type='file'] elements first    
        FileElementList.forEach(function (el) {

            let P = new Promise((resolve, reject) => {
                tp.ReadFiles(true, el.files)
                    .then(function (FileList) {
                        Model[el.name || el.id] = FileList; // HttpFile[]
                        resolve();
                    });
            });

            PromiseList.push(P);
        });

        // the rest elements
        let P2 = new Promise((resolve, reject) => {
            for (i = 0, ln = ElementList.length; i < ln; i++) {
                el = ElementList[i];
                tp.ElementToProperty(el, Model);
            }
            resolve();
        });
        PromiseList.push(P2);


        // nested function
        let Spinner = function (Flag) {
            if (ShowSpinner) {
                tp.ShowSpinner(Flag);
            }
        };

        Spinner(true);

        // create the result promise
        Result = Promise.all(PromiseList)
            .then(function () {
                Spinner(false);
                return Model;
            }).catch(function (e) {
                tp.ForceHideSpinner();
                tp.Throw(e ? e.toString() : 'Unknown error');
            });

    }

    return Result;
};

/**
Converts a specified ArrayBuffer to a Hex string
@param {ArrayBuffer} Buffer The ArrayBuffer to convert to Hex string
@returns {string} Returns the Hex string
*/
tp.ArrayBufferToHex = function (Buffer) {

    var UA = new Uint8Array(Buffer);
    var A = new Array(UA.length);
    var i = UA.length;
    while (i--) {
        A[i] = (UA[i] < 16 ? '0' : '') + UA[i].toString(16);  // map to hex
    }

    UA = null; // free memory
    return A.join('');
};


/**
 * A file sent by a POST action to the server, perhaps by an ajax call.
 * @class
 * */
tp.HttpFile = function () {
    this.FileName = '';
    this.Size = 0;
    this.MimeType = '';
    this.Data = '';
};
/** The file name  */
tp.HttpFile.prototype.FileName = '';
/** The size of the file */
tp.HttpFile.prototype.Size = 0;
/** The mime type of the file content */
tp.HttpFile.prototype.MimeType = '';
/** The file content as a base64 string */
tp.HttpFile.prototype.Data = '';

/**
Loads file data from disk using a system dialog. It is passed a list of File objects to load. 

Returns a Promise with a resolve(ResultFileList) where each entry in the ResultFileList is an object of 
    { FileName:, Size:, MimeType:, Data:,}
where Data is a base64 string.  

IMPORTANT: For increasing the allowed maximub POST size, see: {@link http://stackoverflow.com/questions/3853767/maximum-request-length-exceeded|stackoverflow}
@example
var el = tp.Select('#FileData');

tp.ReadFiles(true, el.files)
.then(function (FileList) {
    // handle file list here
}).catch(function (Error) {
    throw Error;
})

@param {boolean} ShowSpinner - True to show the global spinner while processing files.
@param {string|any[]|FileList} FileListOrSelector - Either an input[type="file"] element, or a selector to such an element, or a list of File objects 
(see File API FileList and File classes at {@link https://developer.mozilla.org/en-US/docs/Web/API/FileList|FileList} )
@param {function} [OnDone=null] - Optional. A function(List: HttpFile[]) to call when done and all files are loaded.
It is passed a list of { FileName:, Size:, MimeType:, Data:,} where Data is a base64 string.
@param {function} [OnError=null] - Optional. A function(e: Error, File: File) to call on error.
It is passed the error event and the File that caused the error.
@param {object} [Context=null] - Optional. Defaults to null. The context (this) to use when calling the provided call-back functions.
@param {boolean} [AsHex=false] - Optional. Defaults to false. If true then the Data of the file is converted to a Hex string. Else to a base64 string.
@returns {Promise} Returns a promise
*/
tp.ReadFiles = function (ShowSpinner, FileListOrSelector, OnDone = null, OnError = null, Context = null, AsHex = false) {

    let ReadAsBase64 = function (ResultList, File, ReadNext, Resolve, Reject) {
        let Reader = new FileReader();

        Reader.onload = function () {

            let Data = Reader.result;
            let Parts = Data.split('base64,');
            if (Parts.length === 2) {
                Data = Parts[1];
            }

            // { FileName:,Size:, MimeType:, Data:,}
            let o = new tp.HttpFile();
            o.FileName = File.name;
            o.Size = File.size;
            o.MimeType = File.type;
            o.Data = Data;

            ResultList.push(o);
            ReadNext();
        };
        Reader.onerror = function (e) {
            Reject(e);
            if (OnError)
                tp.Call(OnError, Context, e);
        };
        Reader.onabort = Reader.onerror;

        Reader.readAsDataURL(File);
    };
    let ReadAsHex = function (ResultList, File, ReadNext, Resolve, Reject) {
        let Reader = new FileReader();

        Reader.onload = function () {

            let Data = Reader.result;

            // { FileName:,Size:, MimeType:, Data:,}
            let o = new tp.HttpFile();
            o.FileName = File.name;
            o.Size = File.size;
            o.MimeType = File.type;
            o.Data = tp.ArrayBufferToHex(Data);

            ResultList.push(o);
            ReadNext();
        };
        Reader.onerror = function (e) {
            Reject(e);
            if (OnError)
                tp.Call(OnError, Context, e);
        };
        Reader.onabort = Reader.onerror;

        Reader.readAsArrayBuffer(File);
    };


    return new Promise(function (Resolve, Reject) {
        let el;
        let FileList = null;
        try {
            if (tp.IsArrayLike(FileListOrSelector)) {
                FileList = FileListOrSelector;
            } else {
                el = tp.Select(FileListOrSelector);
                if (el instanceof HTMLInputElement) {
                    FileList = el.files;
                }
            }
        } catch (e) {
            Reject(e);

            if (OnError)
                tp.Call(OnError, Context, e);
        }

        if (ShowSpinner) {
            tp.ShowSpinner(true);
        }

        var Index = 0;
        var ResultList = [];

        var ReadNext = function () {
            if (Index < FileList.length) {
                var File = FileList[Index++];

                if (AsHex === true) {
                    ReadAsHex(ResultList, File, ReadNext, Resolve, Reject);
                } else {
                    ReadAsBase64(ResultList, File, ReadNext, Resolve, Reject);
                }

            } else {
                if (ShowSpinner) {
                    tp.ShowSpinner(false);
                }
                Resolve(ResultList);
                if (OnDone)
                    tp.Call(OnDone, Context, ResultList);
            }
        };

        ReadNext();


    });
};

/**
Creates an html form and submits that form to a url using POST method. 
It accepts a model parameter (a plain object) whose properties become form's input elements. 
Model properties that are arrays are posted as name[0]=value name[1]=value etc. 
Model properties that are other than primitives and dates are stringified using JSON.
@param {string} Url The url where the form is submitted.  
@param {object} Model A plain object whose properties become input elements in the submitted form. 
 */
tp.PostModelAsForm = function (Url, Model) {
    var form, el, i, ln, PropName, v, Data = {};

    for (PropName in Model) {
        v = Model[PropName];
        if (!tp.IsEmpty(v) && !tp.IsFunction(v)) {
            if (v instanceof Date) {
                v = v.toISOString();
            }
            Data[PropName] = v;
        }
    }

    form = document.createElement("form");
    form.action = Url;
    form.method = 'post';

    var NormalizeValue = function (Value) {
        return tp.IsSimple(Value) ? Value : JSON.stringify(Value);
    };

    for (PropName in Data) {
        v = Data[PropName];

        if (tp.IsArray(v)) {
            for (i = 0, ln = v.length; i < ln; i++) {
                el = document.createElement("input");
                el.setAttribute("type", "hidden");
                el.setAttribute("name", PropName + '[' + i + ']');
                el.setAttribute("value", NormalizeValue(v[i]));
                form.appendChild(el);
            }
        } else {
            el = document.createElement("input");
            el.setAttribute("type", "hidden");
            el.setAttribute("name", PropName);
            el.setAttribute("value", NormalizeValue(v));
            form.appendChild(el);
        }
    }

    document.body.appendChild(form);
    form.submit();

    setTimeout(() => { tp.Remove(form); }, 1000 * 3);
};

//#endregion

//#region Keyboard

/**
 Keys. Numeric constants for keys.
 @class
 @enum {number}
 */
tp.Keys = {
    F1: 112,
    F2: 113,
    F3: 114,
    F4: 115,
    F5: 116,
    F6: 117,
    F7: 118,
    F8: 119,
    F9: 120,
    F10: 121,
    F11: 122,
    F12: 123,
    Ctrl: 17,
    Shift: 16,
    Alt: 18,
    Enter: 13,
    Home: 36,
    End: 35,
    Left: 37,
    Right: 39,
    Up: 38,
    Down: 40,
    PageUp: 33,
    PageDown: 34,
    Escape: 27,
    Space: 32,
    Tab: 9,
    Backspace: 8,
    Delete: 46,
    Insert: 45,
    ContextMenu: 93,
    Windows: 91,
    Decimal: 110
};
Object.freeze(tp.Keys);

/**
 * Returns true if the specified KeyboardEvent represents a printable character.
 * @param   {KeyboardEvent}   e   The KeyboardEvent event
 * @returns {boolean} Returns true if the specified KeyboardEvent represents a printable character.
 */
tp.IsPrintableKey = function (e) {
    // If the key doesn't have a printable representation, e.char is an empty string.
    // see: https://developer.mozilla.org/en-US/docs/Web/Events/keypress
    if ('char' in e && tp.IsBlank(e.char))
        return false;

    // from: http://stackoverflow.com/questions/12467240/determine-if-javascript-e-keycode-is-a-printable-non-control-character
    var code = e.type === 'keypress' ? e.charCode : e.keyCode;

    var Result = code > 47 && code < 58    // number keys
        || code === 32                     // spacebar 
        || code > 64 && code < 91          // letter keys
        || code > 95 && code < 112         // numpad keys
        || code > 185 && code < 193        // ;=,-./` (in order)
        || code > 218 && code < 223;       // [\]' (in order)

    return Result;

};

//#endregion

//#region Mouse

/**
A full static class with the mouse cursor possible values.
{@link http://www.javascripter.net/faq/stylesc.htm}
@class
@static
*/
tp.Cursors = {
    Default: "default",
    Pointer: "pointer",

    Text: "text",
    VerticalText: "vertical-text",
    Help: "help",
    Move: "move",

    Wait: "wait",
    Progress: "progress",

    CrossHair: "crosshair",

    ResizeN: "n-resize",
    ResizeE: "e-resize",
    ResizeW: "w-resize",
    ResizeS: "s-resize",

    ResizeNE: "ne-resize",
    ResizeNW: "nw-resize",
    ResizeSE: "se-resize",
    ResizeSW: "sw-resize",

    ResizeCol: "col-resize",
    ResizeRow: "row-resize",

    AllScroll: "all-scroll",
    NotAllowed: "not-allowed",
    NoDrop: "no-drop",

    Auto: "auto", // let the browser choose the cursor
    Inherit: "inherit"
};

/** An enum-like full static class to help with the hit-testing when resizing elements 
 @class
 */
tp.Edge = {
    None: 0,
    N: 1,
    E: 2,
    W: 4,
    S: 8,
    NE: 0x10,
    NW: 0x20,
    SE: 0x40,
    SW: 0x80,

    get All() { return tp.Edge.N | tp.Edge.E | tp.Edge.W | tp.Edge.S | tp.Edge.NE | tp.Edge.NW | tp.Edge.SE | tp.Edge.SW; },

    get Height() { return tp.Bf.Subtract(tp.Edge.All, tp.Edge.E | tp.Edge.W); },
    get Width() { return tp.Bf.Subtract(tp.Edge.All, tp.Edge.N | tp.Edge.S); },

    get Left() { return tp.Edge.NW | tp.Edge.W | tp.Edge.SW; },
    get Top() { return tp.Edge.NW | tp.Edge.N | tp.Edge.NE; },
    get Right() { return tp.Edge.NE | tp.Edge.E | tp.Edge.SE; },
    get Bottom() { return tp.Edge.SW | tp.Edge.S | tp.Edge.SE; },

    IsHeigth(v) { return tp.Bf.In(v, tp.Edge.Height); },
    IsWidth(v) { return tp.Bf.In(v, tp.Edge.Width); },

    IsLeft(v) { return tp.Bf.In(v, tp.Edge.Left); },
    IsTop(v) { return tp.Bf.In(v, tp.Edge.Top); },
    IsRight(v) { return tp.Bf.In(v, tp.Edge.Right); },
    IsBottom(v) { return tp.Bf.In(v, tp.Edge.Bottom); },

    /**
    * Converts one of the Edge constants to a cursor value
    * @param {number} v - One of the tp.Edge numeric constants
    * @returns {string} Returns one of the tp.Cursors string constants
    */
    ToCursor: function (v) {
        // http://www.javascripter.net/faq/stylesc.htm

        switch (v) {
            case tp.Edge.NE: return tp.Cursors.ResizeNE;
            case tp.Edge.NW: return tp.Cursors.ResizeNW;
            case tp.Edge.SE: return tp.Cursors.ResizeSE;
            case tp.Edge.SW: return tp.Cursors.ResizeSW;

            case tp.Edge.N: return tp.Cursors.ResizeN;
            case tp.Edge.E: return tp.Cursors.ResizeE;
            case tp.Edge.W: return tp.Cursors.ResizeW;
            case tp.Edge.S: return tp.Cursors.ResizeS;
        }

        return tp.Cursors.Default;
    },

    /**
    Performs a hit-test on an element, regarding a possible resize, and returns one of the tp.Edge constants.
    @param {MouseEvent} e - A mouse event object
    @param {Element} el - The html element
    @param {number} [HandleSize=8] - The size of the resize handle in pixels
    @returns {number} Returns one of the tp.Edge constants.
    */
    ResizeHitTest: function (e, el, HandleSize = 8) {

        var R, P, X, Y, W, H, mX, mY, Mouse, i, ln;
        var Size = HandleSize || 8;

        Mouse = tp.Mouse.ToElement(e, el);
        P = new tp.Point(0, 0);

        mX = Mouse.X;
        mY = Mouse.Y;

        X = P.X;
        Y = P.Y;

        R = tp.Rect.FromClientRect(el);
        W = R.Width;
        H = R.Height;

        P = { X: mX, Y: mY };
        R = { X: X, Y: Y, Width: W, Height: H };

        if (tp.Point.PointInRect(P, R)) {

            var A = [
                { Type: tp.Edge.NE, R: { X: X + W - Size, Y: Y, Width: Size, Height: Size } },
                { Type: tp.Edge.NW, R: { X: X, Y: Y, Width: Size, Height: Size } },
                { Type: tp.Edge.SE, R: { X: X + W - Size, Y: Y + H - Size, Width: Size, Height: Size } },
                { Type: tp.Edge.SW, R: { X: X, Y: Y + H - Size, Width: Size, Height: Size } },

                { Type: tp.Edge.N, R: { X: X, Y: Y, Width: W, Height: Size } },
                { Type: tp.Edge.E, R: { X: X + W - Size, Y: Y, Width: Size, Height: H } },
                { Type: tp.Edge.W, R: { X: X, Y: Y, Width: Size, Height: H } },
                { Type: tp.Edge.S, R: { X: X, Y: Y + H - Size, Width: W, Height: Size } }
            ];

            for (i = 0, ln = A.length; i < ln; i++) {
                if (tp.Point.PointInRect(P, A[i].R)) {
                    return A[i].Type;
                }
            }
        }

        return tp.Edge.None;
    }

};
Object.freeze(tp.Edge);

/** An enum-like full static class. Mouse helper.
 * @class
 * */
tp.Mouse = {

    /** Button constant
     * @type {number}
     */
    NONE: 0,
    /** Button constant
     * @type {number}
     */
    LEFT: 1,
    /** Button constant
     * @type {number}
     */
    RIGHT: 2,
    /** Button constant. The wheel button.
     * @type {number}
     */
    MID: 4,

    /** Gets or sets the mouse cursor. <br />
     {@link https://developer.mozilla.org/en-US/docs/Web/CSS/cursor|MDN}
     @type {string}
     */
    get Cursor() { return tp.Doc.body.style.cursor; },
    set Cursor(v) { tp.Doc.body.style.cursor = v; },

    /**
     Returns the mouse button that is currently pressed, using the defined button constants of this class.
     @param {MouseEvent} e A mouse event
     @returns {number} Returns one of the mouse button constants of this class
    */
    Button: function (e) {
        if ('which' in e) {
            switch (e.which) {
                case 1: return tp.Mouse.LEFT;
                case 2: return tp.Mouse.MID;
                case 3: return tp.Mouse.RIGHT;
            }
        } else if ('button' in e) {
            switch (e.button) {
                case 0: return tp.Mouse.LEFT;
                case 1: return tp.Mouse.MID;
                case 2: return tp.Mouse.RIGHT;
            }
        }

        return tp.Mouse.NONE;
    },

    /**
      Returns a bit-field with the mouse buttons currently pressed.
      @param {MouseEvent} e - A mouse event
      @returns {number} Returns a bit-field with the mouse buttons currently pressed.
    */
    Buttons: function (e) { return 'buttons' in e ? e.buttons : tp.Mouse.NONE; },

    /**
    Returns true if the Left mouse button is currently pressed.
    @param {MouseEvent} e - A mouse event
    @returns {boolean} Returns true if the Left mouse button is currently pressed.
    */
    IsLeft: function (e) { return tp.Mouse.LEFT === tp.Mouse.Button(e); },
    /**
    Returns true if the Middle mouse button is currently pressed.
    @param {MouseEvent} e - A mouse event
    @returns {boolean} Returns true if the Middle mouse button is currently pressed.
    */
    IsMid: function (e) { return tp.Mouse.MID === tp.Mouse.Button(e); },
    /**
    Returns true if the Right mouse button is currently pressed.
    @param {MouseEvent} e - A mouse event
    @returns {boolean} Returns true if the Right mouse button is currently pressed.
    */
    IsRight: function (e) { return tp.Mouse.RIGHT === tp.Mouse.Button(e); },


    /** 
    Returns an tp.Point with mouse position relative to the Top/Left of the fully rendered page (document)
    @param {MouseEvent} e - A mouse event
    @returns {tp.Point} Returns an tp.Point with mouse position relative to the Top/Left of the fully rendered page (document)
    */
    ToDocument: function (e) {
        e = e || window.event;
        var doc = e.target.ownerDocument;

        var X = e.pageX;
        var Y = e.pageY;

        if (X === tp.Undefined) {
            X = e.clientX + doc.documentElement.scrollLeft + doc.body.scrollLeft;
            Y = e.clientY + doc.documentElement.scrollTop + doc.body.scrollTop;
        }

        return new tp.Point(Math.round(X), Math.round(Y));
    },
    /** 
    Returns an tp.Point with mouse position relative to the Top/Left of the browser window (viewport)
    @param {MouseEvent} e - A mouse event
    @returns {tp.Point} Returns an tp.Point with mouse position relative to the Top/Left of the browser window (viewport)
    */
    ToViewport: function (e) {
        var X = Math.round(e.clientX);
        var Y = Math.round(e.clientY);
        return new tp.Point(X, Y);
    },
    /** 
     Returns an tp.Point with mouse position relative to the Top/Left of the physical screen
     @param {MouseEvent} e - A mouse event
     @returns {tp.Point} Returns an tp.Point with mouse position relative to the Top/Left of the physical screen
     */
    ToScreen: function (e) {
        let X = e.screenX || 0;
        let Y = e.screenY || 0;

        return new tp.Point(Math.round(X), Math.round(Y));
    },

    /**
    Returns a tp.Point with mouse position relative to the Top/Left of an element (sender of the event)
    @param {MouseEvent} e - A mouse event
    @param {Element} [el=null] - Optional. If not passed then the target (sender) of the event is used.
    @returns {tp.Point} Returns a tp.Point with mouse position relative to the Top/Left of an element (sender of the event)
    */
    ToElement: function (e, el = null) {
        el = el || e.target || e.srcElement;
        var R = el.getBoundingClientRect();
        var X = tp.Truncate(e.clientX - R.left);
        var Y = tp.Truncate(e.clientY - R.top);

        return new tp.Point(X, Y);
    },
    /**
    Returns true if the mouse pointer is inside an element
    @param {MouseEvent} e - A mouse event
    @param {Element} el The element to check
    @returns {boolean} Returns true if the mouse pointer is inside an element
    */
    IsInElement: function (e, el) {
        var P = tp.Mouse.ToElement(e, el);
        var R = tp.Rect.FromClientRect(el);
        R.X = 0;
        R.Y = 0;
        return R.Contains(P);
    },
    /**
     Returns the topmost element at the specified position relative to the Top/Left of the browser window (viewport).
     @param {MouseEvent} e - A mouse event
     @returns {Node} Returns the topmost element at the specified position relative to the Top/Left of the browser window (viewport).
     */
    ElementUnderMouse: function (e) {
        return tp.Mouse.ElementAt(e.clientX, e.clientY);
    },
    /**
     Returns the topmost element at the specified position relative to the Top/Left of the browser window (viewport).         
     @param   {number}   X   The X coordinate relative to the Top/Left of the browser window (viewport)
     @param   {number}   Y   The Y coordinate relative to the Top/Left of the browser window (viewport)
     @returns {Node} Returns the topmost element at the specified position relative to the Top/Left of the browser window (viewport).
     */
    ElementAt: function (X, Y) {
        var Result = null;
        if (document.elementFromPoint) {
            Result = document.elementFromPoint(X, Y);
            if (Result && Result.tagName === tp.Undefined) {               // in case of text nodes (Opera)
                Result = Result.parentNode;
            }
        }
        return Result;
    }

};
Object.freeze(tp.Mouse);

/**
Information regarding a mouse event.
@see {@link https://developer.mozilla.org/en-US/docs/Web/API/MouseEvent|Mouse event}
@see {@link https://stackoverflow.com/questions/9262741/what-is-the-difference-between-pagex-y-clientx-y-screenx-y-in-javascript|Element location}
*/
tp.MouseInfo = class {

    /**
    Constructor
    @param {MouseEvent} e A MouseEvent
    */
    constructor(e) {
        this.Update(e);
    }

    /**
    Returns true if the alt key was down when the mouse event was fired.
    */
    get Alt() { return this.fAlt; }
    /**
    Returns true if the ctrl key was down when the mouse event was fired.
    */
    get Ctrl() { return this.fCtrl; }
    /**
    Returns true if the shift key was down when the mouse event was fired.
    */
    get Shift() { return this.fShift; }
    /**
    Returns true if the meta key was down when the mouse event was fired.
    */
    get Meta() { return this.fMeta; }

    /**
     Returns the mouse button that is currently pressed, using the defined button constants of the tp.Mouse class.
    */
    get Button() { return this.fButton; }
    /**
    Returns a bit-field with the mouse buttons currently pressed, using the defined button constants of the tp.Mouse class.
    */
    get Buttons() { return this.fButtons; }

    /**
    Returns true if the left mouse button is pressed
    */
    get IsLeft() { return tp.Mouse.LEFT === this.fButton; }
    /**
    Returns true if the middle (wheel) mouse button is pressed
    */
    get IsMid() { return tp.Mouse.MID === this.fButton; }
    /**
    Returns true if the right mouse button is pressed
    */
    get IsRight() { return tp.Mouse.RIGHT === this.fButton; }

    /**
    Returns an tp.Point with mouse position relative to the Top/Left of the fully rendered page (document)
    @see {@link https://stackoverflow.com/questions/9262741/what-is-the-difference-between-pagex-y-clientx-y-screenx-y-in-javascript|Element location}
    */
    get Document() { return this.fDocument; }
    /**
    Returns an tp.Point with mouse position relative to the Top/Left of the browser window (viewport)
    @see {@link https://stackoverflow.com/questions/9262741/what-is-the-difference-between-pagex-y-clientx-y-screenx-y-in-javascript|Element location}
    */
    get Viewport() { return this.fViewport; }
    /**
    Returns an tp.Point with mouse position relative to the Top/Left of the physical screen
    @see {@link https://stackoverflow.com/questions/9262741/what-is-the-difference-between-pagex-y-clientx-y-screenx-y-in-javascript|Element location}
    */
    get Screen() { return this.fScreen; }

    /**
    Updates the information of this instance using as source a specified mouse event.
    @param {MouseEvent} e A MouseEvent
    */
    Update(e) {
        if (tp.IsEmpty(e))
            return;

        this.fAlt = e.altKey === true;
        this.fCtrl = e.ctrlKey === true;
        this.fShift = e.shiftKey === true;
        this.fMeta = e.metaKey === true;

        this.fButton = tp.Mouse.Button(e);
        this.fButtons = tp.Mouse.Buttons(e);

        this.fDocument = tp.Mouse.ToDocument(e);
        this.fViewport = tp.Mouse.ToViewport(e);
        this.fScreen = tp.Mouse.ToScreen(e);
    }
    /**
    Returns an tp.Point with the difference between the position this instance represents and a specified mouse event.
    @param {MouseEvent} e A MouseEvent
    @returns {tp.Point} Returns an tp.Point with the difference between the position this instance represents and a specified mouse event.
    */
    Dif(e) {
        let P = tp.Mouse.ToViewport(e);

        P.X = P.X > this.Viewport.X ? P.X - this.Viewport.X : this.Viewport.X - P.X;
        P.Y = P.Y > this.Viewport.Y ? P.Y - this.Viewport.Y : this.Viewport.Y - P.Y;

        return P;
    }

};

//#endregion

//#region Screen

/**
Indicates the screen size mode (xsmall, small, medium, large)
// media all and (max-width :576px)
// media all and (min-width :577px)
// media all and (min-width :992px)
// media all and (min-width :1200px)
@class
@enum
*/
tp.ScreenMode = {
    None: 0,
    XSmall: 1,     //    0 ..  576
    Small: 2,      //  577 ..  991
    Medium: 4,     //  992 .. 1200
    Large: 8       // 1201 .. 
};
Object.freeze(tp.ScreenMode);

tp.ScreenWidthsMax = {
    XSmall: 576,
    Small: 992,
    Medium: 1200
};

/**
A static class helper for the viewport size and the screen mode (xsmall, small, medium, large) <br />
CAUTION: There are two viewports. <br />
Layout Viewport: What is available to be seen
Visual Viewport: What is currently visible
@see {@link https://developer.mozilla.org/en-US/docs/Glossary/layout_viewport}
@see {@link https://developer.mozilla.org/en-US/docs/Glossary/visual_viewport}
@see {@link https://www.quirksmode.org/mobile/viewports.html}
@class
*/
tp.Viewport = {
    Initialized: false,
    OldMode: tp.ScreenMode.None,
    Listeners: [],

    /**
    Initializes this class
    */
    Initialize() {
        if (!tp.Viewport.Initialized) {
            tp.Viewport.Initialized = true;

            tp.Viewport.OldMode = tp.Viewport.Mode;

            window.addEventListener("resize", function (ev) {
                tp.Viewport.ScreenSizeChanged();
            }, false);
        }
    },
    /**
    Returns the size of the viewport.
    @returns {tp.Size}  Returns the size of the viewport
    */
    GetSize() {
        //var w = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
        //var h = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
        //return new tp.Size(w, h);
        return new tp.Size(this.Width, this.Height);
    },
    /**
    Returns the the Top/Left of the viewport regarding the fully rendered document.
    @returns {tp.Point}  Returns the the Top/Left of the viewport regarding the fully rendered document.
    */
    GetPageOffset() {
        var X = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
        var Y = window.pageXOffset || document.documentElement.scrollLeft || document.body.scrollLeft;
        return new tp.Point(X, Y);
    },

    /**
     * Centers an element (i.e. a dialog box) in the window. <br />
     * The element's position should be fixed or absolute.
     * @param {string|Element} el Element or selector of the element to center in the window
     */
    CenterInWindow(el) {
        el = tp(el);
        if (tp.IsHTMLElement(el)) {

            let R = tp.BoundingRect(el);
            let L = (this.Width / 2) - (R.Width / 2);
            let T = (this.Height / 2) - (R.Height / 2);
            let Style = tp.GetComputedStyle(el);
            if (Style.position === 'absolute')
                T += window.pageYOffset;

            el.style.top = tp.px(T);
            el.style.left = tp.px(L);
        }
    },

    /**
    Called when the screen (viewport) size changes and informs the listeners.
    */
    ScreenSizeChanged() {
        let NewMode = tp.Viewport.Mode;
        let ModeFlag = NewMode !== tp.Viewport.OldMode;
        if (ModeFlag) {
            tp.Viewport.OldMode = NewMode;
        }

        // inform listeners
        let L;
        for (let i = 0, ln = tp.Viewport.Listeners.length; i < ln; i++) {
            L = tp.Viewport.Listeners[i];
            L.Func.call(L.Context, ModeFlag);
        }
    },

    /**
    Adds a listener. The listener will get notified when the screen (viewport) size changes.
    @param  {function} Func - A callback function, as function (ScreenModeFlag: boolean), to call when  the screen (viewport) size changes. 
    The ScreenModeFlag is true when the screen mode is changed as well.
    @param  {Context} [Context=null] - Optional. The context (this) to use when calling the callback function.
    @returns {tp.Listener} - Returns the newly create listener object.
    */
    AddListener(Func, Context = null) {
        let L = new tp.Listener(Func, Context);
        tp.Viewport.Listeners.push(L);
        return L;
    },
    /**
    Removes a listener added by a previous call to AddListener().
    @param {tp.Listener} Listener The listener to remove
    */
    RemoveListener(Listener) {
        var Index = this.Listeners.indexOf(Listener);

        if (Index !== -1) {
            this.Listeners.splice(Index, 1);
        }
    },

    get Mode() {
        var VS = tp.Viewport.GetSize();
        if (VS.Width <= tp.ScreenWidthsMax.XSmall) {
            return tp.ScreenMode.XSmall;
        } else if (VS.Width <= tp.ScreenWidthsMax.Small) {
            return tp.ScreenMode.Small;
        } else if (VS.Width <= tp.ScreenWidthsMax.Medium) {
            return tp.ScreenMode.Medium;
        } else {
            return tp.ScreenMode.Large;
        }
    },
    get IsXSmall() { return tp.Viewport.Mode === tp.ScreenMode.XSmall; },
    get IsSmall() { return tp.Viewport.Mode === tp.ScreenMode.Small; },
    get IsMedium() { return tp.Viewport.Mode === tp.ScreenMode.Medium; },
    get IsLarge() { return tp.Viewport.Mode === tp.ScreenMode.Large; },

    get Width() {
        return window.innerWidth && document.documentElement.clientWidth ?
            Math.min(window.innerWidth, document.documentElement.clientWidth) :
            window.innerWidth ||
            document.documentElement.clientWidth ||
            document.getElementsByTagName('body')[0].clientWidth;
    },

    get Height() {
        return window.innerHeight && document.documentElement.clientHeight ?
            Math.min(window.innerHeight, document.documentElement.clientHeight) :
            window.innerHeight ||
            document.documentElement.clientHeight ||
            document.getElementsByTagName('body')[0].clientHeight;
    }
};

//#endregion

//#region Overlay

/** A global (screen) ovelay DIV. Occupies the whole viewport and becomes the top-most element. */
tp.ScreenOverlay = class {

    /** Constructor 
     * This class creates a DIV that occupies the whole viewport and  becomes the top-most element.
     * @param {HTMLElement} [Parent=null] Optional. The parent of the DIV overlay.
     */
    constructor(Parent = null) {

        Parent = tp.IsHTMLElement(Parent) ? Parent : tp.Doc.body;
        this.Handle = tp.Div(Parent); // Parent.ownerDocument.createElement('div');

        this.Handle.id = tp.SafeId('tp-ScreenOverlay');

        let OverlayStyle = `display: flex;    
position: absolute;
top: 2px;
left: 2px;
right: 2px;
bottom: 2px;
justify-content: center;
align-items: center;
background: rgba(0, 0, 0, 0.07);
`;

        tp.StyleText(this.Handle, OverlayStyle);

        Parent.appendChild(this.Handle);
        tp.BringToFront(this.Handle);
    }

    /**
    Returns the z-index of the overlay
    */
    get ZIndex() {
        return Number(this.Handle.style.zIndex);
    }
    /**
    Gets or sets a boolean value indicating whether the overlay is visible
    */
    get Visible() {
        return this.Handle.style.display !== 'none';
    }
    set Visible(v) {
        v === true;

        if (this.Visible !== v) {
            if (v) {
                this.Handle.style.display = 'flex';
                tp.BringToFront(this.Handle);
            } else {
                this.Handle.style.display = 'none';
            }
        }
    }


    /** Creates, shows and returns the DIV. 
     @returns {HTMLDivElement} Returns the DIV.
     */
    Show() {
        this.Visible = true;
        return this.Handle;
    }
    Hide() {
        this.Visible = false;
        return this.Handle;
    }
    /**
     * Hides and destroys the DIV.
     * @returns {any} It always returns null.
     * */
    Dispose() {
        if (this.Handle && this.Handle.parentNode) {
            this.Handle.parentNode.removeChild(this.Handle);
        }
        this.Handle = null;
        return null;
    }
};
tp.ScreenOverlay.prototype.Handle = null;

//#endregion

//#region Spinner

/**
Static class for displaying a global spinner to the user while waiting for a lengthy operation to be completed.
The global spinner creates a DIV that occupies the whole viewport and  becomes the top-most element.
Above that DIV displays a snake-like spinner.
@class
*/
tp.Spinner = (function () {

    let SpinnerContainerStyle = `
    position: relative;
    width: auto;
    height: auto;
    background-color: transparent;
`;

    let SpinnerStyle = `
    height: 60px;
    width: 60px;
    border: 16px solid #034F84;
    border-right-color: transparent;
    border-radius: 50%;
    animation: tripous-global-spinner 1.5s infinite linear;
`;

    let KeyFrames = `
@keyframes tripous-global-spinner {
    0% { transform: rotate(0); }
    100% { transform: rotate(360deg); }
}
`;


    let DefaultSpinner = {
        Initialized: false,
        Overlay: null,
        divContainer: null,
        divSpinner: null,

        Initialize: function () {
            if (!this.Initialized) {
                this.Initialized = true;
                var style = document.createElement('style');
                style.type = 'text/css';
                style.innerHTML = KeyFrames;
                document.getElementsByTagName('head')[0].appendChild(style);
            }
        },

        Show: function () {
            this.Initialize();

            if (!this.Overlay) {
                this.Overlay = new tp.ScreenOverlay();
            }

            this.Overlay.Show();

            this.divContainer = tp.Div(this.Overlay.Handle);
            tp.StyleText(this.divContainer, SpinnerContainerStyle);

            this.divSpinner = tp.Div(this.divContainer);
            tp.StyleText(this.divSpinner, SpinnerStyle);
        },
        Hide: function () {
            this.Overlay.Dispose();
            this.Overlay = null;
            this.divSpinner = null;
            this.divContainer = null;
            this.divOverlay = null;
        }
    };



    let fCounter = 0;
    let fInstance = null;

    let DoShow = function () {
        if (fCounter >= 0) {
            fCounter++;

            if (fCounter === 1) {
                if (!tp.ImplementsInterface(fInstance, ['Show', 'Hide'])) {
                    fInstance = DefaultSpinner;
                }
                fInstance.Show();
            }
        }
    };
    let DoHide = function () {
        if (fCounter > 0) {
            fCounter--;
        }

        if (fCounter === 0) {
            if (tp.ImplementsInterface(fInstance, ['Show', 'Hide'])) {
                fInstance.Hide();
            }
            fInstance = null;
        }
    };


    return {
        /**
        Shows or hides the spinner, according to a specified flag. 
        Calling this method with the flag set to true, it shows the spinner in the first call, and the it just increases a counter.
        Calling with the flag set to false, hides the spinner.
        @param {boolean} Flag  True to show, false to hide.
        @memberof tp.Spinner
        @static
        */
        Show: function (Flag) {
            if (Flag === true)
                DoShow();
            else
                DoHide();
        },
        /**
        Forces the spinner to hide
        @memberof tp.Spinner
        @static
        */
        ForceHide: function () {
            fCounter = 0;
            DoHide();
        },
        /**
         * Sets the object that functions as a spinner.
         * @param {object} Implementation An instance that provides a Show, Hide, and Dispose methods.
         * @memberof tp.Spinner
         * @static
         */
        SetSpinnerImplementation: function (Implementation) {
            if (tp.ImplementsInterface(Implementation, ['Show', 'Hide', 'Dispose'])) {
                fInstance = Implementation;
            }
        },
        /**
        Returns true while the spinner is visible
        @memberof tp.Spinner
        @static
        */
        get IsShowing() { return !tp.IsEmpty(fInstance); },
        /**
        Returns a number indicating how many times the Show() method is called with its Flag set to true, before a call with the Flag set to fasle.
        @memberof tp.Spinner
        @static
        */
        get ShowingCounter() { return fCounter; }
    };

})();
/**
Shows or hides the global spinner, according to a specified flag.  
The global spinner creates a DIV that occupies the whole viewport and  becomes the top-most element.
Above that DIV displays a snake-like spinner.
Calling this method with the flag set to true, it shows the global spinner in the first call, and the it just increases a counter.
Calling with the flag set to false, hides the global spinner.
@param {boolean} Flag True to show, false to hide.
*/
tp.ShowSpinner = function (Flag) {
    tp.Spinner.Show(Flag);
};
/**
Forces the global spinner to hide
*/
tp.ForceHideSpinner = function () { tp.Spinner.ForceHide(); };
//#endregion

//#region  Colors

/** A static helper class for colors. <br />
 * Provides a list of color constants and a few helper methods.
 @class
 @static
 */
tp.Colors = (function () {

    return {
        AliceBlue: "#F0F8FF",
        AntiqueWhite: "#FAEBD7",
        Aqua: "#00FFFF",
        Aquamarine: "#7FFFD4",
        Azure: "#F0FFFF",
        Beige: "#F5F5DC",
        Bisque: "#FFE4C4",
        Black: "#000000",
        BlanchedAlmond: "#FFEBCD",
        Blue: "#0000FF",
        BlueViolet: "#8A2BE2",
        Brown: "#A52A2A",
        BurlyWood: "#DEB887",
        CadetBlue: "#5F9EA0",
        Chartreuse: "#7FFF00",
        Chocolate: "#D2691E",
        Coral: "#FF7F50",
        CornflowerBlue: "#6495ED",
        Cornsilk: "#FFF8DC",
        Crimson: "#DC143C",
        Cyan: "#00FFFF",
        DarkBlue: "#00008B",
        DarkCyan: "#008B8B",
        DarkGoldenRod: "#B8860B",
        DarkGray: "#A9A9A9",
        DarkGreen: "#006400",
        DarkKhaki: "#BDB76B",
        DarkMagenta: "#8B008B",
        DarkOliveGreen: "#556B2F",
        DarkOrange: "#FF8C00",
        DarkOrchid: "#9932CC",
        DarkRed: "#8B0000",
        DarkSalmon: "#E9967A",
        DarkSeaGreen: "#8FBC8F",
        DarkSlateBlue: "#483D8B",
        DarkSlateGray: "#2F4F4F",
        DarkTurquoise: "#00CED1",
        DarkViolet: "#9400D3",
        DeepPink: "#FF1493",
        DeepSkyBlue: "#00BFFF",
        DimGray: "#696969",
        DodgerBlue: "#1E90FF",
        FireBrick: "#B22222",
        FloralWhite: "#FFFAF0",
        ForestGreen: "#228B22",
        Fuchsia: "#FF00FF",
        Gainsboro: "#DCDCDC",
        GhostWhite: "#F8F8FF",
        Gold: "#FFD700",
        GoldenRod: "#DAA520",
        Gray: "#808080",
        Green: "#008000",
        GreenYellow: "#ADFF2F",
        HoneyDew: "#F0FFF0",
        HotPink: "#FF69B4",
        IndianRed: "#CD5C5C",
        Indigo: "#4B0082",
        Ivory: "#FFFFF0",
        Khaki: "#F0E68C",
        Lavender: "#E6E6FA",
        LavenderBlush: "#FFF0F5",
        LawnGreen: "#7CFC00",
        LemonChiffon: "#FFFACD",
        LightBlue: "#ADD8E6",
        LightCoral: "#F08080",
        LightCyan: "#E0FFFF",
        LightGoldenRodYellow: "#FAFAD2",
        LightGray: "#D3D3D3",
        LightGreen: "#90EE90",
        LightPink: "#FFB6C1",
        LightSalmon: "#FFA07A",
        LightSeaGreen: "#20B2AA",
        LightSkyBlue: "#87CEFA",
        LightSlateGray: "#778899",
        LightSteelBlue: "#B0C4DE",
        LightYellow: "#FFFFE0",
        Lime: "#00FF00",
        LimeGreen: "#32CD32",
        Linen: "#FAF0E6",
        Magenta: "#FF00FF",
        Maroon: "#800000",
        MediumAquaMarine: "#66CDAA",
        MediumBlue: "#0000CD",
        MediumOrchid: "#BA55D3",
        MediumPurple: "#9370DB",
        MediumSeaGreen: "#3CB371",
        MediumSlateBlue: "#7B68EE",
        MediumSpringGreen: "#00FA9A",
        MediumTurquoise: "#48D1CC",
        MediumVioletRed: "#C71585",
        MidnightBlue: "#191970",
        MintCream: "#F5FFFA",
        MistyRose: "#FFE4E1",
        Moccasin: "#FFE4B5",
        NavajoWhite: "#FFDEAD",
        Navy: "#000080",
        OldLace: "#FDF5E6",
        Olive: "#808000",
        OliveDrab: "#6B8E23",
        Orange: "#FFA500",
        OrangeRed: "#FF4500",
        Orchid: "#DA70D6",
        PaleGoldenRod: "#EEE8AA",
        PaleGreen: "#98FB98",
        PaleTurquoise: "#AFEEEE",
        PaleVioletRed: "#DB7093",
        PapayaWhip: "#FFEFD5",
        PeachPuff: "#FFDAB9",
        Peru: "#CD853F",
        Pink: "#FFC0CB",
        Plum: "#DDA0DD",
        PowderBlue: "#B0E0E6",
        Purple: "#800080",
        Red: "#FF0000",
        RosyBrown: "#BC8F8F",
        RoyalBlue: "#4169E1",
        SaddleBrown: "#8B4513",
        Salmon: "#FA8072",
        SandyBrown: "#F4A460",
        SeaGreen: "#2E8B57",
        SeaShell: "#FFF5EE",
        Sienna: "#A0522D",
        Silver: "#C0C0C0",
        SkyBlue: "#87CEEB",
        SlateBlue: "#6A5ACD",
        SlateGray: "#708090",
        Snow: "#FFFAFA",
        SpringGreen: "#00FF7F",
        SteelBlue: "#4682B4",
        Tan: "#D2B48C",
        Teal: "#008080",
        Thistle: "#D8BFD8",
        Tomato: "#FF6347",
        Turquoise: "#40E0D0",
        Violet: "#EE82EE",
        Wheat: "#F5DEB3",
        White: "#FFFFFF",
        WhiteSmoke: "#F5F5F5",
        Yellow: "#FFFF00",
        YellowGreen: "#9ACD32",

        /**
         Returns the <code>tp.Colors</code> list as an array of objects of the form <code>{ Text: ColorName, Value: ColorValue }</code> for use with ComboBoxes and ListBoxes
         @returns {object[]}  Returns <code>tp.Colors</code> list as an array of objects of the form <code>{ Text: ColorName, Value: ColorValue }</code> for use with ComboBoxes and ListBoxes
         @memberof tp.Colors 
         */
        ToOptionList() {
            var A = [];

            for (var Prop in tp.Colors) {
                if (typeof tp.Colors[Prop] !== 'function')
                    A.push({ Text: Prop, Value: tp.Colors[Prop] });
            }

            return A;
        },
        /**
         * Shades a color by a percent and returns the new color. <br />
         * taken from: {@link http://stackoverflow.com/questions/5560248/programmatically-lighten-or-darken-a-hex-color-or-rgb-and-blend-colors}
         * @param {string} Color A color in hex with a leading #
         * @param {number} Percent Number from -100 to 100. Negative numbers darken the color.
           @returns {string} Returns the new color.
           @memberof tp.Colors
         */
        Shade(Color, Percent) {
            var num = parseInt(Color.slice(1), 16);
            var amt = Math.round(2.55 * Percent);
            var R = (num >> 16) + amt;
            var G = (num >> 8 & 0x00FF) + amt;
            var B = (num & 0x0000FF) + amt;
            return "#" + (0x1000000 + (R < 255 ? R < 1 ? 0 : R : 255) * 0x10000 + (G < 255 ? G < 1 ? 0 : G : 255) * 0x100 + (B < 255 ? B < 1 ? 0 : B : 255)).toString(16).slice(1);
        },
        /**
         * Shades Color by Percent and returns the new color. <br />
         * taken from: {@link http://stackoverflow.com/questions/5560248/programmatically-lighten-or-darken-a-hex-color-or-rgb-and-blend-colors}
         * @param {string} Color A color in hex with a leading #
         * @param {number} Percent Number from -100 to 100. Negative numbers darken the color.
           @returns {string} Returns the new color.
           @memberof tp.Colors
         */
        Shade2(Color, Percent) {
            Percent = Percent / 100;
            var f = parseInt(Color.slice(1), 16);
            var t = Percent < 0 ? 0 : 255;
            var p = Percent < 0 ? Percent * -1 : Percent;
            var R = f >> 16;
            var G = f >> 8 & 0x00FF;
            var B = f & 0x0000FF;
            return "#" + (0x1000000 + (Math.round((t - R) * p) + R) * 0x10000 + (Math.round((t - G) * p) + G) * 0x100 + (Math.round((t - B) * p) + B)).toString(16).slice(1);
        },
        /**
         * Sets the gradient style of an element, if passed. Returns the gradient css style as string, ready to be assigned to the <code>background</code> style property of any element.
         * @param {string} Color A color.
         * @param {HTMLElement} [el=null] The element to apply the gradient style.
           @returns {string} Returns the gradient css style as string, ready to be assigned to the <code>background</code> style property of any element.
           @memberof tp.Colors
         */
        SetGradientStyle(Color, el = null) {

            var Colors = [];
            Colors.push(Color);

            var A = [];
            A.push(Color + ' 0%');

            var Stops = [];
            Stops = [22, 35, 42, 52];
            var C;
            var factor;
            var S;
            for (var i = 0; i < Stops.length; i++) {
                factor = (i + 1) * 3;

                C = tp.Colors.Shade(Color, -factor);
                Colors.push(C);

                S = C + ' ' + Stops[i] + '%';
                A.push(S);
            }

            S = A.join(', ');

            //background: #d8e0de; /* Old browsers */
            //background: -moz-linear-gradient(top,  #d8e0de 0%, #aebfbc 22%, #99afab 33%, #8ea6a2 50%, #829d98 67%, #4e5c5a 82%, #0e0e0e 100%); /* FF3.6+ */
            //background: -webkit-linear-gradient(top,  #d8e0de 0%,#aebfbc 22%,#99afab 33%,#8ea6a2 50%,#829d98 67%,#4e5c5a 82%,#0e0e0e 100%); /* Chrome10+,Safari5.1+ */
            //background: -o-linear-gradient(top,  #d8e0de 0%,#aebfbc 22%,#99afab 33%,#8ea6a2 50%,#829d98 67%,#4e5c5a 82%,#0e0e0e 100%); /* Opera 11.10+ */
            //background: -ms-linear-gradient(top,  #d8e0de 0%,#aebfbc 22%,#99afab 33%,#8ea6a2 50%,#829d98 67%,#4e5c5a 82%,#0e0e0e 100%); /* IE10+ */
            //background: linear-gradient(to bottom,  #d8e0de 0%,#aebfbc 22%,#99afab 33%,#8ea6a2 50%,#829d98 67%,#4e5c5a 82%,#0e0e0e 100%); /* W3C */
            var prefix = 'linear-gradient(to bottom,  ';
            S = prefix + S + ')';

            if (el instanceof HTMLElement) {
                var PropName = 'background-image';
                el.style.setProperty(PropName, S);
            }

            return S;
        }

    };

})();

//#endregion

//#region Misc functions


/**
Calls a function, if specified, using a context if not null, passing the specified arguments.
Returns whatever the called function returns
@param {Function} Func - A reference to a function, e.g. the function name 
@param {any} [Context] - The context to be used when calling the function 
@param {any} [Args] - The arguments to the function. 
@returns {any} Returns whatever the called function returns 
*/
tp.Call = function (Func, Context, ...Args) {
    if (tp.IsFunction(Func)) {
        if (Args.length > 0) {
            return Func.apply(Context, Args);
        } else {
            return Func.call(Context);
        }
    }

    return null;
};
/**
Returns true when B properties exist in A and have the same values in both
@param {object} A - The first object
@param {object} B - The second object
@returns {boolean} Returns true when B properties exist in A and have the same values in both
*/
tp.Equals = function (A, B) {
    if (A === B)
        return true;

    for (var Key in B) {
        if (B[Key] !== A[Key])
            return false;
    }

    return true;
};
/**
Creates and returns a MutationObserver instance
@see {@link https://developer.mozilla.org/en-US/docs/Web/API/MutationObserver}
@param {function} callback - A call-back function of the form  (mutations: MutationRecord[], observer: MutationObserver): void
@returns {MutationObserver} Returns a MutationObserver instance
*/
tp.CreateMutationObserver = function (callback) {
    if (MutationObserver)
        return new MutationObserver(callback);

    if (window['WebKitMutationObserver'])
        return new window['WebKitMutationObserver'](callback);

    return null;
};
/**
Returns the name (string) of an enumeration value of an enum type.
@example
    var Color = { Red: 1, Green: 2, Blue: 4 };
    var S = tp.EnumNameOf(Color, Color.Green); // returns 'Green'
@param {object} EnumType - The enumeration type, that is any plain javascript object with numeric constants.
@param {number} v - The integer value of an enum constant
@returns {string} Returns the name of an enum constant if found, else empty string.
*/
tp.EnumNameOf = function (EnumType, v) {
    if (typeof v === 'number') {
        for (var Key in EnumType) {
            if (EnumType[Key] === v)
                return Key;
        }
    }

    return '';
};
/**
 * Returns a string with characters separated by commas, where each character represents the type of an argument of a function.
 * Characters used are: s,b,n,o,a,f,d and r for string, boolean, number, object, array, function, date and regular expression.
 * @param {arguments} Args - The arguments of a function
 * @param {boolean} [IncludeEmpty=false] - Optional. When true then null/undefined arguments produce the e character (Empty).
 * @returns {string} Returns a string representing the 
 */
tp.Overload = function (Args, IncludeEmpty = false) {
    var a = arguments[0];
    var Result = [];
    for (var i = 0, ln = a.length; i < ln; i++) {
        if (tp.IsString(a[i])) {
            Result.push("s");
        }
        else if (tp.IsBoolean(a[i])) {
            Result.push("b");
        }
        else if (tp.IsNumber(a[i])) {
            Result.push("n");
        }
        else if (tp.IsObject(a[i])) {
            Result.push("o");
        }
        else if (tp.IsArray(a[i])) {
            Result.push("a");
        }
        else if (tp.IsFunction(a[i])) {
            Result.push("f");
        }
        else if (tp.IsDate(a[i])) {
            Result.push("d");
        }
        else if (tp.IsEmpty(a[i])) {
            if (IncludeEmpty)
                Result.push("e");
        }
        else if (tp.IsRegExp(a[i])) {
            Result.push("r");
        }
    }
    return Result.join();
};
/** Returns true if an instance implements an interface
 @param {object} Instance The instance to check.
 @param {string|string[]} MemberNames The names of methods, properties, fields, etc, of the interface.
 @returns {boolean} True if the specified instance implements all interface members
 */
tp.ImplementsInterface = function (Instance, MemberNames) {

    if (tp.IsValid(Instance)) {

        if (tp.IsString(MemberNames)) {
            let x = MemberNames;
            MemberNames = [];
            MemberNames.push(x);
        }

        if (tp.IsArray(MemberNames)) {

            for (let i = 0, ln = MemberNames.length; i < ln; i++) {
                if (!(MemberNames[i] in Instance)) {
                    return false;
                }
            }

            return true;
        }

    }

    return false;
};
/** 
Returns a random color.
@returns {string} A random color string in hex format.
*/
tp.RandomColor = function () {
    var S = '#';
    S += tp.ToHex(tp.Random(0, 0xFF));
    S += tp.ToHex(tp.Random(0, 0xFF));
    S += tp.ToHex(tp.Random(0, 0xFF));
    return S;
};
//#endregion



//---------------------------------------------------------------------------------------
// Classes
//---------------------------------------------------------------------------------------
tp.TriState = {
    False: 0,
    True: 1,
    Default: 2,
};
Object.freeze(tp.TriState);

//#region tp.StringBuilder

/** A class for constructing strings. The default line break is set to '\n' */
tp.StringBuilder = class StringBuilder {
    /**
     * A class for constructing strings. The default line break is set to '\n'
     * @param {string} LineBreak - Optional.  The line break to use. Defaults to \n .
     */
    constructor(LineBreak = '\n') {
        this.fData = '';
        this.fLB = LineBreak || '\n';
    }

    /* properties */
    /** Returns the length of the internal string 
     */
    get Length() { return this.fData.length; }
    /** True if the internal string is empty 
    */
    get IsEmpty() { return this.fData.length === 0; }
    /**
    Gets or sets the line break. Defaults to '\n'
    */
    get LineBreak() { return this.fLB; }
    set LineBreak(v) { this.fLB = v; }

    /* public */
    /**  
    Sets the internal string to an empty string.
    */
    Clear() {
        this.fData = '';
    }
    /**
    Appends a value
    @param {any} v - The value to append. 
    */
    Append(v) {
        if (tp.IsString(v))
            this.fData += v.toString();
    }
    /**
    Appends a value and a line break
    @param {any} v - Optional. The value to append. If not specified a line break is added.
    */
    AppendLine(v) {
        if (tp.IsString(v)) {
            this.fData += v.toString();
        }

        this.fData += this.LineBreak;
    }
    /**
    Inserts a value at a specified index in the internal string
    @param {number} Index - The index in the internal string.
    @param {any} v - The value to append. 
    */
    Insert(Index, v) {
        if (tp.IsValid(v)) {
            this.fData = tp.InsertText(v.toString(), this.fData, Index);
        }
    }
    /**
    Replaces a value with another value in the internal string
    @param {string} OldValue - The string to be replace.
    @param {string} NewValue - The replacer string.
    @param {boolean} CI - CI (Case-Insensitive) can be true (the default) or false
    */
    Replace(OldValue, NewValue, CI = true) {
        this.fData = tp.ReplaceAll(this.fData, OldValue, NewValue, CI);
    }
    /** 
    Returns the internal string 
    @returns {string} - Returns the internal string.
    */
    ToString() {
        return this.fData;
    }
};
//#endregion

//#region  tp.Bf
/** Bit-Field (set) helper full static class. 
 @class
 */
tp.Bf = {
    /**
    Union (or). Returns the union of A and B.  
    The result is a new set containing ALL the elements of A and B.  
    @param {number} A -
    @param {number} B -
    @returns {number} - 
    */
    Union: function (A, B) { return A | B; },
    /**
    Intersection (and). Returns the intersection of A and B. 
    The result is a new set containing just the COMMON the elements of A and B.  
    @param {number} A -
    @param {number} B -
    @returns {number} -
     */
    Junction: function (A, B) { return A & B; },
    /**
    Difference (xor). Returns the difference of A and B. 
    The result is a new set containing the NON COMMON the elements of A and B. 
    @param {number} A -
    @param {number} B -
    @returns {number} -
     */
    Dif: function (A, B) { return A ^ B; },
    /**
     * Subtraction (-).Returns the subtraction of B from A. 
    The result is a new set containing the the elements of A MINUS the elements of B. 
    @param {number} A -
    @param {number} B -
    @returns {number} -
    */
    Subtract: function (A, B) { return A ^ (A & B); },
    /**
    Membership (in). Returns true if A in B. A can be a single value or a set. 
    Returns true if ALL elements of A are in B.  
    @param {number} A -
    @param {number} B -
    @returns {number} -
     */
    Member: function (A, B) { return 0 === A ? false : (A & B) === A; },
    /**
    Membership (in). Returns true if A in B. A can be a single value or a set. 
    Returns true if ALL elements of A are in B.  
    @param {number} A -
    @param {number} B -
    @returns {number} -
     */
    In: function (A, B) { return 0 === A ? false : (A & B) === A; },

    /**
    Returns true if A is null or 0.
    @param {number} A - 
    @returns {boolean} -
     */
    IsEmpty: function (A) { return A === tp.Undefined || A === null || Number(A) === 0; },

    /**
     * Returns a string representing a bit-field (set) value 
    Example:
    var Color = { Red: 1, Green: 2, Blue: 4 };
    var S = tp.Bf.EnumToString(Color, Color.Red | Color.Blue); // returns 'Red, Blue'
     * @param   {Object} SetType - The Set type, i.e. tp.Anchor
     * @param   {number} Value - The integer value to be converted to string, i.e 5 (returns Top, Left)
     @returns {string} -
     */
    SetToString: function (SetType, Value) {
        var Result = [];

        for (var Prop in SetType) {
            if (tp.Bf.Member(SetType[Prop], Value)) {
                Result.push(Prop);
            }
        }

        var S = Result.join(', ');
        return S;
    }
};
//#endregion

//#region tp.Listener
/** A listener class. A listener demands a callback function, at least, and perhaps a context (this) object for the call. */
tp.Listener = class {

    /**
    Constructor
    @param {function} [Func=null] - The callback function
    @param {object} [Context=null] - The context (this) of the callback function
    */
    constructor(Func = null, Context = null) {
        this.Func = Func;
        this.Context = Context;
    }

    // NOTE: Firefox and Edge do not support fields yet. 
};
/** The callback function */
tp.Listener.prototype.Func = null;
/** The context (this) of the callback function */
tp.Listener.prototype.Context = null;
//#endregion

//#region  tp.Local
/**
 * A full static helper class for the local storage. 
    Data stored in localStorage has no expiration time
 */
tp.Local = {};

/**
 * Clear the storage
 */
tp.Local.Clear = function () {
    if (typeof Storage !== tp.Undefined) {
        localStorage.clear();
    }
};
/**
 * Remove an entry from the storage
 * @param {string} Key A key denoting the entry
 */
tp.Local.Remove = function (Key) {
    if (typeof Storage !== tp.Undefined) {
        localStorage.removeItem(Key);
    }
};
/**
 * Get a value under a key from storage, if any, else return a default value.
 * @param {string} Key A key denoting the entry
 * @param {string} [Default=null] A default value to return if the specified key is not found.
 * @returns {string} Returns the value.
 */
tp.Local.Get = function (Key, Default = null) {
    var Result = null;
    if (typeof Storage !== tp.Undefined) {
        Result = localStorage.getItem(Key);
    }
    if (tp.IsBlank(Result))
        Result = Default;
    return Result;
};
/**
 * Set a value under a key in storage
 * @param {string} Key A key denoting the entry
 * @param {string} v A value
 */
tp.Local.Set = function (Key, v) {
    if (typeof Storage !== tp.Undefined) {
        localStorage.setItem(Key, v);
    }
};
/**
 * Get an object value under a key from storage, if any, else return a default value.
 * @param {string} Key A key denoting the entry
 * @param {object} [Default=null] A default value to return if the specified key is not found.
 * @returns {object} Returns the value.
 */
tp.Local.GetObject = function (Key, Default = null) {

    var Result = Default;

    var Text = tp.Local.Get(Key, null);
    if (tp.IsString(Text)) {
        Result = JSON.parse(Text);
    }
    return Result;
};
/**
* Set an object value under a key in storage
* @param {string} Key A key denoting the entry
* @param {object} v A value
*/
tp.Local.SetObject = function (Key, v) {
    if (!tp.IsEmpty(v)) {
        var S = JSON.stringify(v);
        tp.Local.Set(Key, S);
    }
};
//#endregion

//#region tp.Session
/**
 * A full static helper class for the local storage. 
    Data stored in sessionStorage has no expiration time
 * @class
 * @static
 * @hideconstructor
 */
tp.Session = {};

/**
 * Clear the storage
 */
tp.Session.Clear = function () {
    if (typeof Storage !== tp.Undefined) {
        sessionStorage.clear();
    }
};
/**
 * Remove an entry from the storage
 * @param {string} Key A key denoting the entry
 */
tp.Session.Remove = function (Key) {
    if (typeof Storage !== tp.Undefined) {
        sessionStorage.removeItem(Key);
    }
};
/**
 * Get a value under a key from storage, if any, else return a default value.
 * @param {string} Key A key denoting the entry
 * @param {string} [Default=null] A default value to return if the specified key is not found.
 * @returns {string} Returns the value.
 */
tp.Session.Get = function (Key, Default = null) {
    var Result = null;
    if (typeof Storage !== tp.Undefined) {
        Result = sessionStorage.getItem(Key);
    }
    if (tp.IsBlank(Result))
        Result = Default;
    return Result;
};
/**
 * Set a value under a key in storage
 * @param {string} Key A key denoting the entry
 * @param {string} v A value
 */
tp.Session.Set = function (Key, v) {
    if (typeof Storage !== tp.Undefined) {
        sessionStorage.setItem(Key, v);
    }
};
/**
 * Get an object value under a key from storage, if any, else return a default value.
 * @param {string} Key A key denoting the entry
 * @param {object} [Default=null] A default value to return if the specified key is not found.
 * @returns {object} Returns the value.
 */
tp.Session.GetObject = function (Key, Default = null) {

    var Result = Default;

    var Text = tp.Session.Get(Key, null);
    if (tp.IsString(Text)) {
        Result = JSON.parse(Text);
    }
    return Result;
};
/**
* Set an object value under a key in storage
* @param {string} Key A key denoting the entry
* @param {object} v A value
*/
tp.Session.SetObject = function (Key, v) {
    if (!tp.IsEmpty(v)) {
        var S = JSON.stringify(v);
        tp.Session.Set(Key, S);
    }
};


//#endregion

//#region  tp.Point
/**
 * A point class
 */
tp.Point = class {

    /**
     * constructor
     * @param {number} [X=0] - The left of the point
     * @param {number} [Y=0] - The top of the point
     */
    constructor(X = 0, Y = 0) {
        this.X = tp.Truncate(X || 0);
        this.Y = tp.Truncate(Y || 0);
    }

    /** Field */
    //X: number;
    /** Field */
    //Y: number;

    /* public */

    /**
     * Clears this instance
     */
    Clear() {
        this.X = 0;
        this.Y = 0;
    }
    /**
     * Adds to this instance
     * @param {number} X -
     * @param {number} Y -
     */
    Add(X, Y) {
        this.X += tp.Truncate(X);
        this.Y += tp.Truncate(Y);
    }
    /**
     * Subtracts from this instance
     * @param {number} X -
     * @param {number} Y -
     */
    Subtract(X, Y) {
        this.X -= tp.Truncate(X);
        this.Y -= tp.Truncate(Y);
    }
    /**
     * Returns true if this instance equals to the specified values
     * @param {number} X -
     * @param {number} Y -
     * @returns {boolean}  Returns true if this instance equals to the specified values
     */
    Equals(X, Y) {
        X = tp.Truncate(X);
        Y = tp.Truncate(Y);

        return this.X === X && this.Y === Y;
    }
    /**
    * Returns true if this instance is greater than a specified point, in both axis
    * @param {number} X -
    * @param {number} Y -
    * @returns {boolean}  Returns true if this instance is greater than a specified point, in both axis
    */
    Greater(X, Y) { return this.X >= X && this.Y >= Y; }
    /**
    * Returns true if this instance is less than a specified point, in both axis
    * @param {number} X -
    * @param {number} Y -
    * @returns {boolean}  Returns true if this instance is less than a specified point, in both axis
    */
    Less(X, Y) { return this.X <= X && this.Y <= Y; }
    /**
     True if the passed arguments represent two points and the point this instance represents is greater from the first passed point and is lesser from the second passed point.
     @param {number} X1 - The X of the first point
     @param {number} Y1 - The Y of the first point
     @param {number} X2 - The X of the second point
     @param {number} Y2 - The Y of the second point
     @returns {boolean} Returns true if this point is between the two specified points
     */
    IsInBetween(X1, Y1, X2, Y2) { return this.Greater(X1, Y1) && this.Less(X2, Y2); }
    /** 
     *  @return {string} Returns a string representation of this instance 
     * */
    toString() { return tp.Format("x={0}, y={1}", this.X, this.Y); }


};
/** Returns true if a point is contained by a rectangle
 * @param {tp.Point} P - The point to test
 * @param {tp.Rect} R - The rectangle to check
 * @returns {boolean} Returns true if a point is contained by a rectangle
 */
tp.Point.PointInRect = function (P, R) {
    return P.X >= R.X &&
        P.X <= R.X + R.Width &&
        P.Y >= R.Y &&
        P.Y <= R.Y + R.Height;
};
//#endregion

//#region  tp.Rect
/**
 * A rectangle class
 */
tp.Rect = class {
    /**
     * constructor
     * @param {number} [X=0] - The left of the rectangle
     * @param {number} [Y=0] - The top of the rectangle
     * @param {number} [Width=0] - The width of the rectangle 
     * @param {number} [Height=0] - The height of the rectangle
     */
    constructor(X = 0, Y = 0, Width = 0, Height = 0) {
        this.X = tp.Truncate(X || 0);
        this.Y = tp.Truncate(Y || 0);
        this.Width = tp.Truncate(Width || 0);
        this.Height = tp.Truncate(Height || 0);
    }

    /* fields and poperties
    X: number;
    Y: number;
    Width: number;
    Height: number;
   */

    get Right() { return this.X + this.Width; }
    set Right(v) {
        v = tp.Truncate(v || 0);
        this.Width = v - this.X;
    }

    get Bottom() { return this.Y + this.Height; }
    set Bottom(v) {
        v = tp.Truncate(v || 0);
        this.Height = v - this.Y;
    }


    /* public */
    /**
     * Clears this instance
     */
    Clear() {
        this.X = 0;
        this.Y = 0;
        this.Width = 0;
        this.Height = 0;
    }
    /**
     * Returns true if this instance equals to the specified values
     * @param {number} X -
     * @param {number} Y -
     * @param {number} Width -
     * @param {number} Height -
       @returns {boolean} Returns true if this instance equals to the specified values
     */
    Equals(X, Y, Width, Height) {
        X = tp.Truncate(X);
        Y = tp.Truncate(Y);
        Width = tp.Truncate(Width);
        Height = tp.Truncate(Height);

        return this.X === X && this.Y === Y && this.Width === Width && this.Height === Height;
    }
    /**
     * Returns true if this instance contains a specified point or rectangle
     * @param {any} Args Could be one of the following
     * 1. tp.Point
     * 2. tp.Rect
     * 3. X, Y
     * 4. X, Y, Width, Height
     * @returns {boolean}  Returns true if this instance contains a specified point or rectangle
     */
    Contains(Args) {
        var X, Y, Width, Height, o;
        if (arguments.length === 1) {
            o = arguments[0];

            if ("Width" in o) {
                let R = o;

                return this.X <= R.X
                    && this.Y <= R.Y
                    && this.Width <= R.Width
                    && this.Height <= R.Height;
            } else {
                let P = o;

                return P.X >= this.X
                    && P.X <= this.X + this.Width
                    && P.Y >= this.Y
                    && P.Y <= this.Y + this.Height;
            }

        } else if (arguments.length === 2) {

            X = tp.Truncate(arguments[0]);
            Y = tp.Truncate(arguments[1]);

            return X >= this.X
                && X <= this.X + this.Width
                && Y >= this.Y
                && Y <= this.Y + this.Height;

        } else if (arguments.length === 4) {

            X = tp.Truncate(arguments[0]);
            Y = tp.Truncate(arguments[1]);
            Width = tp.Truncate(arguments[2]);
            Height = tp.Truncate(arguments[3]);

            return this.X <= X
                && this.Y <= Y
                && this.Width <= Width
                && this.Height <= Height;
        }

        return false;
    }
    /**
     * Inflates this instance.
     * This method enlarges this rectangle, not a copy of it. 
     * The rectangle is enlarged in both directions along an axis. 
     * For example, if a 50 by 50 rectangle is enlarged by 50 in the x-axis, the resultant rectangle will be 150 units long (the original 50, the 50 in the minus direction, and the 50 in the plus direction) maintaining the rectangle's geometric center.
     * @param {number} Width - The width to inflate
     * @param {number} Height - the height to inflate
     */
    Inflate(Width, Height) {
        this.X = this.X - Width;
        this.Y = this.Y - Height;
        this.Width = this.Width + (2 * Width);
        this.Height = this.Height + (2 * Height);
    }
    /**
     * True if this instance and a specified rectangle have at least one common point.
     * @param {any} Args Could be one of the following
     * 1. tp.Rect
     * 2. X, Y, Width, Height
     * @returns {boolean} True if this instance and a specified rectangle have at least one common point.
     */
    IntersectsWith(Args) {

        if (arguments.length === 1) {
            let R = arguments[0];

            return R.X < this.X + this.Width
                && this.X < R.X + R.Width
                && R.Y < this.Y + this.Height
                && this.Y < R.Y + R.Height;

        } else {
            var X = tp.Truncate(arguments[0]);
            var Y = tp.Truncate(arguments[1]);
            var Width = tp.Truncate(arguments[2]);
            var Height = tp.Truncate(arguments[3]);

            return X < this.X + this.Width
                && this.X < X + Width
                && Y < this.Y + this.Height
                && this.Y < Y + Height;
        }
    }
    /**
     * Makes this rectangle to be the result of the intersection between this rectangle and a specified rectangle. 
     * If there is no intersection between the two rectangles, nothing happens.
     * @param {any} Args Could be one of the following
     * 1. tp.Rect
     * 2. X, Y, Width, Height
     */
    Intersect(Args) {

        var X1, X2, Y1, Y2;

        if (arguments.length === 1) {
            let R = arguments[0];

            X1 = Math.max(this.X, R.X);
            X2 = Math.min(this.X + this.Width, R.X + R.Width);
            Y1 = Math.max(this.Y, R.Y);
            Y2 = Math.min(this.Y + this.Height, R.Y + R.Height);


        } else {

            var X = tp.Truncate(arguments[0]);
            var Y = tp.Truncate(arguments[1]);
            var Width = tp.Truncate(arguments[2]);
            var Height = tp.Truncate(arguments[3]);

            X1 = Math.max(this.X, X);
            X2 = Math.min(this.X + this.Width, X + Width);
            Y1 = Math.max(this.Y, Y);
            Y2 = Math.min(this.Y + this.Height, Y + Height);
        }

        if (X2 >= X1 && Y2 >= Y1) {
            this.X = X1;
            this.Y = Y1;
            this.Width = X2 - X1;
            this.Height = Y2 - Y1;
        }
    }
    /**
     * Moves the location (X, Y) of this instance to a specified location
     * @param {any} Args Could be one of the following
     * 1. tp.Point
     * 2. X, Y
     */
    Offset(Args) {
        if (arguments.length === 2) {
            this.X = arguments[0];
            this.Y = arguments[1];
        } else {
            this.X = arguments[0].X;
            this.Y = arguments[0].Y;
        }
    }
    /**
     * Makes this rectangle to be the result of the union between this rectangle and a specified rectangle.
     * @param {any} Args Could be one of the following
     * 1. tp.Rect
     * 2. X, Y, Width, Height
     */
    Union(Args) {

        var X1, X2, Y1, Y2;

        if (arguments.length === 1) {
            let R = arguments[0];

            X1 = Math.max(this.X, R.X);
            X2 = Math.min(this.X + this.Width, R.X + R.Width);
            Y1 = Math.max(this.Y, R.Y);
            Y2 = Math.min(this.Y + this.Height, R.Y + R.Height);

        } else {

            var X = tp.Truncate(arguments[0]);
            var Y = tp.Truncate(arguments[1]);
            var Width = tp.Truncate(arguments[2]);
            var Height = tp.Truncate(arguments[3]);

            X1 = Math.max(this.X, X);
            X2 = Math.min(this.X + this.Width, X + Width);
            Y1 = Math.max(this.Y, Y);
            Y2 = Math.min(this.Y + this.Height, Y + Height);
        }

        this.X = X1;
        this.Y = Y1;
        this.Width = X2 - X1;
        this.Height = Y2 - Y1;
    }

    /**
    Returns a string representation of this instance
    @returns {string} Returns a string representation of this instance
    */
    toString() { return tp.Format("x={0}, y={1}, width={2}, height={3}", this.X, this.Y, this.Width, this.Height); }
};
/**
 Creates and returns a tp.Rect based on the bounding client rectangle of a specified element or based on a specified DOM ClientRect.
 The specified argument could be either a DOM ClientRect instance or a HTMLElement instance.
 @param {ClientRect|Element} ElementOrClientRect HTMLElement or DOM ClientRect
 @returns {tp.Rect} Returns a tp.Rect based on the specified argument.
 */
tp.Rect.FromClientRect = function (ElementOrClientRect) {
    if (ElementOrClientRect instanceof HTMLElement) {
        let R = ElementOrClientRect.getBoundingClientRect();
        return new tp.Rect(R.left, R.top, R.width, R.height);
    }

    if (tp.ImplementsInterface(ElementOrClientRect, ['left', 'top', 'width', 'height'])) {
        return new tp.Rect(ElementOrClientRect.left, ElementOrClientRect.top, ElementOrClientRect.width, ElementOrClientRect.height);
    }

    return new tp.Rect();
};
//#endregion

//#region  tp.Size
/**
 * A size class
 */
tp.Size = class {

    /**
    * constructor
    * @param {number} [Width=0] The width of the size
    * @param {number} [Height=0] The height of the size
    */
    constructor(Width = 0, Height = 0) {
        this.Width = tp.Truncate(Width || 0);
        this.Height = tp.Truncate(Height || 0);
    }

    /** Field */
    //Width: number;
    /** Field */
    // Height: number;


    /* public */

    /**
     * Clears this instance
     */
    Clear() {
        this.Width = 0;
        this.Height = 0;
    }
    /**
     * Adds to this instance
     * @param {number} Width - The width to add
     * @param {number} Height - The height to add
     */
    Add(Width, Height) {
        this.Width += tp.Truncate(Width);
        this.Height += tp.Truncate(Height);
    }
    /**
     * Subtracts from this instance
     * @param {number} Width - The width to subtract
     * @param {number} Height - The height to subtract
     */
    Subtract(Width, Height) {
        this.Width -= tp.Truncate(Width);
        this.Height -= tp.Truncate(Height);
    }
    /**
     * Returns true if this instance equals to the specified values
     * @param {number} Width - The width to compare
     * @param {number} Height - The height to compare
       @returns {boolean} Returns true if this instance equals to the specified values
     */
    Equals(Width, Height) {
        Width = tp.Truncate(Width);
        Height = tp.Truncate(Height);

        return this.Width === Width && this.Height === Height;
    }
    /**
    Returns a string representation of this instance
    @returns {string} Returns a string representation of this instance
    */
    toString() { return tp.Format("width={0}, height={1}", this.Width, this.Height); }
};
//#endregion

//#region tp.Names
/** A static class for construction element names 
 @class
 @static
 */
tp.Names = (function () {
    let items = {};
    let counter = 2000; // do not collide with Asp.Net Core auto Ids

    return {
        /**
         * Constructs and returns a string based on Prefix and an auto-inc counter associated to Prefix.
         * It stores passed prefixes in order to auto-increment the associated counter.
         * It Prefix is null or empty, it just returns an auto-inc number as string.
         * WARNING: NOT case-sensitive.
         * @param {string} [Prefix=''] The prefix to prepend in the returned name.
         * @returns {string} Returns the new name.
         * @memberof tp.Names
         * @static
        */
        Next: function (Prefix = '') {

            if (!tp.IsNullOrWhitespace(Prefix)) {
                var ucPrefix = Prefix.toUpperCase();
                if (!(ucPrefix in items)) {
                    items[ucPrefix] = 2000; // do not collide with Asp.Net Core auto Ids
                }
                var V = items[ucPrefix]++;
                return Prefix + V.toString();
            }

            counter++;
            return counter.toString();
        }
    };
})();

/**
Constructs and returns a string based on a specified prefix and an internal auto-inc counter associated to thant prefix. 
It stores passed prefixes in order to auto-increment the associated counter. 
It the prefix is null or empty, it just returns an auto-inc number as string. 
WARNING: NOT case-sensitive.
@param {string} [Prefix=''] The prefix to prepend in the returned name.
@returns {string} Returns the new name.
*/
tp.NextName = function (Prefix = '') {
    return tp.Names.Next(Prefix);
};
/**
 Constructs and returns an Id, based on a specified prefix. If prefix is null or empty, tp.Prefix is used.  
 WARNING: NOT case-sensitive.
@param {string} [Prefix=tp.Prefix] The prefix to prepend in the returned Id.
@returns {string} Returns the new Id.
 */
tp.SafeId = function (Prefix = tp.Prefix) {
    if (tp.IsBlank(Prefix))
        Prefix = tp.Prefix;

    var S = tp.NextName(Prefix);
    S = tp.ReplaceAll(S, '.', '-');

    return S;
};
//#endregion

//#region  tp.Environment
tp.Environment = {

    Initialize: function () {
    },         

    get ScrollbarSize() {
        if (!tp.Environment.fScrollbarSize) {
            tp.Environment.fScrollbarSize = new tp.Size();

            var outer = tp.Doc.createElement("div");
            outer.style.visibility = "hidden";
            outer.style.width = "100px";
            outer.style.height = "100px";
            outer.ownerDocument.body.appendChild(outer);

            var widthNoScroll = outer.offsetWidth;
            var heightNoScroll = outer.offsetHeight;

            // force scrollbars
            outer.style.overflow = "scroll";

            // add innerdiv
            var inner = outer.ownerDocument.createElement("div");
            inner.style.width = "100%";
            inner.style.height = "100%";
            outer.appendChild(inner);

            var widthWithScroll = inner.offsetWidth;
            var heightWithScroll = inner.offsetHeight;

            // remove divs
            outer.parentNode.removeChild(outer);

            tp.Environment.fScrollbarSize.Width = widthNoScroll - widthWithScroll;
            tp.Environment.fScrollbarSize.Height = heightNoScroll - heightWithScroll;
        }

        return tp.Environment.fScrollbarSize;
    }
};
//#endregion

//#region tp.IBroadcasterListener
/**
Represents a tp.Broadcaster listener, that is a subscriber to tp.Broadcaster events.
*/
tp.IBroadcasterListener = class {

    /** 
    Called by tp.Broadcaster to notify a listener about an event.
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    @returns {any} Returns a value or null.
    */
    BroadcasterFunc(Args) { return null; }
};
//#endregion

//#region tp.Broadcaster
/**
A static helper. Represents an object that sends event notifications to its subscribed listeners. <br />
A listener is an object which provides a <code>function  BroadcasterFunc(Args: tp.EventArgs || {}): void</code>
@class
@static
*/
tp.Broadcaster = (function () {
    let listeners = [];

    /**
     * Prepares event arguments and returns a <code>tp.EventArgs</code> object
     * @param {string} EventName The event name
     * @param {object} [Sender=null] The sender
     * @param {object} [Args=null] The event args
     * @returns {tp.EventArgs} Returns a <code>tp.EventArgs</code> object.
     */
    let PrepareEventArgs = function (EventName, Sender = null, Args = null) {
        if (typeof Sender === tp.Undefined) { Sender = null; }
        if (typeof Args === tp.Undefined) { Args = null; }

        Sender = Sender || tp.Broadcaster;

        let Result = Args instanceof tp.EventArgs ? Args : new tp.EventArgs(Args);
        Result.Sender = Args.Sender || Sender;
        Result.EventName = EventName;
        Result.IsBroadcasterMessage = true;

        return Result;
    };

    return {
        /**
         * Type guard. A broadcaster listener should provide a <code>BroadcasterFunc(Args: tp.EventArgs)</code> method.
         * @param {any} v The object to check
         * @returns {boolean} Returns true if the object passes the check.
         * @memberof tp.Broadcaster
         * @static
         */
        IsBroadcasterListener(v) { return !tp.IsEmpty(v) && tp.IsFunction(v['BroadcasterFunc']); },
        /**
        Adds a broadcaster listener in the internal subscribers list
        @param {object} Listener An object that provides a <code>BroadcasterFunc(Args: tp.EventArgs)</code> method.
        @memberof tp.Broadcaster
        @static
        */
        Add(Listener) {
            if (this.IsBroadcasterListener(Listener) && !tp.ListContains(listeners, Listener))
                listeners.push(Listener);
        },
        /**
        Removes a listener from the internal subscribers list
        @param {object} Listener An object that provides a <code>BroadcasterFunc(Args: tp.EventArgs)</code> method.
        @memberof tp.Broadcaster
        @static
        */
        Remove(Listener) {
            tp.ListRemove(listeners, Listener);
        },
        /**
        Sends a notification message to all of the subcribers synchronously.
         @param {string} EventName - The name of the event
         @param {object} [Sender=null] - Optional. The sender. Defaults to this object
         @param {object|tp.EventArgs} [Args=null] - Optional. Any additional information, as a javascript object. Could be a <code>tp.EventArgs</code> object.
         @memberof tp.Broadcaster
         @static
        */
        Send(EventName, Sender = null, Args = null) {
            let ea = PrepareEventArgs(EventName, Sender, Args);

            var Listener;
            for (var i = 0, ln = listeners.length; i < ln; i++) {
                Listener = listeners[i];
                Listener.BroadcasterFunc.call(Listener, ea);
            }
        }
    };


})();
//#endregion

//#region  tp.Debug
/**
 * A static helper for displaying debug messages.
 * As function displays the message to the console.
 * @class
 * @param {any} o The object to display
 */
tp.Debug = function (o) { tp.Debug.Log(o); };
/**
 * Converts an object to JSON
 * @param {any} o The object to convert
 * @return {string} Returns the JSON representation of the specified object
 */
tp.Debug.AsText = function (o) { return tp.IsEmpty(o) ? "..." : JSON.stringify(o, null, " "); };
/**
 * Converts a specified object to JSON and displays it to a dialog box
 * @param {any} o The object to display
 */
tp.Debug.Show = function (o) {
    var S = "NULL";
    if (!tp.IsEmpty(o)) {
        S = tp.IsSimple(o) ? o.toString() : tp.Debug.AsText(o);
    }
    tp.InfoNote(S);
};
/**
 * Converts a specified object to JSON and displays it to the console
 * @param {any} o The object to display
 */
tp.Debug.Log = function (o) {
    var S = "NULL";
    if (!tp.IsEmpty(o)) {
        if (tp.IsString(o))
            S = o;
        else if (tp.IsObject(o))
            S = tp.Debug.AsText(o);
        else
            S = o;
    }
    log(S);
};
//#endregion


//#region  tp.Log

/**
 * A static helper class for displaying debug messages to a div with id='LogDiv'.
 */
tp.Log = {};
/** Clears the log div */
tp.Log.Clear = function () {
    tp.LogDiv.innerHTML = '';
};
/**
 Appends a text line to the log div
 @param {string} Text The text line to append
 */
tp.Log.Line = function (Text) {
    var S = tp.LogDiv.innerHTML ? tp.LogDiv.innerHTML : '';
    tp.LogDiv.innerHTML = S + Text + '<br />';
};
/**
 Replaces the content of the log div with a specified text
 @param {string} Text The text
 */
tp.Log.Text = function (Text) {
    tp.LogDiv.innerHTML = Text;
};

//#endregion


//---------------------------------------------------------------------------------------
// list classes
//---------------------------------------------------------------------------------------

//#region tp.Enumerator

/** 
A "classic" enumerator class.
@example
    let E = new tp.Enumerator([1, 2, 3]);
    let Sum: number = 0;
    while (E.MoveNext()) {
        Sum += E.Current;
    }
*/
tp.Enumerator = class {
    /**
     * constructor
     * @param {ArrayLike} List An array-like object
     */
    constructor(List) {
        this.Index = -1;
        this.List = List;
    }

    // Index = -1;
    // List: IArrayLike = null;

    /**
    Gets the element in the collection at the current position of the enumerator.
    Returns the current element in the collection
    @type {any} 
    */
    get Current() { return this.GetCurrent(); }

    /**
    Advances the enumerator to the next element of the collection. 
    Returns true if there are more elements in the collection, else false.
    @returns {boolean} Returns true if there are more elements in the collection, else false.
    */
    MoveNext() {
        if (tp.InRange(this.List, this.Index + 1)) {
            this.Index++;
            return true;
        }
        return false;
    }
    /**
     Resets the current position to the initial position.
     */
    Reset() {
        this.Index = -1;
    }
    /**
    Returns the element in the collection at the current position of the enumerator.
    @returns {any} Returns the element in the collection at the current position of the enumerator.
    */
    GetCurrent() {
        if (tp.InRange(this.List, this.Index))
            return this.List[this.Index];
        return null;
    }
};
tp.Enumerator.prototype.Index = -1;
tp.Enumerator.prototype.List = null;    // array-like
//#endregion

//#region tp.ListChangeType

/**
A static helper enum-like class. <br />
Indicates what kind of change is going to happen or happened in a list
@class
@static
*/
tp.ListChangeType = {
    /** Clear */
    Clear: 'Clear',
    Assign: 'Assign',
    AddRange: 'AddRange',
    Insert: 'Insert',
    Remove: 'Remove',
    Update: 'Update'
};
Object.freeze(tp.ListChangeType);
//#endregion

//#region  tp.ListEventArgs
/**
EventArgs derived class.
*/
tp.ListEventArgs = class extends tp.EventArgs {
    /**
    Constructor
    @param {string} Action - One of the tp.ListChangeType constants.
    */
    constructor(Action) {
        super(null, null, null);
        this.Action = Action;
    }
};
/** Indicates what kind of change is going to happen or happened in a list. One of the tp#ListChangeType constants. 
 @type {tp.ListChangeType}
 */
tp.ListEventArgs.prototype.Action = '';
/** The item index */
tp.ListEventArgs.prototype.Index = -1;
/** The item */
tp.ListEventArgs.prototype.Item = null;
/** The source list. */
tp.ListEventArgs.prototype.Source = null;
//#endregion

//#region  tp.List
/**
A list class, based on javascript array. 
*/
tp.List = class extends Array {
    /**
    Constructor.
    @param {any[]} [Source=null] - Optional. The source array as the initial content.
    */
    constructor(Source = null) {
        super(Source);
        this.length = 0;                    // for some reason, after the inherited constructor, the list has one element, so set it to zero.
        this.fUpdateCounter = 0;            // disabled
        this.fEventsEnabledCounter = 0;     // disabled
        this.Assign(Source);
    }

    /* Protected - event triggering related */
    /**
    Returns the invocation list, that is an array of Listener objects, for a specified event. The returned array could be empty.
    @param {string} EventName The name of the event for which the invocation list is requested.
    @returns {Listener[]} Returns an array with the listeners of the event, or an empty array.
    */
    GetInvocationList(EventName) {
        let Result = [];
        if (this.fEvents && !tp.IsBlank(EventName)) {
            EventName = EventName.toUpperCase();
            Result = this.fEvents[EventName] || [];
        }
        return Result;
    }
    /**
    Returns true if this instance has subscribed listeners for an event specified by its name.
    @param {string} EventName The name of the event for which the invocation list is requested.
    @returns {boolean}  Returns true if this instance has subscribed listeners for an event specified by its name.
    */
    HasListeners(EventName) { return this.GetInvocationList(EventName).length > 0; }

    /* Protected - event triggers */
    /**
     * Triggers an event
     * @param {tp.ListEventArgs} Args The event arguments
     */
    OnChanging(Args) {
        if (this.EventsEnabled === true && this.Updating === false) {
            this.Trigger('Changing', Args);
        }
    }
    /**
     * Triggers an event
     * @param {tp.ListEventArgs} Args The event arguments
     */
    OnChanged(Args) {
        if (this.EventsEnabled === true && this.Updating === false) {
            this.Trigger('Changed', Args);
        }
    }


    /* Protected - overridables */
    /**
    Clears the list
    */
    DoClear() {
        this.length = 0;
    }
    /**
    Inserts an item at an index. Returns the inserted item.
    @param {number} Index The index to use
    @param {any} Item The item to insert
    @returns {any} Returns the inserted item.
    */
    DoInsert(Index, Item) {
        if (Index >= this.length) {
            this.push(Item);
        } else {
            this.splice(Index, 0, Item);
        }

        return Item;
    }
    /**
    Removes an item found at a specified index.
    @param {number} Index The index to use
    */
    DoRemoveAt(Index) {
        this.splice(Index, 1);
    }
    /**
    Appends the items of a specified source array to this instance.
    @param {any[]} Items - The source array.
    */
    DoAddRange(Items) {
        if (Items) {
            for (var i = 0, ln = Items.length; i < ln; i++) {
                this.push(Items[i]);
            }
        }
    }


    /* Public - event emmiter */
    /**
    Adds a listener to an event of this instance and returns the listener object.  
    The caller specifies the event name, a callback function and a context for the callback. 
    When the event occurs then this instance invokes the callback passing a single parameter of type tp.EventArgs.
    @example
    MyObject.On('OnSomething', function(Args: tp.EventArgs) {
        //
    });

    // or
    MyObject.On('OnSomething', this.HandlerFunc, this);

    // or, if the caller is a tp.tpObject
    MyObject.On('OnSomething', this.FuncBind(this.HandlerFunc));

    @param {string} EventName - Can be any event name this instance supports.
    @param {function} Func - The event handler function to call back when the event occurs. Function signature: function (Args: tp.EventArgs): void
    @param {object} [Context=null] - Optional. The context (this) to use when calling the callback function.
    @returns {tp.Listener} Returns the created tp.Listener listener object
    */
    On(EventName, Func, Context = null) {

        if (!tp.IsBlank(EventName) && tp.IsFunction(Func)) {
            if (!this.fEvents)
                this.fEvents = {};

            EventName = EventName.toUpperCase();

            if (!(EventName in this.fEvents))
                this.fEvents[EventName] = []; // new invocation list for the event

            let L = new tp.Listener();
            L.Func = Func;
            L.Context = Context;

            this.fEvents[EventName].push(L);

            return L;
        }

        return null;
    }
    /**
    Removes a listener of the EventName event of this instance. That is does the opposite of On() method.
    @param {string} EventName - The name of the event used in a previous call to On().
    @param {tp.Listener|function} ListenerOrFunc - The listener object returned by a previous call to On() with the same event name or
    the callback function passed to a previous call to On() with the same event name.
    */
    Off(EventName, ListenerOrFunc) {
        EventName = arguments[0].toUpperCase();
        let InvocationList = this.GetInvocationList(EventName);
        if (InvocationList.length > 0) {

            // find it
            let Arguments = arguments;
            let L = null;
            if (arguments[1] instanceof tp.Listener) {
                L = tp.FirstOrDefault(InvocationList, (item) => item === Arguments[1]);
            } else if (tp.IsFunction(arguments[1])) {
                L = tp.FirstOrDefault(InvocationList, (item) => item.Func === Arguments[1]);
            }

            // remove it
            if (L) {
                InvocationList.splice(InvocationList.indexOf(L), 1);
            }
        }
    }
    /**
    Triggers EventName event passing Args. It calls all previously registered listeners. 
    NOTE: If EventsEnabled is false, nothing happens.
    @example
    this.Trigger('OnSomething', { });
    @param {string} EventName - The name one of the events this class supports.
    @param {tp.EventArgs|object} [Args=null] - Optional. The arguments to be passed to the listeners. Could be a plain javascript object or a tp.EventArgs instance.
    */
    Trigger(EventName, Args) {
        if (this.EventsEnabled === true && this.fEvents && !tp.IsBlank(EventName)) {
            let sEventName = EventName;
            EventName = EventName.toUpperCase();
            var InvocationList = this.GetInvocationList(EventName);
            if (InvocationList && InvocationList.length > 0) {

                Args = Args instanceof tp.EventArgs ? Args : new tp.EventArgs(Args);
                Args.EventName = sEventName;
                Args.Sender = this;
                var Listener;
                var i, ln;

                for (i = 0, ln = InvocationList.length; i < ln; i++) {
                    Listener = InvocationList[i];
                    Listener.Func.call(Listener.Context, Args);
                }
            }
        }
    }


    /* event handler */
    /**
    Implementation of the DOM EventListener interface. 
    For handling all DOM element events. Either when this is a DOM element and the sender (target) of the event is this.Handle
    or when the sender (target) of the event is any other object and listener is this instance.
    @see {@ling http://www.w3.org/TR/DOM-Level-2-Events/events.html#Events-EventListener|specification}
    @param {Event} e The event to handle
     */
    handleEvent(e) {
    }

    /* public */
    /**
    Clears the list
    */
    Clear() {
        //this.length = 0;
        let Args = new tp.ListEventArgs(tp.ListChangeType.Clear);
        this.OnChanging(Args);
        if (Args.Cancel !== true) {
            this.DoClear();
            this.OnChanged(Args);
        }
    }
    /**
    Removes all existing items and assigns the source array to this instance.
    @param {any[]} Source - The source array.
    */
    Assign(Source) {
        if (Source) {
            let Args = new tp.ListEventArgs(tp.ListChangeType.Assign);
            Args.Source = Source;

            this.OnChanging(Args);
            if (Args.Cancel !== true) {
                this.DoClear();
                this.DoAddRange(Source);
                this.OnChanged(Args);
            }
        }
    }
    /**
    Appends the items of a specified source array to this instance.
    @param {any[]} Items - The source array.
    */
    AddRange(Items) {
        if (Items) {
            let Args = new tp.ListEventArgs(tp.ListChangeType.AddRange);
            Args.Source = Items;

            this.OnChanging(Args);
            if (Args.Cancel !== true) {
                this.DoAddRange(Items);
                this.OnChanged(Args);
            }
        }
    }
    /**
    Adds an item. Returns the added item.
    @param {any} Item The item to add
    @returns {any}  Returns the added item.
    */
    Add(Item) {
        return this.Insert(this.length, Item);
    }
    /**
    Inserts an item at an index. Returns the inserted item.
    @param {number} Index The index to use
    @param {any} Item The item to insert
    @returns {any} Returns the inserted item.
    */
    Insert(Index, Item) {
        let Args = new tp.ListEventArgs(tp.ListChangeType.Insert);
        Args.Index = Index;
        Args.Item = Item;

        this.OnChanging(Args);
        if (Args.Cancel !== true) {
            this.DoInsert(Index, Item);
            this.OnChanged(Args);
        }

        return Item;
    }
    /**
    Removes an item from this instance
    @param {any} Item The item to remove
    */
    Remove(Item) {
        var Index = this.indexOf(Item);
        if (Index !== -1) {
            this.RemoveAt(Index);
        }
    }
    /**
    Removes an item found at a specified index.
    @param {number} Index The index to use
    */
    RemoveAt(Index) {
        let Args = new tp.ListEventArgs(tp.ListChangeType.Remove);
        Args.Index = Index;
        Args.Item = this[Index];

        this.OnChanging(Args);
        if (Args.Cancel !== true) {
            this.DoRemoveAt(Index);
            this.OnChanged(Args);
        }
    }
    /**
    Returns true if an items exists in this instance.
    @param {T} Item The item to check
    @returns {boolean} Returns true if an items exists in this instance.
    */
    Contains(Item) {
        return this.indexOf(Item) !== -1;
    }
    /**
    Returns the index of an item, if found, else -1.
    @param {T} Item The item to check
    @returns {number} Returns the index of an item, if found, else -1.
    */
    IndexOf(Item) {
        return this.indexOf(Item);
    }
    /**
    Returns a new array containing all the items of this instance
    @returns {any[]} Returns a new array containing all the items of this instance
    */
    ToArray() {
        return this.slice();
    }

    // 'by' methods 
    /**
    Finds and returns an item, if that item has a specified property with a specified value, if any, else null.
    @param {string} Prop - The property name
    @param {any} Value - The property value
    @returns {any} Returns the found item or null.
    */
    FindBy(Prop, Value) {
        var Index = this.IndexBy(Prop, Value);
        return Index !== -1 ? this[Index] : null;
    }
    /**
    Returns true if this instance contains an item and that item has a specified property with a specified value.
    @param {string} Prop - The property name
    @param {any} Value - The property value
    @returns {boolean} Returns true if this instance contains an item and that item has a specified property with a specified value.
    */
    ContainsBy(Prop, Value) {
        return this.IndexBy(Prop, Value) !== -1;
    }
    /**
    Returns the index of an item found having a specified property with a specified value, if any, else -1.
    @param {string} Prop - The property name
    @param {any} Value - The property value
    @returns {number} Returns the index of an item found having a specified property with a specified value, if any, else -1.
    */
    IndexBy(Prop, Value) {
        for (var i = 0, ln = this.length; i < ln; i++) {
            if (this[i][Prop] === Value)
                return i;
        }
        return -1;
    }
    /**
    Removes an item found having a specified property with a specified value.
    @param {string} Prop - The property name
    @param {any} Value - The property value
    @returns {boolean} Returns true if the item is found and removed
    */
    RemoveBy(Prop, Value) {
        var Index = this.IndexBy(Prop, Value);
        if (Index !== -1) {
            this.RemoveAt(Index);
            return true;
        }
        return false;
    }

    /**
    Returns an enumerator for this list.
    @returns {tp.Enumerator} Returns an enumerator for this list.
    */
    GetEnumerator() {
        return new tp.Enumerator(this);
    }


    /**
    Returns the number of items in the list
    @returns {number} Returns the number of items in the list
    */
    get Count() {
        return this.length;
    }
    /**
    Gets or sets a boolean value indicating whether events are enabled or not.
    When false, the default,, calling Trigger() does NOT inform any listener. 
    Defaults to false, that is no events are triggered by default;
    */
    get EventsEnabled() { return this.fEventsEnabledCounter > 0; }
    set EventsEnabled(v) {
        this.fEventsEnabledCounter = v === true ? this.fEventsEnabledCounter + 1 : this.fEventsEnabledCounter - 1;
    }
    /**
    Gets or sets a boolean value indicating whether batch updating is in progress.
    When true then the list defers event triggering until it becomes false again.
    Defaults to false.
    */
    get Updating() { return this.fUpdateCounter > 0; }
    set Updating(v) {
        this.fUpdateCounter = v === true ? this.fUpdateCounter + 1 : this.fUpdateCounter - 1;

        if (this.fUpdateCounter === 0) {
            let Args = new tp.ListEventArgs(tp.ListChangeType.Update);
            this.OnChanged(Args);
        }

        if (this.fUpdateCounter < 0) {
            this.fUpdateCounter = 0;
        }
    }

};
/* fields */
tp.List.prototype.fEventsEnabledCounter = 0;
tp.List.prototype.fEvents = null;               //  { [EventName: string]: tp.Listener[]; }
tp.List.prototype.fUpdateCounter = 0;

/**
 * Occurs just before the list is about to change
 * @event tp.List#Changing
 * @type {tp.ListEventArgs}
 */
/**
 * Occurs after the list has changed
 * @event tp.List#Changed
 * @type {tp.ListEventArgs}
 */

//#endregion

//#region  tp.CollectionItem


/** 
 *  A collection item class. Represents an object that is a collection item. It provides a single Collection field. 
 * */
tp.CollectionItem = class {
    constructor() {
        this.Collection = null;
    }
};
/** The collection the item belongs to. 
 @type {tp.Collection}
 */
tp.CollectionItem.prototype.Collection = null;

/**
Type guard function for the ICollectionItem interface
@param {object} v The object to check
@return {boolean} Returns true if the specified object provides a Collection property.
*/
tp.IsCollectionItem = function (v) { return 'Collection' in v; };
//#endregion

//#region tp.Collection

/**
A collection of items. Each item must implement the <code>tp.CollectionItem</code> interface.
@extends tp.List
*/
tp.Collection = class extends tp.List {

    /**
     * Constructor
     * @example
     * let Col = new tp.Collection(tp.CollectionItem);
     * @param {object} ItemClass - The class of the collection item, e.g <code>tp.CollectionItem</code>. <br />
     * It must provide a default constructor. <br />
     * Otherwise, derived classes may implement the <code>CreateItem()</code> method. <br />
     * @param {any[]} [Source=null] - Optional. The source array as the initial content.
     */
    constructor(ItemClass = null, Source = null) {
        super();
        this.ItemClass = ItemClass || null;
        if (Source)
            this.Assign(Source);
    }

    /* protected */
    /** 
    Creates and returns a new item.    
    @returns {tp.CollectionItem} Creates and returns a new item.
    @protected
    */
    CreateItem() {
        let Item = new this.ItemClass();
        return Item;
    }
    /**
    Called when an item is inserted. Source item may be null.
    @param {tp.CollectionItem} Item The inserted item.
    @param {object} [SourceItem=null] Optional. An item may be a copy of a source item.
    @protected
    */
    ItemInserted(Item, SourceItem = null) {
        if (tp.IsCollectionItem(Item))
            Item.Collection = this;
    }

    /* public */
    /**
    Adds an item. Returns the added item.
    @param {tp.CollectionItem} [Item=null] - Optional. If not passed then just a new item is created.
    @returns {object} Returns the added item.
    @override
    */
    Add(Item = null) {
        return this.Insert(this.length, Item);
    }
    /**
    Inserts an item at an index. Returns the inserted item.
    @param {number} Index The index to use
    @param {tp.CollectionItem} [Item=null]  - Optional. If not passed then just a new item is created.
    @returns {object} Returns the inserted item.
    @override
    */
    Insert(Index, Item = null) {
        if (!Item) {
            Item = this.CreateItem();
        } else if (!tp.IsCollectionItem(Item)) {
            throw "Can not insert an item. Item should be a tp.CollectionItem instance";
        }
        Item = super.Insert(Index, Item);
        this.ItemInserted(Item, null);
        return Item;
    }
    /**
    Appends the items of a specified source array to this instance. <br />
    NOTE: It actuall creates a clone for each item found in the specified source items.
    @param {tp.CollectionItem[]} Items - The source array.
    @override
    */
    AddRange(Items) {

        if (Items) {
            let SourceItem, Item;

            for (var i = 0, ln = Items.length; i < ln; i++) {
                SourceItem = Items[i];
                if (!tp.IsCollectionItem(SourceItem)) {
                    throw "Can not insert an item. Item should be a tp.CollectionItem instance";
                }
                Item = this.CopyItem(SourceItem);
                this.push(Item);
                this.ItemInserted(Item, SourceItem);
            }
        }
    }
    /**
    Creates and returns a new item which is a copy of a specified source item.
    @param {object} SourceItem The item to use as source.
    @returns {object} Returns a new item which is a copy of a specified source item.
    */
    CopyItem(SourceItem) {
        var Item = null;

        if (tp.IsCloneable(SourceItem)) {
            Item = SourceItem.Clone();

            if (!tp.IsCollectionItem(Item)) {
                throw "Can not copy an item. Item should be a tp.CollectionItem instance";
            }
        } else {
            Item = this.CreateItem();

            if (tp.IsAssignable(Item)) {
                Item.Assign(SourceItem);
            } else {
                tp.MergePropsDeep(Item, SourceItem);
            }
        }

        return Item;
    }

};
/** The class of the child items. <code>tp.CollectionItem</code> class or a derived class.
 @type tp.CollectionItem
 */
tp.Collection.prototype.ItemClass = null;
//#endregion

//#region tp.NamedItem
/**
A collection item with a Name property
*/
tp.NamedItem = class extends tp.CollectionItem {
    /**
    Constructor
    @param {string} [Name=null] The name of the item.
    */
    constructor(Name = null) {
        super();
        this.fName = typeof Name === 'string' ? Name : '';
    }

    /* protected */
    get_Name() { return this.fName; }
    set_Name(v) { this.fName = v; }

    /* public */
    /**
    Gets or sets the name of this instance
    */
    get Name() { return this.get_Name(); }
    set Name(v) { this.set_Name(v); }

    /**
    Assigns a source item to this instance
    @param {tp.NamedItem} Source The source to assign to this instance
    */
    Assign(Source) {
        this.Name = Source.Name;
    }
};
tp.NamedItem.prototype.fName = '';
//#endregion

//#region  tp.NamedItems
/**
    A collection of named items
*/
tp.NamedItems = class extends tp.Collection {

    /**
     * Constructor
     * @param {object} [ItemClass=tp.NamedItem]  - The class of the collection item, e.g tp.NamedItem.
     */
    constructor(ItemClass = tp.NamedItem) {
        super(ItemClass, null);
    }

    /* protected */
    /** 
    Creates and returns a new item
    @param {string} [Name=null] - Optional. The name of the new item
    @returns {tp.NamedItem} Returns the new item.
    */
    CreateItem(Name = null) {
        let Item = super.CreateItem();
        if (Name) {
            Item.Name = Name;
        }
        return Item;
    }

    /* public */
    /**
    Adds an item
    @param {string|tp.NamedItem} NameOrItem - If it is a string a new item is created
    @returns {tp.NamedItem} Returns the item.
    */
    Add(NameOrItem) {
        let Item = typeof NameOrItem === 'string' ? this.CreateItem(NameOrItem) : NameOrItem;
        return super.Add(Item);
    }
    /**
    Inserts an item at an index
    @param {number} Index The index to use
    @param {string|tp.NamedItem} NameOrItem - If it is a string a new item is created
    @returns {tp.NamedItem} Returns the item.
    */
    Insert(Index, NameOrItem) {
        let Item = typeof NameOrItem === 'string' ? this.CreateItem(NameOrItem) : NameOrItem;
        return super.Insert(Index, Item);
    }
    /**
    Removes an item.
    @param {string|tp.NamedItem} NameOrItem - If it is a string then a search is done by name, in order to find the item
    */
    Remove(NameOrItem) {
        var Index = this.IndexOf(NameOrItem);
        if (Index !== -1) {
            this.RemoveAt(Index);
        }
    }
    /**
    Returns true if an item exists in this collection
    @param {string|tp.NamedItem} NameOrItem - If it is a string then a search is done by name, in order to find the item
    @returns {boolean} Returns true if this collection contains the item.
    */
    Contains(NameOrItem) {
        return this.IndexOf(NameOrItem) !== -1;
    }
    /**
    Returns the index of an item.
    @param {string|tp.NamedItem} NameOrItem - If it is a string then a search is done by name, in order to find the item
    @returns {number} Returns the index of the item in this collection.
    */
    IndexOf(NameOrItem) {
        return typeof NameOrItem === 'string' ? this.IndexBy('Name', NameOrItem) : super.IndexOf(NameOrItem);
    }
    /**
    Finds and returns an item by name, if any, else null
    @param {string} Name - The name of the item.
    @returns {tp.NamedItem} Returns the item, if any, or null.
    */
    Find(Name) {
        return this.FindBy('Name', Name);
    }
};
//#endregion

//#region tp.Descriptor
/**
    Represents an object that describes an object.
*/
tp.Descriptor = class extends tp.NamedItem {
    /**
    Constructor
    @param {string} [Name=null] The name of this item
    */
    constructor(Name = null) {
        super(Name);
    }

    /* protected */
    get_Alias() {
        return tp.IsBlank(this.fAlias) ? this.Name : this.fAlias;
    }

    /**
    Gets or sets the Alias
    */
    get Alias() { return this.get_Alias(); }
    set Alias(v) { this.fAlias = v; }
    /**
    Gets or sets the Title
    */
    get Title() { return this.fTitle; }
    set Title(v) { this.fTitle = v; }
    /**
    Gets or sets the TitleKey
    */
    get TitleKey() { return this.fTitleKey; }
    set TitleKey(v) { this.fTitleKey = v; }

    /**
   Assigns a source item to this instance
   @param {tp.Descriptor} Source The source item
   */
    Assign(Source) {
        super.Assign(Source);
        this.Title = Source.Title;
        this.TitleKey = Source.TitleKey;
        this.Alias = Source.Alias;
    }

};

tp.Descriptor.prototype.fAlias = '';
tp.Descriptor.prototype.fTitle = '';
tp.Descriptor.prototype.fTitleKey = '';

/**
Finds and returns a descriptor in a list, by its Alias
@param {string} Alias The alias to search for.
@param {any[]} List The list to search in.
@returns {tp.Descriptor} Returns the found object, if any, else null.
*/
tp.Descriptor.FindByAlias = function (Alias, List) {
    for (var i = 0, ln = List.length; i < ln; i++) {
        if (tp.IsSameText(Alias, List[i].Alias))
            return List[i];
    }
    return null;
};

//#endregion

//#region tp.Dictionary
/**
A generic dictionary
*/
tp.Dictionary = class {
    /**
    Constructor
    */
    constructor() {
        this.fItems = [];
    }

    /* protected */
    FindEntry(Key) {
        for (let i = 0, ln = this.fItems.length; i < ln; i++) {
            if (this.fItems[i].Key === Key)
                return this.fItems[i];
        }
        return null;
    }

    /* public */
    /**
    Returns the number of entries
    */
    get Count() {
        return this.fItems.length;
    }

    /**
    Sets a key and its value. A new entry is created if no entry exists under the specified key.
    @param {any} Key The key
    @param {any} Value The value
    */
    Set(Key, Value) {
        let Entry = this.FindEntry(Key);
        if (tp.IsEmpty(Entry)) {
            Entry = { Key: Key, Value: Value };
            this.fItems.push(Entry);
        } else {
            Entry.Value = Value;
        }
    }
    /**
    Returns the value of a specified key. If no entry exists under the key, null is returned.
    @param {any} Key The key
    @returns {any} Returns the value of a specified key. If no entry exists under the key, null is returned.
    */
    Get(Key) {
        let Entry = this.FindEntry(Key);
        return !tp.IsEmpty(Entry) ? Entry.Value : null;
    }

    /**
    Returns true if this dictionary contains an entry under a specified key.
    @param {any} Key The key
    @returns {boolean} Returns true if this dictionary contains an entry under a specified key.
    */
    ContainsKey(Key) {
        return !tp.IsEmpty(this.FindEntry(Key));
    }
    /**
    Removes an entry by key
    @param {any} Key The key
    */
    Remove(Key) {
        let Entry = this.FindEntry(Key);
        if (!tp.IsEmpty(Entry))
            tp.ListRemove(this.fItems, Entry);
    }
    /**
    Removes all entries from the dictionary
    */
    Clear() {
        this.fItems.length = 0;
    }

    /**
    Returns an array with the keys
    @returns {any[]} Returns an array with the keys
    */
    Keys() {
        return tp.Transform(this.fItems, (entry) => { return entry.Key; });
    }
    /**
    Returns an array with the values
    @returns {any[]} Returns an array with the values
    */
    Value() {
        return tp.Transform(this.fItems, (entry) => { return entry.Value; });
    }

    /**
    Returns the value of a key, if the key exists, else returns a specified default value.
    @param {any} Key The key
    @param {any} Default A default value.
    @returns {any} Returns the value of a key, if the key exists, else returns a specified default value.
    */
    ValueOf(Key, Default) {
        let Entry = this.FindEntry(Key);
        return !tp.IsEmpty(Entry) ? Entry.Value || Default : Default || null;
    }

    /**
    Returns an enumerator for this dictionary.
    @returns {tp.Enumerator}  Returns an enumerator for this dictionary.
    */
    GetEnumerator() {
        return new tp.Enumerator(this.fItems);
    }
};
tp.Dictionary.prototype.fItems = []; // { Key: any, Value: any }[]
//#endregion

//#region  tp.NameValueStringList
/**
    A string list class where each line has the format Name=Value
*/
tp.NameValueStringList = class {

    /**
    Constructor
    @param {any} Source? - An initial value. Could be a NameValueStringList, a Dictionary, a DataTable, or just a plain string.
    */
    constructor(Source = null) {
        this.fItems = [];

        if (tp.IsValid(Source)) {
            this.Assign(Source);
        }
    }

    /* properties */
    /**
    Gets or sets the content of this instance
    */
    get Text() { return this.fItems.join('\n'); }
    set Text(v) {
        this.Clear();
        if (!tp.IsBlank(v)) {
            var Sep = tp.ContainsText(v, '\r\n') ? '\r\n' : '\n';
            var Lines = tp.Split(v, Sep, true);
            for (var i = 0, ln = Lines.length; i < ln; i++) {
                if (!tp.IsBlank(Lines[i])) {
                    this.AddLine(Lines[i]);
                }
            }
        }
    }
    /**
    Returns the number of lines
    */
    get Count() { return this.fItems.length; }
    /** 
    Returns the lines of this instance, joined by comma, e.g. Line1, Line2, LineN
    */
    get CommaText() { return this.fItems.join(','); }
    /**
    Returns a string array with the names. The name is the first part of a line, e.g. Name=Value
    @returns {string[]} A string array with the names.
    */
    get Names() {
        var Result = [];

        var o;
        for (var i = 0, ln = this.fItems.length; i < ln; i++) {
            o = this.Split(this.fItems[i]);
            Result.push(o.Name);
        }

        return Result;
    }


    /* public */
    /**
    Clears the content of this instance
    */
    Clear() {
        this.fItems.length = 0;
    }
    /**
    Assigns the content of a specified source object to this instance.
    @param {any} Source - Could be a NameValueStringList, a Dictionary, or tp.DataTable, or just a plain string.
    */
    Assign(Source) {
        this.Clear();

        var i, ln;

        if (tp.IsArray(Source)) {
            this.fItems = tp.ListClone(Source);
        } else if (Source instanceof tp.NameValueStringList) {
            this.fItems = tp.ListClone(Source.fItems);
        } else if (Source instanceof tp.Dictionary) {
            var Keys = Source.Keys();
            var Values = Source.Values();
            for (i = 0, ln = Keys.length; i < ln; i++) {
                this.Add(Keys[i], Values[i]);
            }
        }

        else if (!tp.IsEmpty(tp.DataTable) && Source instanceof tp.DataTable) {
            for (i = 0, ln = Source.RowCount; i < ln; i++) {
                this.Add(Source.Rows[i].Get(0), Source.Rows[i].Get(1));
            }
        }
        else if (tp.IsString(Source)) {
            this.Text = Source;
        }

    }
    /**
    Returns a clone of this instance
    @returns {tp.NameValueStringList} Returns a clone of this instance
    */
    Clone() {
        var Result = new NameValueStringList();
        Result.fItems = tp.ListClone(this.fItems);
        return Result;
    }

    /**
    Adds a string line. It must have the format Name=Value
    @param {string} Line The line to add. It must have the format Name=Value
    */
    AddLine(Line) {
        this.InsertLine(this.fItems.length, Line);
    }
    /**
    Inserts a string line at a specified index It must have the format Name=Value
    @param {number} Index The index to use.
    @param {string} Line The line to add. It must have the format Name=Value
    */
    InsertLine(Index, Line) {
        if (tp.IsBlank(Line))
            throw "Item can not be null, empty, or white space";

        var o = this.Split(Line);

        this.Insert(Index, o.Name, o.Value);
    }
    /**
    Removes a line, if found.
    @param {string} Line The line to remove
    */
    RemoveLine(Line) {
        this.RemoveAt(this.IndexOfLine(Line));
    }
    /**
    Returns the index of a line, if exists, else -1. Case insensitive match.
    @param {string} Line The line to search for.
    @returns {number} Returns the index of the line, if exists, else -1.
    */
    IndexOfLine(Line) {
        for (var i = 0, ln = this.fItems.length; i < ln; i++) {
            if (tp.IsSameText(this.fItems[i], Line))
                return i;
        }
        return -1;
    }
    /**
    Returns true if a line exists. Case insensitive match.
    @param {string} Line  The line to search for.
    @returns {boolean} Returns true if this instance contains the specified line.
    */
    ContainsLine(Line) {
        return this.IndexOfLine(Line) !== -1;
    }

    /**
    Adds a line by constracting a new one, using a specified name and value
    @param {string} Name The name
    @param {string} Value The value
    */
    Add(Name, Value) {
        this.Insert(this.fItems.length, Name, Value);
    }
    /**
    Inserts a line at a specified index, by constracting a new one, using a specified name and value
    @param {number} Index The index to use
    @param {string} Name The name
    @param {string} Value The value
    */
    Insert(Index, Name, Value) {
        if (tp.IsBlank(Name))
            throw "Name can not be null, empty, or white space";

        if (!tp.IsBlank(Value) && this.Contains(Name))
            throw tp.Format("Name already exists in list: {0}", Name);

        tp.ListInsert(this.fItems, Index, this.Concat(Name, Value));
    }
    /**
    Returns the index of a name, if any, else -1. Case insensitive match.
    @param {string} Name  index to use
    @returns {number} Returns the index of a name, if any, else -1. Case insensitive match.
    */
    IndexOf(Name) {
        var o;
        for (var i = 0, ln = this.fItems.length; i < ln; i++) {
            o = this.Split(this.fItems[i]);
            if (tp.IsSameText(Name, o.Name))
                return i;
        }

        return -1;
    }
    /**
    Returns true if a specified name exists. Case insensitive match.
    @param {string} Name The name
    @returns {boolean} Returns true if this instance contains an entry with the specified Name.
    */
    Contains(Name) {
        return this.IndexOf(Name) !== -1;
    }
    /**
    Removes a line by a specified name, if exists.  Case insensitive match.
    @param {string} Name The name
    */
    Remove(Name) {
        this.RemoveAt(this.IndexOf(Name));
    }
    /** 
    Removes a line at a specified index.
    @param {number} Index The index to use
    */
    RemoveAt(Index) {
        if (Index >= 0 && Index <= this.fItems.length - 1)
            tp.ListRemoveAt(this.fItems, Index);
    }

    /**
    Returns the lines of this instance as a string array.
    @returns {string[]}  Returns the lines of this instance as a string array.
    */
    ToArray() {
        return tp.ListClone(this.fItems);
    }
    /**
    Returns the lines of this instance as a Dictionary.
    @returns {tp.Dictionary} Returns the lines of this instance as a Dictionary.
    */
    ToDictionary() {
        var Result = new tp.Dictionary();
        var o;
        for (var i = 0, ln = this.fItems.length; i < ln; i++) {
            o = this.Split(this.fItems[i]);
            Result.Set(o.Name, o.Value);
        }
        return Result;
    }



    /**
    Returns the name of a line found at a specified index
    @param {number} Index The index
    @returns {string}  Returns the name of a line found at a specified index
    */
    NameAt(Index) {
        var o = this.Split(this.fItems[Index]);
        return o.Name;
    }
    /**
    Returns the value of a line found at a specified index
    @param {number} Index The index
    @returns {string} Returns the value of a line found at a specified index
    */
    ValueAt(Index) {
        var o = this.Split(this.fItems[Index]);
        return o.Value;
    }
    /**
    Returns the value of a line having a specified name (Key)
    @param {string} Name The line name (Key)
    @returns {string} Returns the value of a line having a specified name
    */
    GetValue(Name) {
        var Index = this.IndexOf(Name);
        return tp.InRange(this.fItems, Index) ? this.ValueAt(Index) : '';
    }
    /**
    Sets the value of a line having a specified name
    @param {string} Name The line name (Key)
    @param {string} v The value to set
    */
    SetValue(Name, v) {
        var Index = this.IndexOf(Name);

        if (Index === -1) {
            this.Add(Name, v.toString());
        } else {
            this.fItems[Index] = this.Concat(Name, v);
        }
    }
    /**
    Splits a line into an object having Name and Value fields
    @param {string} Line The whole line, e.g. Key=Value
    @returns {object} Returns an object as <code>{Name: string, Value: string}</cod>
    */
    Split(Line) {
        var Result = {
            Name: '',
            Value: ''
        };

        if (!tp.IsBlank(Line)) {
            var Parts = tp.Split(Line, '=');
            if ((Parts !== null) && (Parts.length > 0)) {
                Result.Name = tp.Trim(Parts[0]);
                if (Parts.length > 1)
                    Result.Value = tp.Trim(Parts[1]);
            }
        }

        return Result;
    }
    /**
    Concatenates a name and a value and returns the line
    @param {string} Name The line name (Key)
    @param {string} Value The value
    @returns {string} Returns a string as Name=Value
    */
    Concat(Name, Value) {
        if (!tp.IsBlank(Value)) {
            if (!tp.IsBlank(Name))
                return tp.Format("{0}={1}", Name, Value);
            else
                return Value;
        }

        return !tp.IsBlank(Name) ? Name : '';
    }
    /**
    Returns a {@link tp.Enumerator} enumerator to be used in iterating the lines of this instance.
    @returns {tp.Enumerator}  Returns a {@link tp.Enumerator} enumerator to be used in iterating the lines of this instance.
    */
    GetEnumerator() {
        return new tp.Enumerator(this.fItems);
    }
    /**
    Localizes the values of this instance in the current language
    */
    LocalizeValues() {
        if (!tp.IsValid(tp.Res))
            return;

        var self = this;
        var E = this.GetEnumerator();
        var o;

        function GS() {
            if (E.MoveNext()) {
                o = self.Split(E.Current);
                tp.Res.GS(o.Name, OnResult, o.Value, null, null);
            }
        }

        function OnResult(Value, UserTag) {
            self.SetValue(o.Name, Value);
            GS();
        }

        GS();


    }
};
tp.NameValueStringList.prototype.fItems = []; // string[]
//#endregion




//---------------------------------------------------------------------------------------
// Ajax and Async (promise)
//---------------------------------------------------------------------------------------


//#region tp.AjaxArgs

/**
Arguments class for the {@link tp.Ajax} class methods.
*/
tp.AjaxArgs = class {

    /**
     * Constructor.
     * Creates an arguments object for use with the {@link tp.Ajax} class methods
     * @param {object|tp.AjaxArgs} SourceArgs - Optional. A source arguments object to copy property values from. Could be a {@link tp.AjaxArgs} instance.
     */
    constructor(SourceArgs = null) {

        // default initialization
        this.Method = "POST";
        this.Url = '';
        //this.Async = true;
        this.Data = null;                       // the data to send
        this.UriEncodeData = true;
        this.Timeout = 0;
        this.ContentType = 'application/x-www-form-urlencoded; charset=UTF-8';
        this.Context = null;                                                    // context for calling the two callbacks
        this.AntiForgeryToken = '';                                             // used when POST-ing an html form in Microsoft MVC framework
        this.OnSuccess = null;                                                  // function(Args: tp.AjaxArgs)
        this.OnFailure = null;                                                  // function(Args: tp.AjaxArgs)
        this.OnRequestHeaders = tp.AjaxOnRequestHeadersDefaultHandler;          // function(Args: tp.AjaxArgs)
        this.ResponseHandlerFunc = tp.AjaxResponseDefaultHandler;

        this.XHR = null;                        // XMLHttpRequest
        this.ErrorText = '';                    // the XMLHttpRequest.statusText in case of an error
        this.Result = false;                    // true if ajax call succeeded

        this.ResponseData = {                   // server response  
            Result: false,
            ErrorText: '',
            Packet: {}
        };

        this.Tag = null;                        // a user defined value

        // apply the specified parameteter
        SourceArgs = SourceArgs || {};

        for (var Prop in SourceArgs) {
            this[Prop] = SourceArgs[Prop];
        }
    }

    /* properties */
    /** Returns true if a POST method is specified. 
     @type {boolean}
     */
    get IsPost() {
        return tp.IsSameText('post', this.Method);
    }
    /** Returns true if a GET method is specified.
    @type {boolean} */
    get IsGet() {
        return !this.IsPost;
    }

    /** The XMLHttpRequest.responseText string in any case (could be null though in case of an error). <br />
     * <strong>Valid only after response from server</strong>
     @type {string}
     */
    get ResponseText() {
        return this.XHR ? this.XHR.responseText : '';
    }


    /** Returns a string representation of this instance 
     @returns {string} Returns a string representation of this instance
     */
    toString() {
        let S = `
Method: "${this.Method}"
Url: "${this.Url}"
AjaxResult: "${this.Result}"
ErrorText: "${this.ErrorText}"
ResponseText: "${this.ResponseText}"
ResponseResult: "${this.ResponseData.IsSuccess}"
ResponseErrorText: "${this.ResponseData.ErrorText}" `;

        return S;
    }
};
/** The Http method to use. Defaults to POST. 
@default POST
 @type {string} */
tp.AjaxArgs.prototype.Method = "POST";
/** The url.  
 @type {string} */
tp.AjaxArgs.prototype.Url = '';
//tp.AjaxArgs.prototype.Async = true;
/** Represents the data to send. Defaults to null. <br />
 * In POSTs it is a plain object with one or more key/value pairs. <br />
 * In GETs can be null or empty, or a string with query parameters, e.g. <code>param1=value1&paramN=valueN</code>
 * @default null
 * @type {object} */
tp.AjaxArgs.prototype.Data = null;
/** When true, then Data is Uri-encoded. Defaults to true.
 * @default true
 * @see {@link http://stackoverflow.com/questions/18381770/does-ajax-post-data-need-to-be-uri-encoded|stackoverflow}
*/
tp.AjaxArgs.prototype.UriEncodeData = true;
/** The timeout in milliseconds. <br />
 * Defaults to zero, meaning no timeout. <br />
 * When set to a non-zero value will cause fetching to terminate after the given time has passed. 
 @default 0
 @type {number}
 */
tp.AjaxArgs.prototype.Timeout = 0;
/** The content type. Defaults to <code>application/x-www-form-urlencoded; charset=UTF-8</code>
 @default application/x-www-form-urlencoded; charset=UTF-8
 @type {string}
 */
tp.AjaxArgs.prototype.ContentType = 'application/x-www-form-urlencoded; charset=UTF-8';
/** context for calling the two callbacks  
 @default null
 @type {object}
 */
tp.AjaxArgs.prototype.Context = null;
/** A string used when POST-ing an html form in Microsoft MVC framework. Defaults to empty string.  
 @default ''
 */
tp.AjaxArgs.prototype.AntiForgeryToken = '';
/** A <code>function(Args: tp.AjaxArgs)</code> callback function to call on success  
 @default null
 @type {function}
 */
tp.AjaxArgs.prototype.OnSuccess = null;
/** A <code>function(Args: tp.AjaxArgs)</code> callback function to call on failure
 @default null
 @type {function}
 */
tp.AjaxArgs.prototype.OnFailure = null;
/** A function(Args: tp.AjaxArgs) callback function to call in order to give the caller code a chance to add additional request headers.
 @default null
 @type {function}
 */
tp.AjaxArgs.prototype.OnRequestHeaders = null;
/**
 * A function(Args: tp.AjaxArgs) callback function. It is called just before the OnSuccess() call-back. <br />
 * Processes the response after an ajax call returns. <br />
 * The default response handler deserializes the Args.ResponseText into an object and assigns the Args.ResponseData object.
 * It assumes that the ResponseText is a json text containing an object with at least two properties: <code> { Result: boolean, ErrorText: string } </code>. <br />
 * Further on, if the Args.ResponseData  contains a Packet property and that Packet property is a json text, deserializes it into an object.
 @default null
 @type {function}
 */
tp.AjaxArgs.prototype.ResponseHandlerFunc = null;

/** The {@link https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest|XMLHttpRequest} object. <br />
 * <strong>Valid only after response from server</strong>
 @type {XMLHttpRequest}
 */
tp.AjaxArgs.prototype.XHR = null;
/** The XMLHttpRequest.statusText string in case of an error. <br />
 * <strong>Valid only after response from server</strong>
 @type {string}
 */
tp.AjaxArgs.prototype.ErrorText = '';
/** True when ajax call succeeds 
 @type {boolean}
 */
tp.AjaxArgs.prototype.Result = false;
/** The response from the server it is always packaged as a C# Tripous.HttpActionResult instance. <br />
 * That is it comes as an object <code>{ IsSuccess: false, ErrorText: '', Packet: {} }</code>  where Packet is the actual response json data. <br />
 * <strong>Valid only after response from server</strong>
 @type {object}
 */
tp.AjaxArgs.prototype.ResponseData = { Result: false, ErrorText: '', Packet: {} };
/** A user defined value.  
 @default null
 @type {any}
 */
tp.AjaxArgs.prototype.Tag = null;

//#endregion

/**
 * The default handler after an ajax call returns. <br />
 * Deserializes the Args.ResponseText into an object and assigns the Args.ResponseData object.
 * It assumes that the ResponseText is a json text containing an object with at least two properties: <code> { Result: boolean, ErrorText: string } </code>. <br />
 * Further on, if the Args.ResponseData  contains a Packet property and that Packet property is a json text, deserializes it into an object.
 * @param {tp.AjaxArgs} Args  A {@link tp.AjaxArgs} instance
 */
tp.AjaxResponseDefaultHandler = function (Args) {
    function ErrorText(Text) {
        return tp.IsString(Text) && !tp.IsBlank(Text) ? Text : "Unknown error";
    }

    if (Args.Result === false)
        throw ErrorText(Args.ErrorText);

    let o = null;
    try {
        o = JSON.parse(Args.ResponseText);
        Args.ResponseData = o;
    } catch (e) {
        return;
    }

    if (!tp.IsEmpty(o) && o.Result === false)
        throw ErrorText(o.ErrorText);

    if (tp.IsJson(Args.ResponseData.Packet)) {
        try {
            Args.ResponseData.Packet = JSON.parse(Args.ResponseData.Packet);
        } catch (e) {
            //
        }
    }
};

/**
 * The default handler for the OnRequestHeaders event. <br />
 * Does nothing
 * @param {any} Args
 */
tp.AjaxOnRequestHeadersDefaultHandler = function (Args) {
};

//#region Ajax

/**
Ajax static function.
Executes ajax requests.
@param {tp.AjaxArgs} Args The passed arguments object. Should be a {@link tp.AjaxArgs} instance.
*/
tp.Ajax = function (Args) {

    /**
    Returns true if the ajax request is successful, by examining the status property.
    @see {@link https://developer.mozilla.org/en-US/docs/Web/HTTP/Status|Http status codes}
    @see {@link http://help.dottoro.com/ljspgwvf.php|dottoro.com}
    @param {XMLHttpRequest} XHR The XMLHttpRequest object
    @returns {boolean} Returns true if the request is successful.
    */
    let Succeeded = function (XHR) { return !tp.IsEmpty(XHR) && (XHR.status === 0 || XHR.status >= 200 && XHR.status < 300 || XHR.status === 304 || XHR.status === 1223); };

    let Context = Args.Context;
    let Data = Args.Data;
    let Url = encodeURI(Args.Url.toLowerCase());
    let Async = true;

    // see: http://stackoverflow.com/questions/18381770/does-ajax-post-data-need-to-be-uri-encoded
    if (!tp.IsEmpty(Data) && !tp.IsString(Data) && Args.UriEncodeData === true) {
        Data = tp.EncodeArgs(Data);
    }

    if (Args.IsGet && !tp.IsEmpty(Data)) {
        Url += '?' + Data;
    }


    // create
    let XHR = new XMLHttpRequest();
    Args.XHR = XHR;

    // error handler
    let OnError = function (e) {
        Args.ResponseText = XHR.responseText;

        var List = ['Ajax call failed. Url: ' + Url];

        if (tp.IsEmpty(e)) {
            List.push('Status Text: ' + XHR.statusText);
        } else if (e instanceof ProgressEvent) {
            List.push('Ajax call failed because of a failure on the network level');
        } else {
            List.push('Error Text: ' + tp.ExceptionText(e));
        }
        Args.ErrorText = List.join('\n');

        if (tp.IsFunction(Args.OnFailure))
            tp.Call(Args.OnFailure, Context, Args);
        else
            tp.Throw(Args.ErrorText);

    };


    // set XMLHttpRequest events
    XHR.onload = function (e) {
        if (XHR.readyState === XMLHttpRequest.DONE) {
            Args.ResponseText = XHR.responseText;
            if (Succeeded(XHR)) {
                Args.Result = true;
                tp.Call(Args.ResponseHandlerFunc, null, Args);
                tp.Call(Args.OnSuccess, Context, Args);
            } else {
                OnError(e);
            }
        }
    };
    XHR.onerror = function (e) {
        OnError(e);
    };


    try {
        XHR.open(Args.Method, Url, Async);

        if (Async) {
            XHR.timeout = Args.Timeout;
        }

        // headers
        XHR.setRequestHeader('Content-Type', Args.ContentType);
        XHR.setRequestHeader("Accept", "*/*");
        //XHR.setRequestHeader('X-Requested-With', 'XMLHttpRequest'); // invalid in cross-domain call
        if (!tp.IsBlank(Args.AntiForgeryToken)) {
            XHR.setRequestHeader("__RequestVerificationToken", Args.AntiForgeryToken);
        }

        tp.Call(Args.OnRequestHeaders, Context, Args);

        Data = Args.IsPost ? Data : null;

        // send         
        XHR.send(Data);
    } catch (e) {
        OnError(e);
    }
};





/**
Executes an ajax request inside a promise.
@param {tp.AjaxArgs} Args The passed arguments object.
@returns {tp.AjaxArgs} Returns a {@link tp.AjaxArgs} {@link Promise}.
*/
tp.Ajax.Async = async function (Args) {

    let Context = Args.Context || null;
    let OnSuccess = Args.OnSuccess || null;
    let OnFailure = Args.OnFailure || null;

    // ------------------------------------------
    let ExecutorFunc = function (Resolve, Reject) {
        Args.Context = null;
        Args.OnSuccess = function SuccessFunc(Args) {

            if (tp.IsFunction(OnSuccess)) {

                // if there is an onsuccess callback, then call it and check the results
                tp.Call(OnSuccess, Context, Args);

                if (!tp.IsEmpty(Args.ResponseData) && Args.ResponseData.IsSuccess === false && !tp.IsBlank(Args.ResponseData.ErrorText)) {
                    Reject(tp.ExceptionText(Args));
                } else {
                    Resolve(Args);
                }
            } else {
                Resolve(Args);
            }
        };
        Args.OnFailure = function FailureFunc(Args) {
            if (tp.IsFunction(OnFailure))
                tp.Call(OnFailure, Context, Args);
            Reject(tp.ExceptionText(Args));
        };

        tp.Ajax(Args);
    };
    // ------------------------------------------
    let Result = new Promise(ExecutorFunc);
    return Result;
};


/* tp.Ajax standard calls */

/**
Executes a GET ajax request
@param {string} Url - The url to call
@param {object} [Data=null] - Optional. The data to sent. Could be null
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
*/
tp.Ajax.Get = function (Url, Data = null, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = this.GetArgs(Url, Data, OnSuccess, OnFailure, Context);
    tp.Ajax(Args);
};
/**
Executes a POST ajax request
@param {string} Url - The url to call
@param {object} [Data=null] - Optional. The data to sent. Could be null
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
*/
tp.Ajax.Post = function (Url, Data = null, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = tp.Ajax.PostArgs(Url, Data, OnSuccess, OnFailure, Context);
    tp.Ajax(Args);
};
/**
Executes a POST ajax request and sends a model to the server. The function serializes the model by calling JSON.stringify(). 
It also adjusts the Content-Type header as application/json; charset=utf-8
@param {string} Url - The url to call
@param {object} Model - The model, a plain object, to sent. The function serializes the model by calling JSON.stringify().
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
*/
tp.Ajax.PostModel = function (Url, Model, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = tp.Ajax.ModelArgs(Url, Model, OnSuccess, OnFailure, Context);
    tp.Ajax(Args);
};


/* tp.Ajax promise calls */

/**
Executes a GET ajax request inside a promise
@param {string} Url - The url to call
@param {object} [Data=null] - Optional. The data to sent. Could be null
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
@returns {tp.AjaxArgs} Returns a {@link tp.AjaxArgs} {@link Promise}.
*/
tp.Ajax.GetAsync = async function (Url, Data = null, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = tp.Ajax.GetArgs(Url, Data, OnSuccess, OnFailure, Context);
    return tp.Ajax.Async(Args);
};
/**
Executes a POST ajax request inside a promise
@param {string} Url - The url to call
@param {object} [Data=null] - Optional. The data to sent. Could be null
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
@returns {tp.AjaxArgs} Returns a {@link tp.AjaxArgs} {@link Promise}.
*/
tp.Ajax.PostAsync = async function (Url, Data = null, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = tp.Ajax.PostArgs(Url, Data, OnSuccess, OnFailure, Context);
    return tp.Ajax.Async(Args);
};
/**
Executes a POST ajax request inside a promise and sends a model to the server. 
The function serializes the model by calling JSON.stringify(). 
It also adjusts the Content-Type header as application/json; charset=utf-8
@param {string} Url - The url to call
@param {object} Model - The model, a plain object, to sent. The function serializes the model by calling JSON.stringify().
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
@returns {tp.AjaxArgs} Returns a {@link tp.AjaxArgs} {@link Promise}.
*/
tp.Ajax.PostModelAsync = async function (Url, Model, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = tp.Ajax.ModelArgs(Url, Model, OnSuccess, OnFailure, Context);
    return tp.Ajax.Async(Args);
};

/**
Executes a POST ajax request inside a promise, sending a container element as a model.
If the container element is a form and has a valid action, that action is used a Url.

This method serializes a form, or any other container, into a javascript object, by adding a property for each input, select, textarea or button child element, to that object. 
Each added property is named after child element's name or id (in this order) 

That is for a child element such as 
    &lt;input type='text' id='UserName' value='John' /&gt; 
 a property/value is added as 
    { UserName: 'John' } 

NOTE: input elements of type file or image, are INCLUDED.  
NOTE: A select element of type select-multiple generates an array property. 
@param {Element|string} ElementOrSelector - A selector or n html form or any other container element, that contains input, select, textarea and button elements.
@param {string} Url - The url to call. If it is null or empty and the container is a html form with a defined action url, then that url is used.
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
@returns {tp.AjaxArgs} Returns a {@link tp.AjaxArgs} {@link Promise}.
*/
tp.Ajax.PostContainerAsync = async function (ElementOrSelector, Url, OnSuccess = null, OnFailure = null, Context = null) {
    var el = tp.Select(ElementOrSelector);
    if (tp.IsBlank(Url) && el instanceof HTMLFormElement) {
        Url = el.action;
    }

    return tp.ContainerToModelAsync(true, el)
        .then(function (Model) {
            return tp.Ajax.PostModelAsync(Url, Model, OnSuccess, OnFailure, Context);
        });
};
/**
Executes a list of tp.AjaxArgs simultaneously, using Promise.all() and returns a promise when all items are done, or in the first rejection.
@param {boolean} ShowSpinner - True to show the global spinner while processing.
@param {tp.AjaxArgs[]} ArgsList An array of tp.AjaxArgs objects
@returns {Promise} Returns a promise after all items are processed succesfully, or in the first rejection.
*/
tp.Ajax.AllAsync = async function (ShowSpinner, ArgsList) {
    let Result = tp.Async.All(ShowSpinner, ArgsList, tp.Ajax.Async);
    return Result;
};

/**
Executes a list of tp.AjaxArgs sequentially, one by one. The next item executes only when the previous is done executing. 
A user provided break callback function can be used to interrupt the execution, just like a break statement in a loop. 
Returns a promise with the last item when all items are done, or in the first rejection.
@param {boolean} ShowSpinner - True to show the global spinner while processing.
@param {tp.AjaxArgs[]} ArgsList An array of tp.AjaxArgs objects
@param {function} [BreakFunc=null] - Optional. A callback function(Args: tp.AjaxArgs): boolean.
Returning true cancels any further execution, just like a break statement in a loop.
@returns {Promise} Returns a promise with the last item when all items are done, or in the first rejection.
*/
tp.Ajax.ChainAsync = async function (ShowSpinner, ArgsList, BreakFunc = false) {
    let Result = tp.Async.Chain(ShowSpinner, ArgsList, tp.Ajax.Async, BreakFunc);
    return Result;
};


/* tp.AjaxArgs preparation helpers */

/**
Creates and returns a tp.AjaxArgs instance for a POST communication
@param {string} Url - The url to call
@param {object} [Data=null] - Optional. The data to sent. Could be null
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
@returns {tp.AjaxArgs} Returns a tp.AjaxArgs object.
*/
tp.Ajax.PostArgs = function (Url, Data = null, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = new tp.AjaxArgs();
    Args.Url = Url;
    Args.Method = 'POST';
    Args.Data = Data;

    Args.Context = Context;
    Args.OnSuccess = OnSuccess;
    Args.OnFailure = OnFailure;

    return Args;
};
/**
Creates and returns a tp.AjaxArgs instance for a GET communication
@param {string} Url - The url to call
@param {object} [Data=null] - Optional. The data to sent. Could be null
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
@returns {tp.AjaxArgs} Returns a tp.AjaxArgs object.
*/
tp.Ajax.GetArgs = function (Url, Data = null, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = new tp.AjaxArgs();
    Args.Url = Url;
    Args.Method = 'GET';
    Args.Data = Data;

    Args.Context = Context;
    Args.OnSuccess = OnSuccess;
    Args.OnFailure = OnFailure;

    return Args;
};
/**
Creates and returns a tp.AjaxArgs instance for a POST-ing a model. The function serializes the model by calling JSON.stringify(). 
It also adjusts the Content-Type header as application/json; charset=utf-8
@param {string} Url - The url to call
@param {object} Model - The model, a plain object, to sent. The function serializes the model by calling JSON.stringify().
@param {function} [OnSuccess=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on success.
@param {function} [OnFailure=null] - Optional. A callback function(Args: tp.AjaxArgs): void to call on failure.
@param {object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
@returns {tp.AjaxArgs} Returns a tp.AjaxArgs object.
*/
tp.Ajax.ModelArgs = function (Url, Model, OnSuccess = null, OnFailure = null, Context = null) {
    let Args = new tp.AjaxArgs();
    Args.Url = Url;
    Args.Method = 'POST';
    Args.ContentType = 'application/json; charset=utf-8';

    if (!tp.IsEmpty(Model)) {
        if ('__RequestVerificationToken' in Model) {
            Args.AntiForgeryToken = Model['__RequestVerificationToken'];
            delete Model['__RequestVerificationToken'];
        }
        Args.Data = tp.IsString(Model) ? Model : JSON.stringify(Model);
    }

    Args.Context = Context;
    Args.OnSuccess = OnSuccess;
    Args.OnFailure = OnFailure;

    return Args;
};
/**
Merges a specified object with the Data part (tp.AjaxArgs.Data) of the ajax arguments. 
@param {tp.AjaxArgs} Args The tp.AjaxArgs object to use.
@param {Object} ExtraData - The object to merge with tp.AjaxArgs.Data
@returns {tp.AjaxArgs} Returns a tp.AjaxArgs object.
*/
tp.Ajax.AddExtraData = function (Args, ExtraData) {
    Args.Data = Args.Data || {};

    if (!tp.IsEmpty(ExtraData)) {
        for (var Prop in ExtraData) {
            Args.Data[Prop] = ExtraData[Prop];
        }
    }

    return Args;
};

//#endregion

//#region tp.Async (Promise)

/** Promise static class/function
* Executes a plain function inside a promise and returns the promise.
* @param {function} Func - A callback function(Info = null): any to promisify.
* @param {any} [Info=null] - Optional. An argument to pass to the callback function.
* @param {object} [Context=null] - Optional. The context (this) to use when calling the callback function.
* @returns {Promise} Returns a Promise.
*/
tp.Async = async function (Func, Info = null, Context = null) {
    let ExecutorFunc = (Resolve, Reject) => {
        try {
            tp.Call(Func, Context, Info);
            Resolve(Info);
        } catch (e) {
            Reject(e);
        }
    };

    var Result = new Promise(ExecutorFunc);
    return Result;
};
/**
Executes multiple promise calls sequentially, that is next is callled ONLY if the previous call is completed.  
Returns a promise with the last item when all items are done, or in the first rejection.

@example
// Info = { Index: number, Flag: boolean }

let Counter = 0;
let ArgsList = [{ Id: 0 }, { Id: 1 }, { Id: 2 }];

let Func = function (Info) {
    return new Promise((resolve, reject) => {
        let timeout = tp.Random(500, 5000);
        setTimeout(() => {
            Info.Index = Counter++;
            log('Id: ' + Info.Id + ' Index: ' + Info.Index + ' Timeout: ' + timeout);
            //if (Info.Index === 2)
            //    throw 'I dont like 2';
            resolve(Info);
        }, timeout);

    });
};

let BreakFunc = function (Info: IInfo): boolean {
    return false;
}

let Result = tp.Async.Chain(true, ArgsList, Func, BreakFunc);
@see {@link https://www.reddit.com/r/typescript/comments/54qe6w/use_reduce_and_promises_to_execute_multiple_async/|promises and reduce}
@param {boolean} ShowSpinner - If true then a spinner overlay div is displayed while the operation executes.
@param {any[]} List - An array of values. Each value is passed, in turn, in a provided callback function.
@param {function} Func - A callback function(v: T): Promise which gets one of the array values and executes inside a promise.
@param {function} [BreakFunc=null] - Optional. A callback function(v: U): boolean called just before every iteration and if returns true the whole operation terminates.
@param {Object} [Context=null] - Optional. The context (this) to use when calling the callback functions.
@returns {Promise} Returns a promise with the last item when all items are done, or in the first rejection.
*/
tp.Async.Chain = async function (ShowSpinner, List, Func, BreakFunc = null, Context = null) {

    let Spinner = function (Flag) {
        if (ShowSpinner) {
            tp.ShowSpinner(Flag);
        }
    };

    Spinner(true);

    let ReduceFunc = (promise, current) => {
        return promise.then((value) => {
            if (tp.IsFunction(BreakFunc) && tp.Call(BreakFunc, Context, value))
                return promise;
            return tp.Call(Func, Context, current);
        });
    };

    let InitialValue = Promise.resolve(null);

    let Result = List.reduce(ReduceFunc, InitialValue);

    if (ShowSpinner) {
        Result.then((value) => {
            Spinner(false);
            return value;
        });
    }

    return Result;
};
/**
Executes multiple promise calls simultaneously, using Promise.all() and returns a promise when all items are done, or in the first rejection.   
@example
// Info = { Index: number, Flag: boolean }

let Counter = 0;
let ArgsList: IInfo[] = [{ Id: 0 }, { Id: 1 }, { Id: 2 }];

let Func = function (Info) {
    return new Promise<IInfo>((resolve, reject) => {
        let timeout = tp.Random(500, 5000);
        setTimeout(() => {
            Info.Index = Counter++;
            log('Id: ' + Info.Id + ' Index: ' + Info.Index + ' Timeout: ' + timeout);
            //if (Info.Index === 2)
            //    throw 'I dont like 2';
            resolve(Info);
        }, timeout);

    });
}; 

let Result = tp.Async.All(true, ArgsList, Func);
@param {boolean} ShowSpinner - True to show the global spinner while processing.
@param {any[]} List - An array of values. Each value is passed in the provided callback function.
@param {function} Func - A callback <code>function(v: T): Promise<U></code> which gets one of the array values and executes inside a promise.
@param {object} Context=null - Optional. The context (this) to use when calling the callback functions.
@returns {Promise} Returns a promise after all items are processed succesfully, or in the first rejection.
*/
tp.Async.All = async function (ShowSpinner, List, Func, Context = null) {

    let Spinner = function (Flag) {
        if (ShowSpinner) {
            tp.ShowSpinner(Flag);
        }
    };

    Spinner(true);

    var A = List.map((Item) => {
        return tp.Call(Func, Context, Item);
    });

    var Result = Promise.all(A);

    if (ShowSpinner) {
        Result.then((value) => {
            Spinner(false);
            return value;
        });
    }

    return Result;
};


//#endregion

//---------------------------------------------------------------------------------------
// tpObject and tpElement
//---------------------------------------------------------------------------------------

//#region tpObject

/**
The ultimate base class.
*/
tp.tpObject = class {

    /* construction */
    constructor() {
        this.InitClass();
    }

    /* properties */
    /**
    When false, calling Trigger() does NOT inform any listener. Defaults to true;
    @type {boolean}
    */
    get EventsEnabled() { return this.fEventsEnabledCounter >= 0; }
    set EventsEnabled(v) {
        this.fEventsEnabledCounter = v === true ? this.fEventsEnabledCounter + 1 : this.fEventsEnabledCounter - 1;
    }


    /* protected */
    /**
    Returns the invocation list, that is an array of Listener objects, for a specified event. The returns array could be empty.
    @param {string} EventName The event name
    @returns {tp.Listener[]} Returns an array with the tp.Listener listeners of the event, or an empty array.
    */
    GetInvocationList(EventName) {
        let Result = [];
        if (this.fEvents && !tp.IsBlank(EventName)) {
            EventName = EventName.toUpperCase();
            Result = this.fEvents[EventName] || [];
        }
        return Result;
    }
    /**
    Returns true if this instance has subscribed listeners for an event specified by its name.
    @param {string} EventName The event name
    @returns {boolean}  Returns true if this instance has subscribed tp.Listener listeners for an event specified by its name.
    */
    HasListeners(EventName) {
        return this.GetInvocationList(EventName).length > 0;
    }
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    */
    InitClass() {
        this.tpClass = 'tp.tpObject';
        this.fJsonExcludes = ['tpClass', 'EventsEnabled'];
    }
    /**
    Returns true if a proprety of this instance should included in serialization
    @param {string} Prop - The name of the property
    @returns {boolean} Returns true if a proprety of this instance should included in serialization
    */
    CanSerialize(Prop) { return typeof this[Prop] !== 'function' && this.fJsonExcludes.indexOf(Prop) === -1 && Prop.charAt(0) !== 'f'; }

    /* public */
    /**
    If an object being stringified has a property named toJSON whose value is a function, then the toJSON() method customizes JSON stringification behavior.
    Instead of the object being serialized, the value returned by the toJSON() method when called will be serialized.
    @see {@link http://www.ecma-international.org/ecma-262/5.1/#sec-15.12.3|specification}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify|mdn}
    @return {object} Returns a plain object having all the properties and values of this instance. JSON.stringify() uses that returned object instead of this instance when serializing.
    */
    toJSON() {
        var Result = {};

        for (var Prop in this) {
            if (this.CanSerialize(Prop))
                Result[Prop] = this[Prop];
        }

        return Result;
    }


    /**
    True if the specified object is of the same class as this instance or derives from it.
    @param {object} o - The object to check
    @returns {boolean} True if the specified object is of the same class as this instance or derives from it.
    */
    IsSameClass(o) { return o instanceof this.constructor; }
    /**
    Creates and returns an instance (object) of this class.
    @param {...Args}  Args A rest parameter
    @returns {tp.tpObject} Returns a tp.tpObject.
    */
    CreateInstance(...Args) {
        var Ctor = this.constructor;
        return tp.CreateInstance(Ctor, Args);
    }

    /**
    Assigns a source instance to this instance
    @param {any} Source The source instance
    */
    Assign(Source) {
        this.Clear();
        if (Source) {
            tp.MergePropsDeep(this, Source);
        }
    }
    /**
    Clones this instance and returns the result
    @returns {object} Returns the clone object.
    */
    Clone() {
        var Result = this.CreateInstance();
        Result.Assign(this);
        return Result;
    }
    /**
    Clears the values in fields/properties of this instance
    */
    Clear() {
    }



    /**
    Adds a listener to an event of this instance and returns the listener object. 
    The caller specifies the event name, a callback function and a context for the callback.  
    When the event occurs then this instance invokes the callback passing a single parameter of type tp.EventArgs.
    @example
    MyObject.On('OnSomething', function(Args: tp.EventArgs) {
        //
    });

    // or
    MyObject.On('OnSomething', this.HandlerFunc, this);

    // or, if the caller is a tp.tpObject
    MyObject.On('OnSomething', this.FuncBind(this.HandlerFunc));

    @param {string} EventName - Can be any event name this instance supports.
    @param {function} Func - A function(Args: tp.EventArgs): void to call back when the event occurs.
    @param {object} [Context=null] - Optional. The context (this) to use when calling the callback function.
    @returns {tp.Listener} Returns the created listener object
    */
    On(EventName, Func, Context = null) {

        if (!tp.IsBlank(EventName) && tp.IsFunction(Func)) {
            if (!this.fEvents)
                this.fEvents = {};

            EventName = EventName.toUpperCase();

            if (!(EventName in this.fEvents))
                this.fEvents[EventName] = []; // new invocation list for the event

            let L = new tp.Listener();
            L.Func = Func;
            L.Context = Context;

            this.fEvents[EventName].push(L);

            return L;
        }

        return null;
    }
    /**
    Removes a listener of the EventName event of this instance. That is does the opposite of On() method.
    @param {string} EventName - The name of the event used in a previous call to On().
    @param {tp.Listener|function} ListenerOrFunc - The listener object returned by a previous call to On() with the same event name or
    the callback function passed to a previous call to On() with the same event name.
    */
    Off(EventName, ListenerOrFunc) {
        EventName = arguments[0].toUpperCase();
        let InvocationList = this.GetInvocationList(EventName);
        if (InvocationList.length > 0) {

            // find it
            let Arguments = arguments;
            let L = null;
            if (arguments[1] instanceof tp.Listener) {
                L = tp.FirstOrDefault(InvocationList, (item) => item === Arguments[1]);
            } else if (tp.IsFunction(arguments[1])) {
                L = tp.FirstOrDefault(InvocationList, (item) => item.Func === Arguments[1]);
            }

            // remove it
            if (L) {
                InvocationList.splice(InvocationList.indexOf(L), 1);
            }
        }
    }
    /**
    Triggers EventName event passing Args. It calls all previously registered listeners. 
    NOTE: If EventsEnabled is false, nothing happens.
    @example
    this.Trigger('OnSomething', { });
    @param {string} EventName - The name one of the events this class supports.
    @param {tp.EventArgs|object} [Args=null] - Optional. The arguments to be passed to the listeners. Could be a plain javascript object or a tp.EventArgs instance.
*/
    Trigger(EventName, Args) {
        if (this.EventsEnabled === true && this.fEvents && !tp.IsBlank(EventName)) {
            let sEventName = EventName;
            EventName = EventName.toUpperCase();
            var InvocationList = this.GetInvocationList(EventName);
            if (InvocationList && InvocationList.length > 0) {

                Args = Args instanceof tp.EventArgs ? Args : new tp.EventArgs(Args);
                Args.EventName = sEventName;
                Args.Sender = this;
                var Listener;
                var i, ln;

                for (i = 0, ln = InvocationList.length; i < ln; i++) {
                    Listener = InvocationList[i];
                    Listener.Func.call(Listener.Context, Args);
                }
            }
        }
    }

    /* event handler */
    /**
    Implementation of the DOM EventListener interface. 
    For handling all DOM element events. Either when this is a DOM element and the sender (target) of the event is this.Handle
    or when the sender (target) of the event is any other object and listener is this instance.
    @see {@link http://www.w3.org/TR/DOM-Level-2-Events/events.html#Events-EventListener|specification}
    @see {@link https://medium.com/@WebReflection/dom-handleevent-a-cross-platform-standard-since-year-2000-5bf17287fd38|handleEvent}
    @param {Event} e The event to handle
     */
    handleEvent(e) {
    }


    /**
    Binds a specified Function to this, if not already bound, and returns the result. 
    Useful in passing methods of this instance as callbacks and event handlers.
    @example
    var x = this.FuncBind(this.MyFunction);
    @param {function} Func The function of this instance to bind.
    @returns {any} Returns the result of the Func.bind(this) call
    */
    FuncBind(Func) {
        if (tp.IsFunction(Func)) {

            var IndexOfFuncBind = function (Func, Binds) {
                if (tp.IsFunction(Func) && Binds) {
                    for (var i = 0, ln = Binds.length; i < ln; i++) {
                        if (Binds[i].Func === Func) {
                            return i;
                        }
                    }
                }

                return -1;
            };


            var o = null;
            var Index = IndexOfFuncBind(Func, this.fBinds);
            if (Index < 0) {
                if (!this.fBinds) {
                    this.fBinds = [];
                }

                o = {
                    Func: Func,
                    Bind: Func.bind(this)
                };

                this.fBinds.push(o);
            } else {
                o = this.fBinds[Index];
            }

            return o.Bind;
        }

        return null;

    }

};


tp.tpObject.prototype.tpClass = '';                         // treat it as read-only class field (static)
tp.tpObject.prototype.fEventsEnabledCounter = 0;
tp.tpObject.prototype.fJsonExcludes = null;                 // string[]
tp.tpObject.prototype.fEvents = null;                       // { [EventName: string]: Listener[]; }  
tp.tpObject.prototype.fBinds = null;                        // { Func: Function, Bind: any } [] = null;


// tpObject association ---------------------------------------------------------------
/**
Returns the Tripous script object associated to a DOM element, if any or null.  
NOTE: When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class.
Also a new property, named tpObject, is created and attached to the DOM element object, pointing to the Tripous object.
@param {string|Node} el - The element to get the associated tripous script object from.
@returns {tp.tpObject} Returns the Tripous script object associated to a DOM element, if any or null.
*/
tp.GetObject = function (el) {
    if (tp.IsString(el))
        el = tp.Select(el);

    if (tp.IsElement(el)) {
        if (el['tpObject']) //  && el['tpObject'] instanceof tp.tpObject
            return el['tpObject'];
    }

    return null;
};
/**
Associates a DOM element to a Tripous script object.
NOTE: When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class.
Also a new property, named tpObject, is created and attached to the DOM element object, pointing to the Tripous object.
@param {string|Node} el - The element to associate with the Tripous script object from.
@param {tp.tpObject} v - The Tripous script object.
*/
tp.SetObject = function (el, v) {
    if (tp.IsString(el))
        el = tp.Select(el);

    if (tp.IsElement(el))
        el['tpObject'] = v;
};
/**
Returns true if a specified DOM element is associated to a tp.tpElement
NOTE: When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class.
Also a new property, named tpObject, is created and attached to the DOM element object, pointing to the Tripous object.
@param {string|Node} el The element to check
@returns {boolean} Returns true if a specified DOM element is associated to a tp.tpElement
*/
tp.HasObject = function (el) { return tp.GetObject(el) !== null; };
/**
Returns an array with all tp.Element objects constructed/existing up on child DOM elements of a specified parent element or the entire document. 
NOTE: When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class.
Also a new property, named tpObject, is created and attached to the DOM element object, pointing to the Tripous object.
@param {string|Node} ParentElementOrSelector - Defaults to document. The container of controls. If null/undefined/empty the document is used.
@returns {tp.tpObject[]} Returns an array with tp.tpObject objects constructed up on elements of a parent element
*/
tp.GetObjects = function (ParentElementOrSelector) {
    var elParent = null;

    if (!tp.IsEmpty(ParentElementOrSelector)) {
        elParent = tp.Select(ParentElementOrSelector);
    }

    elParent = elParent || document;
    var NodeList = tp.SelectAll(elParent, '.tp-Object');
    var List = []; // tp.tpObject[]
    let o;

    if (!tp.IsEmpty(NodeList)) {
        for (var i = 0, ln = NodeList.length; i < ln; i++) {
            o = tp.GetObject(NodeList[i]);
            if (o)
                List.push(o);
        }
    }

    return List;
};
/**
Returns a tp.tpObject instance of a specified tp.tpObject class starting the search from a specified dom node/element and going all the parents up until the document.body is reached.
Returns null if nothing is found. 
NOTE: To be used from inside event handler methods in order to find the right tp.tpObject sender instance.
 * @param {Node} el - The element that plays the role of the starting point.
 * @param {object} ElementClass A tp.tpElement class or a descendant, to check for a match.
 * @returns {tp.tpObject} Returns a tp.tpObject or null
 */
tp.GetContainerByClass = function (el, ElementClass) {

    let o;

    while (true) {
        if (el === el.ownerDocument.body)
            return null;

        if (el) {
            o = tp.GetObject(o);

            if (o instanceof ElementClass)
                return o;
        }

        el = el.parentNode;
    }

};


//#endregion

//#region CreateParams

/**
Represenst an initialization options list that is passed to a {@link tp.tpElement} constructor.
@class
*/
tp.CreateParams = class {
    /**
    Constructor.
    @param {object} [Source=null] - Optional. A source object to copy properties from.
    */
    constructor(Source = null) {
        if (Source) {
            tp.MergeQuick(this, Source);
        }
    }
};

/** The parent of the <code>tp.tpElement</code> being initialized
 @type {HTMLElement|tp.tpElement}
 @default null
 */
tp.CreateParams.prototype.Parent = null;             // HTMLElement | tp.tpElement;
/** The DOM element upon the new <code>tp.tpElement</code> is going to be constructed
 @type {HTMLElement|string}
 @default null
 */
tp.CreateParams.prototype.ElementOrSelector = null;   // string | HTMLElement;
/** The id of the new element
 @type {string}
 @default ''
 */
tp.CreateParams.prototype.Id = '';
/** The name of the new element, if applicable. For <code>input</code> elements.
 @type {string}
 @default ''
 */
tp.CreateParams.prototype.Name = '';
/** The <code>innerHTML</code> of the new element, if applicable.
 @type {string}
 @default ''
 */
tp.CreateParams.prototype.Html = '';
/** A list of css class names, space delimited, for the new element
 @type {string}
 @default ''
 */
tp.CreateParams.prototype.CssClasses = '';
/** The inline css style of the new element
 @type {string}
 @default ''
 */
tp.CreateParams.prototype.CssText = '';
/** When no DOM element is passed to the <code>tp.tpElement</code> constructor a new DOM element is created by the constructor, by calling the <code>CreateHandle()</code> method. <br />
When this property is set to true, then the constructor does NOT call the <code>CreateHandle()</code> and it is the caller responsibility to call it at a later time.
 @type {boolean}
 @default false
 */
tp.CreateParams.prototype.DeferHandleCreation = false;
//#endregion

//#region tpElement

//---------------------------------------------------------------------------------------
// tp.tpElement
//---------------------------------------------------------------------------------------


/**
Represents a HTML element. <br />
CAUTION: Properties (i.e. fields) in derived classes, in order to be visible from the super constructor, <br />
should either belong to the derived class prototype, e.g. MyDerivedClass.prototype.MyProperty <br />
or provide a getter and setter function. <br />
Example:

tp.Button = class extends tp.tpElement {

    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    // NOT visible in the super constructor. 
    // In addition this initial value here is passed to the instance AFTER the super constructor finishes.
    // So if this property is initialized somehow by the super constructor, that value is lost. And this value here prevails.
    // So put that kind of properties to MyDerivedClass.prototype.XXXX to be safe.
    Command = '';  
}

Ofcourse it's insane.
*/
tp.tpElement = class extends tp.tpObject {

    /**
    Constructor
    @param {string|HTMLElement} [ElementOrSelector=null] - Optional. If a CreateParams is passed having its DeferHandleCreation to true, then then the
    constructor does NOT call the CreateHandle() and it is the caller responsibility to call it at a later time..
    When this property is set to true, then the constructor does NOT call the CreateHandle() and it is the caller responsibility to call it at a later time.
    @param {object|tp.CreateParams} [CreateParams=null]- Optional. The creation parameters for the element.
    */
    constructor(ElementOrSelector = null, CreateParams = null) {
        super();    // this calls the InitClass() method

        this.CreateParams = {};

        if (tp.IsObject(CreateParams)) {
            this.CreateParams = CreateParams;

            if (tp.IsEmpty(ElementOrSelector) && !tp.IsEmpty(CreateParams.ElementOrSelector)) {
                let el = tp.Select(CreateParams.ElementOrSelector);
                if (el instanceof HTMLElement)
                    ElementOrSelector = el;

                delete this.CreateParams.ElementOrSelector;
            }
        }

        let Flag = this.fDeferHandleCreation || this.CreateParams && Boolean(this.CreateParams.DeferHandleCreation);

        if (!Flag) {
            this.CreateHandle(ElementOrSelector);
        }
    }

    
    /* protected - overridable getters and setters  */

    /** Returns the value of the Enabled property, true if this instance is enabled.
     * @returns {boolean} Returns true if this instance is enabled. 
     * */
    get_Enabled() { return this.fEnabled; }
    /**
     * Sets the Enabled property.
     * @param {boolean} v Sets the Enabled property.
     */
    set_Enabled(v) {
        v = v === true;
        if (this.Handle && v !== this.Enabled) {
            this.fEnabled = v;
            tp.Enabled(this.Handle, v);
        }
    }

    /* non-dom properties */
    /** Returns a value used with the display style property, when the element is visible 
     * @type {string}
     */
    get DisplayType() {
        if (tp.IsBlank(this.fDisplayType)) {
            if (!this.Handle) {
                this.fDisplayType = 'block';
            } else {
                var S = tp.GetComputedStyle(this.Handle).display;
                this.fDisplayType = !tp.IsSameText('none', S) ? S : 'block';
            }
        }

        return this.fDisplayType;
    }
    /** Returns the type of this element, i.e. the desired nodeName property value      
     * @type {string}
     */
    get ElementType() { return this.fElementType; }
    /** Used when this is an input element. Returns the input type of this element, i.e. the desired type attribute value 
     * @type {string}
     */
    get ElementSubType() { return this.fElementSubType; }

 
    /** Returns true when the handle of this instance comes from the html markup, i.e. the dom element exists in markup and it is not dynamically created by code. 
     * @type {boolean}
     */
    get FromMarkup() { return this.fFromMarkup; }
    /** Returns true after the Dispose() is called indicating that this instance is no longer usable. 
     * @type {boolean}
     */
    get IsDisposed() { return this.fIsDisposed; }


    /* properties */
    /** The DOM document this element belongs to. 
     * @type {HTMLDocument}
     */
    get Document() { return this.fDocument || tp.Doc; }
    /** Returns the number of direct HTMLElement children of this element 
     * @type {number}
     */
    get Count() { return this.Handle ? tp.ChildHTMLElements(this.Handle).length : 0; }

    /**
    The actual dom element (HTMLElement). The handle of this instance.
    This can be set just once to an HTMLElement value. 
    After that only null is allowed which ends up calling the DestroyHandle() method.
    @type {HTMLElement}  
    */
    get Handle() { return this.fHandle; }
    set Handle(v) {
        if (tp.IsEmpty(this.fHandle) && tp.IsHTMLElement(v)) {
            this.fHandle = v;
            tp.SetObject(this.Handle, this);
        } else if (tp.IsHTMLElement(this.fHandle) && tp.IsEmpty(v)) {
            this.Dispose();
        }
    }
    /**  Gets or sets the id attribute 
     * @type {string}
     */
    get Id() { return this.Handle && this.Handle.id ? this.Handle.id : ''; }
    set Id(v) {
        if (this.Handle && tp.IsString(v)) {
            this.Handle.id = v;

            // set the name too, if it is blank
            if ('name' in this.Handle && tp.IsBlank(tp.Attribute(this.Handle, 'name')))
                tp.Attribute(this.Handle, 'name', v);
        }
    }
    /** Gets or sets the name attribute 
     * @type {string}
     */
    get Name() { return tp.IsNamedHtmlElement(this.Handle) ? this.Handle.name : ''; }
    set Name(v) {
        if (tp.IsString(v) && 'name' in this.Handle)
            tp.Attribute(this.Handle, 'name', v);
    }
    /**  Returns the element tag name, i.e div, etc 
     * @type {string}
     */
    get NodeType() { return this.Handle && this.Handle.nodeName ? this.Handle.nodeName : ''; }
    /** Gets or sets the parent tp.tpElement 
     * @type {tp.tpElement}
     */
    get Parent() {
        var el = this.ParentHandle;
        let o = tp.GetObject(el);
        return o instanceof tp.tpElement ? o : null;
    }
    set Parent(v) {
        this.SetParent(v);
    }
    /**
    Gets or sets the parent DOM element
    @type {HTMLElement}
    */
    get ParentHandle() {
        return this.Handle ? this.Handle.parentNode : null;
    }
    set ParentHandle(v) {
        if (v instanceof HTMLElement)
            this.SetParent(v);
    }


    /**
    Gets or sets the innerHTML property of the element
    @type {string}
    */
    get Html() { return this.Handle ? this.Handle.innerHTML : ''; }
    set Html(v) {
        if (tp.IsString(v) && this.Handle)
            this.Handle.innerHTML = v;
    }
    /**
    Gets or sets the value or textContent or innerHTML, depending on the node type
    @type {string}
    */
    get Text() { return this.Handle ? tp.val(this.Handle) : ''; }
    set Text(v) {
        let S = '';
        if (this.Handle) {

            if (tp.IsEmpty(v)) 
                S = '';
            else  
                S = tp.IsString(v) ? v : v.toString();            
        }
        tp.val(this.Handle, v);
           
    }
    /**
    Gets or sets the title attribute
    @type {string}
    */
    get ToolTip() { return this.Handle ? this.Handle.title : ''; }
    set ToolTip(v) {
        if (tp.IsString(v) && this.Handle)
            this.Handle.title = v;
    }
    /**
    Gets or sets a boolean value indicating whether the element may be checked for spelling errors.
    @type {boolean}
    */
    get SpellCheck() { return this.Handle ? this.Handle.spellcheck : false; }
    set SpellCheck(v) { this.Handle.spellcheck = v === true; }
    /**
    Gets or sets the defaultValue attribute if applicable
    @type {string}
    */
    get DefaultValue() { return this.Handle && 'defaultValue' in this.Handle ? tp.Attribute(this.Handle, 'defaultValue') : ''; }
    set DefaultValue(v) {
        if (tp.IsString(v) && this.Handle && 'defaultValue' in this.Handle)
            tp.Attribute(this.Handle, 'defaultValue', v);
    }

    /**
    Gets or sets the position style property, i.e. relative, absolute, fixed, static.
    @type {string}
    */
    get Position() { return this.StyleProp('position'); }
    set Position(v) {
        this.StyleProp('position', v);
    }

    /**
    Enables or disables the element by setting the disabled attribute, if applicable
    @type {boolean}
    */
    get Enabled() { return this.get_Enabled(); }
    set Enabled(v) {
        let OldValue = this.Enabled;
        this.set_Enabled(v);
        if (OldValue !== this.Enabled) {
            this.OnEnabledChanged();
        }
    }
    /**
    Shows or hides the element by setting the display style property.
     @type {boolean}
    */
    get Visible() { return this.Handle ? this.Handle.style.display === '' : false; }
    set Visible(v) {
        if (this.Handle) {
            v = v === true;
            if (this.Visible !== v) {
                this.Handle.style.display = v ? '' : 'none';
                this.OnVisibleChanged();
            }
        }
    }
    /**
    Shows or hides the element by setting the visibility style property.
    @type {boolean}
    */
    get Visibility() { return this.Handle ? tp.IsSameText(this.GetComputedStyle().visibility, "visible") : false; }
    set Visibility(v) {
        this.Handle.style.visibility = v === true ? "visible" : "hidden";
    }

    /**
    Gets or sets the opacity style property. Opacity is a float number from 0.0 to 1.0
    @type {number}
    */
    get Opacity() { return this.Handle ? tp.StrToFloat(this.GetComputedStyle().opacity) : 0; }
    set Opacity(v) {
        if (this.Handle)
            this.Handle.style.opacity = v.toString();
    }

    /**
    Gets or sets the background color
    @type {string}
    */
    get BackColor() { return this.Handle ? this.GetComputedStyle().backgroundColor : ''; }
    set BackColor(v) {
        if (this.Handle)
            this.Handle.style.backgroundColor = v;
    }
    /**
    Gets or sets the font color
    @type {string}
    */
    get FontColor() { return this.Handle ? this.GetComputedStyle().color : ''; }
    set FontColor(v) {
        if (this.Handle)
            this.Handle.style.color = v;
    }

    /**
    Gets or sets the z-index
    @see {@link http://philipwalton.com/articles/what-no-one-told-you-about-z-index/|article}
    @see {@link http://www.w3.org/TR/CSS2/zindex.html|specification}
    @type {number}
    */
    get ZIndex() {
        if (this.Handle) {
            var Result = this.GetComputedStyle().zIndex;
            return isNaN(Result) ? 0 : Number(Result);
        }

        return -1;
    }
    set ZIndex(v) {
        if (this.Handle)
            this.Handle.style.zIndex = v.toString();
    }
    /**
    Gets or sets the the mouse cursor - for valid values see tp.Cursors
    @type {string}
    */
    get Cursor() { return this.Handle ? this.GetComputedStyle().cursor : ''; }
    set Cursor(v) {
        this.Handle.style.cursor = v;
    }

    /**
    Gets or sets the text align string. Valid values: left, right, justify
    @type {string}
    */
    get TextAlign() { return this.Handle ? this.GetComputedStyle().textAlign : ''; }
    set TextAlign(v) {
        this.Handle.style.textAlign = v;
    }


    /**
    Gets or sets the tab-index. An element is focusable only if tablIndex > 0
    <pre>
    Tab index   < 0         - can be focused either by a mouse click, or the focus() function
    Tab index   >= 0        - can be focused either by a mouse click, or the focus() function, or the tab key, according to its tab order
    Tab index   unspecifed  - IE sets it to 0 and can be focused either by a mouse click, or the focus() function
                              Chrome and Firefox set it to -1 and it can NOT be focused at all

    Clicking on a nested div (element) with unspecified tab-index
        IE gives focus always to the child
        Chrome and Firefox give focus only to the parent, and only if the parent has its tab-index specified explicitly

    Conclusion:
        It seems that setting tab-index explicitly to -1 is the best choice when the purpose is to control the focus
        either by mouse clicks or by the focus() function
    </pre>
    @see {@link https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/tabindex|mdn}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/Accessibility/Keyboard-navigable_JavaScript_widgets|mdn: Keyboar navigatable widgets}
    @type {number}
    */
    get TabIndex() { return this.Handle ? this.Handle.tabIndex : 0; }
    set TabIndex(v) {
        if (this.Handle)
            this.Handle.tabIndex = v;
    }
    /**
    Returns true if this element is the focused element in the document
    @type {boolean}
    */
    get IsFocused() { return tp.IsFocused(this.Handle); }
    /**
    Returns true if this element is the focused element in the document OR contains the focused element
    */
    get HasFocused() {
        return tp.HasFocused(this.Handle);
    }

    /**
    Gets or sets the className attribute
    @type {string}
    */
    get CssClasses() { return this.Handle ? this.Handle.className : ''; }
    set CssClasses(v) {
        if (this.Handle)
            this.Handle.className = v;
    }

    /**
    Gets or sets the cssText property of the element style property
    @type {string}
    */
    get CssText() { return this.Style ? this.Style.cssText : ''; }
    set CssText(v) {
        if (this.Handle)
            this.Style.cssText = v;
    }



    /**
    Gets or sets the contentEditable attribute which controls whether the contents of the object are editable
    @type {boolean}
    */
    get ContentEditable() { return this.Handle ? tp.IsSameText(this.Handle.contentEditable, "true") : false; }
    set ContentEditable(v) {
        this.Handle.contentEditable = v === true ? "true" : "false";
    }
    /**
    Gets or sets the draggable attribute
    @type {boolean}
    */
    get Draggable() { return this.Handle ? this.Handle.draggable : false; }
    set Draggable(v) {
        if (this.Handle)
            this.Handle.draggable = v === true;
    }

    /**
    Gets of sets the left style property.  
    Could be string or number. If a number is passed then it is considered as pixels
    @type {number|string}
    */
    get X() { return this.Handle ? this.GetComputedStyle().left : null; }
    set X(v) {
        if (this.Handle) {
            this.Handle.style.left = tp.IsNumber(v) ? tp.px(v) : v;
        }
    }
    /**
    Gets of sets the top style property.  
    Could be string or number. If a number is passed then it is considered as pixels
    @type {number|string}
    */
    get Y() { return this.Handle ? this.GetComputedStyle().top : null; }
    set Y(v) {
        if (this.Handle) {
            this.Handle.style.top = tp.IsNumber(v) ? tp.px(v) : v;
        }
    }
    /**
    Gets of sets the width style property. 
    Could be string or number. If a number is passed then it is considered as pixels
    @type {number|string}
    */
    get Width() { return this.Handle ? this.GetComputedStyle().width : null; }
    set Width(v) {
        if (this.Handle) {
            this.Handle.style.width = tp.IsNumber(v) ? tp.px(v) : v;
        }
    }
    /**
    Gets of sets the height style property. 
    Could be string or number. If a number is passed then it is considered as pixels
    @type {number|string}
    */
    get Height() { return this.Handle ? this.GetComputedStyle().height : null; }
    set Height(v) {
        if (this.Handle) {
            this.Handle.style.height = tp.IsNumber(v) ? tp.px(v) : v;
        }
    }
    /**
    Gets of sets the right style property.  
    Could be string or number. If a number is passed then it is considered as pixels
    @type {number|string}
    */
    get Right() { return this.Handle ? this.GetComputedStyle().right : null; }
    set Right(v) {
        if (this.Handle) {
            this.Handle.style.right = tp.IsNumber(v) ? tp.px(v) : v;
        }
    }
    /**
    Gets of sets the bottom style property. 
    Could be string or number. If a number is passed then it is considered as pixels
    @type {number|string}
    */
    get Bottom() { return this.Handle ? this.GetComputedStyle().bottom : null; }
    set Bottom(v) {
        if (this.Handle) {
            this.Handle.style.bottom = tp.IsNumber(v) ? tp.px(v) : v;
        }
    }

    /**
    Gets or sets the number of pixels by which the contents of an object are scrolled to the left
    @type {number}
    */
    get ScrollLeft() { return this.Handle ? this.Handle.scrollLeft : 0; }
    set ScrollLeft(v) {
        if (this.Handle)
            this.Handle.scrollLeft = v;
    }
    /**
    Gets or sets the number of pixels by which the contents of an object are scrolled upward
    @type {number}
    */
    get ScrollTop() { return this.Handle ? this.Handle.scrollTop : 0; }
    set ScrollTop(v) {
        if (this.Handle)
            this.Handle.scrollTop = v;
    }

    /**
    Returns the rectangle of an element relative to the Top/Left of its parent element.
    @type {tp.Rect}
    */
    get OffsetRect() { return this.Handle ? tp.OffsetRect(this.Handle) : new tp.Rect(0, 0, 0, 0); }
    /**
    Returns the offset size (Width and Height) of the Element (that is an area including padding, border and scroll-bar, if visible, but not the margin).
    @type {tp.Size}
    */
    get OffsetSize() {
        if (this.Handle) {
            return new tp.Size(this.Handle.offsetWidth, this.Handle.offsetHeight);
        }
        return new tp.Size(0, 0);
    }

    /**
    Gets the computed style 
    @type {CSSStyleDeclaration}
    */
    GetComputedStyle() {
        if (!this.Handle)
            tp.Throw('Cannot get computed css style. No handle yet.');
 
        return this.Handle.ownerDocument.defaultView.getComputedStyle(this.Handle, '');
    }
    /**
    Gets the style that is applied to the element level (inline style)
    @type {CSSStyleDeclaration}
    */
    get Style() { return this.Handle ? this.Handle.style : null; }

    /**
    Registers or unregisters this instance as a listener to element size changes.
    Registered instances receive notifications in their OnElementSizeChanged() method.
    @type {boolean}
    */
    get IsElementResizeListener() { return !tp.IsEmpty(this.fResizeDetector); }
    set IsElementResizeListener(v) {
        v = v === true;

        if (this.Handle && v !== this.IsElementResizeListener) {
            if (v) {
                if (tp.IsEmpty(this.fResizeDetector))
                    this.fResizeDetector = new tp.ResizeDetector(this.Handle, this.OnElementSizeChanged, this, true);
                else
                    this.fResizeDetector.Start();
            }
            else if (!tp.IsEmpty(this.fResizeDetector)) {
                this.fResizeDetector.Stop();
            }
        }
 
    }
    /**
    Registers or unregisters this instance as a listener to screen size changes and screen mode (XSmall, Small, Medium, Large) changes.
    Registered instances receive notifications in their OnScreenSizeChanged() method.
    @type {boolean}
    */
    get IsScreenResizeListener() { return !tp.IsEmpty(this.fScreenResizeListener); }
    set IsScreenResizeListener(v) {
        v = v === true;
        if (this.Handle && v !== this.IsScreenResizeListener) {
            if (v) {
                this.fScreenResizeListener = tp.Viewport.AddListener(this.OnScreenSizeChanged, this);
            } else {
                tp.Viewport.RemoveListener(this.fScreenResizeListener);
                this.fScreenResizeListener = null;
            }
        }
    }


    /* protected */

    /**
    Initializes the 'static' and 'read-only' class fields
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.tpElement';

        this.fElementType = 'div';
        this.fDisplayType = '';         // by default, either the css defined or block
        this.fDefaultCssClasses = '';
        this.fAutoId = false;
    }
    /**
    Initializes fields and properties just before applying the create params.        
    */
    InitializeFields() {
        this.fDeferHandleCreation = false;
        this.fAutoId = false;
        this.fEnabled = true;
    }
 

    /**
    Notification sent by tp.Viewport when the screen (viewport) size changes. 
    This method is called only if this.IsScreenResizeListener is true.
    @param {boolean} ScreenModeFlag - Is true when the screen mode (XSmall, Small, Medium, Large) is changed as well.
    */
    OnScreenSizeChanged(ScreenModeFlag) { }
    /**
    Notification sent by tp.ResizeDetector when the size of this element changes.
    This method is called only if this.IsElementResizeListener is true.
    @param {object} ResizeInfo An object of type <code>{Width: boolean, Height: boolean}</code>
    */
    OnElementSizeChanged(ResizeInfo) {
        this.OnResized(ResizeInfo);
    }
    /**
    Handles any DOM event
    @param {Event} e The Event object
    */
    OnAnyDOMEvent(e) { }
    /**
    Handles any event. Even DOM events are send in this method.
    @param {tp.EventArgs} Args The event args.
    */
    OnAnyEvent(Args) {
        this.Trigger(Args.EventName, Args);
    }


    /* notifications */
    /**
    Notification 
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnHandleCreated() { }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing 
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnFieldsInitialized() { }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization and options (CreateParams) processing  
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnCreateParamsProcessed() { }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing 
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnInitializationCompleted() { }


    /**
     * This should be called OUTSIDE of any constructor, after the constructor finishes. <br />
     * 
     * 
     * WARNING
     * 
     * 
     * In javascript fields contained in a derived class declaration, are initialized after the base constructor finishes.
     * So, if the base constructor calls a virtual method where the derived class initializes a field,
     * when the constructor finishes, the field is assigned again whatever value the class declaration dictates.
     * 
     * NOTE 1: In javascript the function and the class object provide a prototype property.
     * Fields belonging to class (or function) prototype are initialized as in any other OOP language.
     * 
     * NOTE 2: In javascript fields contained in a derived class declaration are NOT part of its prototype.
     * 
     * see: https://stackoverflow.com/questions/58141967/field-initialization-in-a-sub-class-happens-after-constructor-finishes-in-the-pa
     *
     *  A = class {
     *      constructor() {
     *          this.SetFields();
     *      }
     *  
     *      Field1 = 789;
     *  
     *      SetFields() { }
     *  }
     *  
     *  B = class extends A {
     *      constructor() {
     *          super();
     *          this.Display();
     *      }
     *  
     *      Field1 = 123;	// this is assigned again after the SetFields() call, when the constructor completes.
     *  
     *      SetFields() {
     *          super.SetFields();
     *          this.Field1 = 987;
     *          this.Field2 = 'AAA';
     *      }
     *      Display() {
     *          alert(this.Field1);
     *          alert(this.Field2);
     *      }
     *  };
     *  B.prototype.Field2 = 'xxx';
     * 
     * 
     * The result is that the field retains the value it had in the class declaration, the null value.
     *
     * 
     * */
    OnAfterConstruction() {
    }


    /* event triggers */
    /**
    Event trigger
    */
    OnParentChanged() { this.Trigger('ParentChanged', {}); }
    /**
    Event trigger
    */
    OnEnabledChanged() { this.Trigger('EnabledChanged', {}); }
    /**
    Event trigger
    */
    OnVisibleChanged() { this.Trigger('VisibleChanged', {}); }
    /**
    Event trigger
    @param {object} ResizeInfo An object of type <code>{Width: boolean, Height: boolean}</code>
    */
    OnResized(ResizeInfo) {
        this.PropagateSize();
        this.Trigger('Resized', ResizeInfo);
    }


    //---------------------------------------------------------------------------------------
    // public
    //---------------------------------------------------------------------------------------

    /**
    Constructs and returns an id for a specified element.
    @param {HTMLElement} [el=null] Optional. If not specified the this.Handle is used
    @return {string} Returns the Id
    */
    ConstructId(el) {
        el = el || this.Handle;

        if (tp.IsElement(el)) {
            var s = '';

            if (this.tpClass === 'tp.Element') {
                s = el.nodeName;
                s = s.toLowerCase();
            } else {
                var Parts = tp.Split(this.tpClass, '.');
                s = Parts[Parts.length - 1];
            }

            return tp.SafeId(s);
        }
        return '';
    }

    /* self handling */
    /**
    Sets the parent of this instance.
    @param {tp.tpElement|HTMLElement} Parent - The parent
    */
    SetParent(Parent) {
        if (this.Handle) {
            if (Parent instanceof tp.tpElement) {
                Parent = Parent.Handle;
            }

            if (tp.IsEmpty(Parent)) {
                this.RemoveFromDom();
            } else {
                this.AppendTo(Parent);
            }
        }
    }
    /**
    Appends this element to a parent
    @param {tp.tpElement|HTMLElement} Parent - The parent
    */
    AppendTo(Parent) {
        if (this.Handle) {
            if (Parent instanceof tp.tpElement && Parent.Handle) {
                Parent.Handle.appendChild(this.Handle);
                this.OnParentChanged();
            } else if (tp.IsHTMLElement(Parent)) {
                Parent.appendChild(this.Handle);
                this.OnParentChanged();
            }
        }
    }
    /**
    Inserts this instance to DOM, before a reference node.
    @param {tp.tpElement|HTMLElement} RefNode - The reference node
    */
    InsertBefore(RefNode) {
        if (this.Handle) {
            if (RefNode instanceof tp.tpElement && RefNode.ParentHandle) {
                RefNode.ParentHandle.insertBefore(this.Handle, RefNode.Handle);
                this.OnParentChanged();
            } else if (tp.IsHTMLElement(RefNode) && RefNode.parentNode) {
                RefNode.parentNode.insertBefore(this.Handle, RefNode);
                this.OnParentChanged();
            }
        }
    }
    /**
    Inserts this instance as a child to a parent, at a specified position
    @param {number} Index - The index among siblings.
    @param {tp.tpElement|HTMLElement} Parent - The parent
    */
    InsertAt(Index, Parent) {
        if (this.Handle && Parent) {
            var elParent = null;
            if (Parent instanceof tp.tpElement) {
                elParent = Parent.Handle;
            } else if (tp.IsElement(Parent)) {
                elParent = Parent;
            }

            if (elParent instanceof HTMLElement) {
                let List = tp.ChildHTMLElements(elParent);

                var Count = List.length;

                if (Count === 0) {
                    elParent.appendChild(this.Handle);
                } else {
                    var RefNode = List[Index];
                    this.InsertBefore(RefNode);
                }
            }
        }
    }
    /**
    Removes this instance from DOM, if it has a parent element
    */
    RemoveFromDom() {
        if (this.Handle && this.ParentHandle) {
            this.ParentHandle.removeChild(this.Handle);
            this.OnParentChanged();
        }
    }

    /**
    Creates the handle (element) and initializes this instance.
    @param {string|HTMLElement} ElementOrSelector - A selector or an HTMLElement. That element becomes the handle of this instance.
    */
    CreateHandle(ElementOrSelector) {

        if (tp.IsEmpty(this.Handle)) {

            this.fFromMarkup = false;
            var el = null;  //  string | HTMLElement | Element

            // ensure we have an element
            if (tp.IsElement(ElementOrSelector)) {
                el = ElementOrSelector;
                this.fFromMarkup = true;
            } else if (tp.IsString(ElementOrSelector) && !tp.IsBlank(ElementOrSelector)) {
                if (tp.ListContainsText(tp.tpElement.StandardNodeTypes, ElementOrSelector)) {
                    el = this.Document.createElement(ElementOrSelector);
                } else {
                    el = tp.Select(ElementOrSelector);
                    this.fFromMarkup = tp.IsElement(el);
                }
            }


            if (!tp.IsHTMLElement(el)) {
                el = this.Document.createElement(this.ElementType);
            }


            if (tp.IsHTMLElement(el)) {

                this.fDocument = el.ownerDocument;

                // it is an input element
                if (el instanceof HTMLInputElement || el instanceof HTMLButtonElement || el instanceof HTMLSelectElement) {
                    if (!tp.IsBlank(this.ElementSubType)) {
                        tp.Attribute(el, 'type', this.ElementSubType);
                    }
                }

                this.fHandle = el;

                // create params
                this.CreateParams = this.CreateParams || {};                  // options passed in to the constructor
                let DataSetup = tp.GetDataSetupObject(el);                    // options defined in a data-* (data-setup) attribute, as javascript object
                this.CreateParams = tp.MergeQuick(this.CreateParams, DataSetup);

                // css classes  
                if (!tp.IsBlank(this.fDefaultCssClasses)) {
                    tp.AddClasses(el, this.fDefaultCssClasses);
                }

                if (!tp.IsEmpty(this.CreateParams.CssClasses) && !tp.IsBlank(this.CreateParams.CssClasses)) {
                    this.CreateParams.CssClasses = tp.IsBlank(el.className) ? this.CreateParams.CssClasses : this.CreateParams.CssClasses + ' ' + el.className;
                    el.className = '';
                    tp.AddClasses(el, this.CreateParams.CssClasses);
                }

                

                // css text
                if (!tp.IsEmpty(this.CreateParams.CssText) && !tp.IsBlank(this.CreateParams.CssText)) {
                    el.style.cssText = this.CreateParams.CssText;
                }

                // id
                if (tp.IsBlank(el.id)) {
                    if (!tp.IsBlank(this.CreateParams.Id))
                        el.id = this.CreateParams.Id;
                    else if (this.fAutoId === true)
                        el.id = this.ConstructId(el);
                }

                // name
                if (!tp.IsEmpty(this.CreateParams.Name) && !tp.IsBlank(this.CreateParams.Name) && 'name' in el && tp.IsBlank(el['name']))
                    el['name'] = this.CreateParams.Name;

                // Html
                if (!tp.IsEmpty(this.CreateParams.Html) && !tp.IsBlank(this.CreateParams.Html))
                    el.innerHTML = this.CreateParams.Html;


                // link to tripous object
                tp.SetObject(el, this);

                // Parent
                if (tp.IsEmpty(el.parentNode)) {
                    if (tp.IsElement(this.CreateParams.Parent)) {
                        this.CreateParams.Parent.appendChild(el);
                    } else if (this.CreateParams.Parent instanceof tp.tpElement) {
                        this.CreateParams.Parent.Handle.appendChild(el);
                    }
                }



                this.OnHandleCreated();                             // notification

                this.InitializeFields();
                this.OnFieldsInitialized();                         // notification

                this.ProcessCreateParams(this.CreateParams);

                tp.AddClass(el, 'tp-Object');

                this.OnCreateParamsProcessed();                     // notification

                this.OnInitializationCompleted();                   // notification
            }
        }
    }
    /**
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    */
    Dispose() {
        if (this.fIsDisposed === false && tp.IsElement(this.fHandle)) {
            this.IsElementResizeListener = false;
            this.IsScreenResizeListener = false;

            var el = this.fHandle;
            tp.SetObject(this.Handle, null);
            this.fHandle = null;
            if (tp.IsElement(el.parentNode)) {
                el.parentNode.removeChild(el);
            }

            this.fIsDisposed = true;
        }
    }

    /** Returns and array of property names the ProcessCreateParams() should NOT set 
     @returns {string[]} Returns and array of property names the ProcessCreateParams() should NOT set
     */
    GetAvoidParams() {
        return ['Id', 'Name', 'Handle', 'Parent', 'Html', 'CssClasses', 'CssText'];
    }
    /**
     * Processes the this.CreateParams by applying its properties to the properties of this instance <br />
     * CAUTION: For properties to be processed here should belong to the prototype of the derived class, <br />
     * e.g. MyDerivedClass.prototype.MyProperty, <br />
     * and NOT in the class statement itself. <br />
     * The reason is that this method is called from inside the super constructor and the construction cycle has not yet completed. <br />
     * Only properties belonging to the super class and to MyDerivedClass.prototype.MyProperty are visible at this point. <br />
     * For a property in a derived class to be visible here must have a getter and a setter in the derived class, i.e. not to be just a field but a "real" property. <br />
     * NOTE: If you ask me, I say it is insane.
     * @param {object} [o=null] - Optional. The create params object to processs.
     */
    ProcessCreateParams(o = null) {
        this.CreateParams = o || {};

        let AvoidParams = this.GetAvoidParams();
        let Value; 

        for (var Prop in this.CreateParams) {
            Value = this.CreateParams[Prop];
            this.ProcessCreateParam(Prop, Value, AvoidParams);
        }
    }
    /**
     * Processes an entry of the this.CreateParams.
     * CAUTION: For properties to be processed here should belong to the prototype of the derived class, <br />
     * e.g. MyDerivedClass.prototype.MyProperty, <br />
     * and NOT in the class statement itself. <br />
     * The reason is that this method is called from inside the super constructor and the construction cycle has not yet completed. <br />
     * Only properties belonging to the super class and to MyDerivedClass.prototype.MyProperty are visible at this point. <br />
     * For a property in a derived class to be visible here must have a getter and a setter in the derived class, i.e. not to be just a field but a "real" property. <br />
     * NOTE: If you ask me, I say it is insane.
     * @param {string} Name The name of the property in this.CreateParams
     * @param {any} Value The value of the property in this.CreateParams
     * @param {string[]} AvoidParams A string array of property names the ProcessCreateParam() should NOT set
     */
    ProcessCreateParam(Name, Value, AvoidParams) {
        if (tp.HasWritableProperty(this, Name) && !tp.IsFunction(Value)) {
            let Allowed = AvoidParams.indexOf(Name) === -1;
            if (Allowed) {
                this[Name] = Value;
            }
        }
    }
    /**
    Returns a property value from this.CreateParams in a safe manner.
    @param {string} PropName The name of the property in this.CreateParams.
    @param {any} [Default=null] - Optional. The default value to return if the property does not exist
    @returns {any} Returns a property value from this.CreateParams in a safe manner.
    */
    GetCreateParamsOption(PropName, Default = null) {
        Default = Default || null;
        if (!tp.IsEmpty(this.CreateParams) && PropName in this.CreateParams) {
            return this.CreateParams[PropName];
        }
        return Default;
    }
 

    /**
    Adds a listener to an event of this instance and returns the listener object. 
    The caller specifies the event name, a callback function and a context for the callback.  
    When the event occurs then this instance invokes the callback passing a single parameter of type tp.EventArgs.
    @example
    MyObject.On('OnSomething', function(Args: tp.EventArgs) {
        //
    });

    // or
    MyObject.On('OnSomething', this.HandlerFunc, this);

    // or, if the caller is a tp.tpObject
    MyObject.On('OnSomething', this.FuncBind(this.HandlerFunc));

    @param {string} EventName - Can be any event name this instance supports.
    @param {function} Func - A function(Args: tp.EventArgs): void to call back when the event occurs.
    @param {object} [Context=null] - Optional. The context (this) to use when calling the callback function.
    @returns {tp.Listener} Returns the created listener object
    */
    On(EventName, Func, Context = null) {
        let S = tp.Events.ToDom(EventName);             // could be a tp.Events.XXXX constant
        if (!tp.IsSameText(S, tp.Events.Unknown)) {
            this.HookEvent(S);                          // it is a dom event
        }

        return super.On(EventName, Func, Context);
    }
    /**
    Subscribes this instance as a listener to the handle of this instance for a specified event.  
    After that subscription, this instance can handle the event in the OnAnyDOMEvent() handler,
    and, if EventsEnabled is true, in the OnAnyEvent() handler too.
    @param {string} EventName - Could be a DOM event name, e.g. click or keydown, or one of the tp.Events constants.
    */
    HookEvent(EventName) {
        if (!this.fHookedEvents) {
            this.fHookedEvents = [];
        }

        var S = tp.Events.ToDom(EventName);                     // could be a tp.Events.XXXX constant
        EventName = S !== tp.Events.Unknown ? S : EventName;
        if (!tp.ListContainsText(this.fHookedEvents, EventName)) {
            tp.On(this.Handle, EventName, this, true);
            this.fHookedEvents.push(EventName);
        }
    }
    /**
    Subscribes this instance as a listener to the handle of this instance for one or more groups of events. 
    After that subscription, this instance can handle the event(s) in the OnAnyDOMEvent() handler,
    and, if EventsEnabled is true, in the OnAnyEvent() handler too.
    @param {number} EventGroups - The groups of events. A bit-field number constructed using the tp.EventGroup constants.
    */
    HookEventGroups(EventGroups) {
        if (this.Handle) {
            var M = EventGroups;

            if (tp.Bf.In(tp.EventGroup.Click, M)) {
                this.HookEvent('click');
                this.HookEvent('dblclick');
            }
            if (tp.Bf.In(tp.EventGroup.Mouse, M)) {
                this.HookEvent('mousedown');
                this.HookEvent('mouseup');

                this.HookEvent('mouseover');
                this.HookEvent('mousemove');
                this.HookEvent('mouseout');
            }
            if (tp.Bf.In(tp.EventGroup.Keyboard, M)) {
                this.HookEvent('keydown');
                this.HookEvent('keypress');
                this.HookEvent('keyup');
            }
            if (tp.Bf.In(tp.EventGroup.Text, M)) {
                this.HookEvent('oninput');          //oninput = TextChanged
                this.HookEvent('onselect');         //onselect  = TextSelected
            }
            if (tp.Bf.In(tp.EventGroup.Scroll, M)) {
                this.HookEvent('scroll');
                this.HookEvent('mousewheel');
                this.HookEvent('DOMMouseScroll');   // mousewheel for FF
            }
            if (tp.Bf.In(tp.EventGroup.ContextMenu, M)) {
                this.HookEvent('contextmenu');
            }
            //if (tp.Bf.In(tp.EventGroup.Size, M)) {
            //    this.HookEvent('resize');
            //}
            if (tp.Bf.In(tp.EventGroup.Focus, M)) {
                this.HookEvent('activate');         // IE
                this.HookEvent('DOMActivate');      // the others
                this.HookEvent('focus');
                this.HookEvent('blur');             // lost focus
            }
            if (tp.Bf.In(tp.EventGroup.Change, M)) {
                this.HookEvent('change');
            }
            if (tp.Bf.In(tp.EventGroup.DragSource, M)) {
                this.HookEvent('dragstart');
                this.HookEvent('drag');
                this.HookEvent('dragend');
            }
            if (tp.Bf.In(tp.EventGroup.DropTarget, M)) {
                this.HookEvent('dragenter');
                this.HookEvent('dragover');
                this.HookEvent('dragleave');
                this.HookEvent('drop');
            }
            if (tp.Bf.In(tp.EventGroup.Clipboard, M)) {
                this.HookEvent('copy');
                this.HookEvent('cut');
                this.HookEvent('paste');
            }
        }
    }

    /**
    Implementation of the DOM EventListener interface.  
    For handling all DOM element events. Either when this is a DOM element and the sender (target) of the event is this.Handle
    or when the sender (target) of the event is any other object and listener is this instance.
    @see {@ling http://www.w3.org/TR/DOM-Level-2-Events/events.html#Events-EventListener|specification}
    @param {Event} e The event to handle
     */
    handleEvent(e) {
        this.OnAnyDOMEvent(e);

 
        if (this.EventsEnabled === true) {
            let EventName = tp.Events.ToTripous(e.type);
            EventName = tp.IsSameText(EventName, tp.Events.Unknown) ? e.type : EventName;

            let Args = new tp.EventArgs(EventName, this, e);
            this.OnAnyEvent(Args);
        }
    }

    /**
    Propagates the size change of this instance to its child controls and siblings
    */
    PropagateSize() {
        if (this.Handle) {
            this.ResizeControls();
            var i, ln, o;
            var List = this.GetChildren();
            for (i = 0, ln = List.length; i < ln; i++) {
                o = tp.GetObject(List[i]);
                if (o instanceof tp.tpElement) {
                    o.ParentSizeChanged();
                }
            }

            if (this.InformSiblings && this.ParentHandle && this.ParentHandle instanceof HTMLElement) {
                List = tp.ChildHTMLElements(this.ParentHandle);

                for (i = 0, ln = List.length; i < ln; i++) {
                    o = tp.GetObject(List[i]);
                    if (o !== this && o instanceof tp.tpElement) {
                        o.SiblingSizeChanged(this);
                    }
                }
            }
        }
    }
    /**
    For inheritors to resize child controls
    */
    ResizeControls() { }

    /**
    Called becacuse the size of its parent is changed
    */
    ParentSizeChanged() {
        this.UpdateSize();
    }
    /**
    Called becacuse the size of one of its siblings is changed
    @param {tp.tpElement} Sibling A sibling to this instance
    */
    SiblingSizeChanged(Sibling) {
        this.UpdateSize();
    }
    /**
     It may be called in any case where the size should be recalculated and applied. 
     Inheritors should calculate and apply their new size, according to changes
     */
    UpdateSize() {
        this.PropagateSize();
    }

    /**
    Sets multiple properties of the style property of this element, at once, based on a specified object
    @example
    // set multiple style properties at once
    tp.SetStyle({'width': '100px', height: '100px', 'background-color', 'yellow', backgroundColor: 'red' });   
    @param   {object}  Style  - An object with key/value pairs where key may be a string
    */
    SetStyle(Style) {
        tp.SetStyle(this.Handle, Style);
    }
    /**
    Gets or sets the value of a style property   
    NOTE: For the function to act as a get, just let the last argument unspecified.  
    @example
    // get
    var v = tp.StyleProp(el, 'width')
    @example
    // set a single style property
    tp.StyleProp(el, 'width', '100px');
 
    @param  {string} Prop Denotes the property name 
    @param  {object} [v=null] The value of the property to set.
    @returns {any} When acts as a get then it returns the value of the property. When acts as a set, it always returns null.
    */
    StyleProp(Prop, v = null) {
        return tp.StyleProp(this.Handle, Prop, v);
    }

    /**
    Sets multiple mutliple attributes of an element, at once, based on a specified object
    @example
    // set multiple attributes at once
    tp.SetAttributes({ id: 'img0', src: 'image.jpg' );   
    @param {object} o - An object with key/value pairs where key may be a string
    */
    SetAttributes(o) {
        tp.SetAttributes(this.Handle, o);
    }
    /**
    Gets or sets the value of an attribute  
    NOTE: For the function to act as a get, just let the last argument unspecified. 
    @example
    // get
    var v = tp.Attribute(el, 'id');
    @example
    // set a single attribute
    tp.Attribute(el, 'id', 'div0');
    @param {string} Name Denotes the attribute name.  
    @param {object} [v=null] The value of the attribute to set.
    @returns {any} When acts as a get then it returns the value of the attribute. When acts as a set, it always returns null.
     */
    Attribute(Name, v = nul) {
        return tp.Attribute(this.Handle, Name, v);
    }
    /**
    Removes an attribute from an element.
    @param {string} Name Denotes the attribute name.
    */
    RemoveAttribute(Name) {
        tp.RemoveAttribute(this.Handle, Name);
    }

    /**
    Gets or sets the value of a data-* attribute or sets the values of multiple data-* attributes. 
    NOTE: For the function to act as a get, just let the last argument unspecified. 
    NOTE: When setting multiple items the last argument is not used.
    @example
    // get
    var v = this.Data('field');
    
    @example
    // set a single data-* attribute
    this.Data('field', 'UserName');
    
    // set multiple data-* attributes at once
    this.Data({'field': 'UserName', level: 'guest', points: '456', 'color', 'yellow', index: '0' });
     
    @param {string|object} o When string denotes the item name. Else it's an object with key/value pairs where key may be a string
    @param {object} [v=null]  The value of the item to set.
    @returns {string} When acts as a get then it returns the value of the data-* attribute. When acts as a set, it always returns empty string.
     */
    Data(o, v = null) {
        return tp.Data(this.Handle, o, v);
    }
    /**
    Gets or sets the value of a data-role attribute of an element 
    NOTE: For the function to act as a get, just let the last argument unspecified. 
    @param {string} [v=null] The value to set.
    @returns {string} When acts as a get then it returns the value of the data-role attribute. When acts as a set, it always returns empty string.
    */
    Role(v = null) {
        return tp.Role(this.Handle, v);
    }

    /**
    Returns true if an element has a specified css class.
    @param {string} Name - The css class name
    @returns {boolean} Returns true if an element has a specified css class.
    */
    HasClass(Name) {
        return tp.HasClass(this.Handle, Name);
    }
    /**
    Adds a specified css class to an element, if not already there.
    @param {string} Name - The css class name
    */
    AddClass(Name) {
        tp.AddClass(this.Handle, Name);
    }
    /**
    Removes a specified css class from an element.
    @param {string} Name - The css class name
    */
    RemoveClass(Name) {
        tp.RemoveClass(this.Handle, Name);
    }
    /**
    Toggles a specified css class from an element, i.e. adds the class if not there, removes the class if there.
    @param {string} Name - The css class name
    */
    ToggleClass(Name) {
        tp.ToggleClass(this.Handle, Name);
    }
    /**
    Adds one or more css classes to an element.
    @param {...Names} Names - Rest parameter. One or more css class names.
    */
    AddClasses(...Names) {
        tp.AddClasses(this.Handle, Names.join(' '));
    }
    /**
    Removes one or more css classes from an element.
    @param {...Names} Names - Rest parameter. One or more css class names.
    */
    RemoveClasses(...Names) {
        tp.RemoveClasses(this.Handle, Names.join(' '));
    }



    /* z-index */
    /**
    Brings this element in front of all of its siblings (child elements in the same parent element). 
    Returns the z-index of the element after the placement.
    @returns {number} Returns the z-index of the element after the placement.
    */
    BringToFront() {
        if (this.Handle) {
            return tp.BringToFront(this.Handle);
        }

        return -1;
    }
    /**
    Sends this element to back of all of its siblings (child elements in the same parent element). 
    Returns the z-index of the element after the placement.
    @returns {number} Returns the z-index of the element after the placement.
    */
    SendToBack() {
        if (this.Handle) {
            return tp.SendToBack(this.Handle);
        }

        return -1;
    }

    /* miscs */
    /**
    Makes the handle of this instance the focused element
    */
    Focus() {
        if (this.Handle)
            this.Handle.focus();
    }
    /**
    Generates a click event
    */
    Click() {
        if (this.Handle)
            this.Handle.click();
    }


    /* direct or nested tp.tpElement children */
    /**
    Returns an array with all tp.tpElement objects existing on direct or nested child DOM elements, of the handle of this instance. 
    When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class and a new property, named tpObject
    is created and attached to the DOM element object, pointing to the Tripous object.
    @returns {tp.tpElement[]} Returns an array with tp.Element objects constructed up on elements of a parent element
    */
    GetControls() {
        return this.Handle ? tp.GetScriptObjects(this.Handle) : [];
    }
    /**
    Returns a direct or nested child tp.tpElement having a specified css class, if any, else null
    @param {string} v - The css class
    @returns {tp.tpElement} The found tp.tpElement or null
    */
    FindControlByCssClass(v) {
        var List = this.GetControls();
        for (var i = 0, ln = List.length; i < ln; i++) {
            if (List[i].HasClass(v))
                return List[i];
        }
        return null;
    }
    /**
    Returns a direct or nested child tp.tpElement having a id, if any, else null
    @param {string} v - The id
    @returns {tp.tpElement} The found tp.tpElement or null
    */
    FindControlById(v) {
        var List = this.GetControls();
        for (var i = 0, ln = List.length; i < ln; i++) {
            if (tp.IsSameText(List[i].Id, v))
                return List[i];
        }
        return null;
    }
    /**
      Finds and returns a direct or nested child tp.Elemement contained by this instance, by a property. If not found, then null is returned
      @param {string} PropName The property to match
      @param {any} PropValue The value to match
      @returns {tp.tpElement} Returns the tp.tpElement or null.
     */
    FindControlByProp(PropName, PropValue) {
        return tp.FindControlByProp(PropName, PropValue, this.Handle);
    }

    /* direct tp.tpElement children */
    /**
    Returns the associated tp.tpElement of a direct child  (DOM element) found at a specified index, if any, else null
    @param {number} Index The index to use.
    @returns {tp.tpElement} Returns the associated tp.tpElement of a direct child  (DOM element) found at a specified index, if any, else null
    */
    ControlAt(Index) {
        let el = this.ChildAt(Index);
        let o = tp.GetObject(el);
        return o instanceof tp.tpElement ? o : null;
    }
    /**
    Adds a child tp.tpElement to this instance and returns the child.
    @param {string|tp.tpElement} Child - Could be any of the tp.tpElement.StandardNodeTypes, i.e. div, span, etc. or a tp.tpElement
    @returns {tp.tpElement} Returns the newly added tp.tpElement.
    */
    AddControl(Child) {
        if (this.Handle) {

            if (tp.IsString(Child) && tp.ListContainsText(tp.tpElement.StandardNodeTypes, Child)) {
                var el = this.Handle.ownerDocument.createElement(Child);
                Child = new tp.tpElement(el);
            }

            if (Child instanceof tp.tpElement && Child.Handle) {
                this.Handle.appendChild(Child.Handle);
            }
        }

        return Child instanceof tp.tpElement ? Child : null;
    }
    /**
    Inserts a child tp.tpElement to this instance, at a specified child position, and returns the child.
    @param {number|Node} IndexOrNode - A number indicating the child position or an already existing child element
    @param {string|tp.tpElement} Child - Could be any of the tp.tpElement.StandardNodeTypes, i.e. div, span, etc. or a tp.tpElement
    @returns {tp.tpElement} Returns the newly inserted tp.tpElement.
    */
    InsertControl(IndexOrNode, Child) {

        if (this.Handle) {
            if (tp.IsString(Child) && tp.ListContainsText(tp.tpElement.StandardNodeTypes, Child)) {
                var el = this.Handle.ownerDocument.createElement(Child);
                Child = new tp.tpElement(el);
            }

            if (Child instanceof tp.tpElement && Child.Handle) {
                var beforeElement = null;

                if (tp.IsNumber(IndexOrNode) && IndexOrNode >= 0) {
                    if (IndexOrNode === 0 && this.Count === 0) {
                        this.Handle.appendChild(Child.Handle);
                        return Child;
                    }

                    beforeElement = this.Handle.childNodes[IndexOrNode];
                } else if (IndexOrNode instanceof HTMLElement) {
                    beforeElement = IndexOrNode;
                }

                if (tp.IsElement(beforeElement) && this.IsChild(beforeElement)) {
                    this.Handle.insertBefore(Child.Handle, beforeElement);
                }
            }
        }

        return Child instanceof tp.tpElement ? Child : null;
    }
    /**
    Removes a child
    @param {tp.tpElement} Child The child to remove
    */
    RemoveControl(Child) {
        if (this.Handle) {

            let el = null;

            if (tp.IsString(Child))
                el = tp.Select(Child);

            if (Child instanceof tp.tpElement)
                el = Child.Handle;

            if (el instanceof HTMLElement && el.parentNode === this.Handle) {
                this.Handle.removeChild(el);
            }
        }

    }

    /* direct DOM element children */
    /**
    Returns an array with the direct HTMLElement children of this element
    @returns {HTMLElement[]} Returns an array with the direct HTMLElement children of this element
    */
    GetChildren() {
        return this.Handle ? tp.ChildHTMLElements(this.Handle) : [];
    }
    /**
    Returns a direct child (DOM element) found at a specified index, if any, else null
    @param {number} Index The index to use
    @returns {HTMLElement} Returns a direct child (DOM element) found at a specified index, if any, else null
    */
    ChildAt(Index) {
        let List = this.GetChildren();
        if (tp.InRange(List, Index)) {
            return List[Index];
        }

        return null;
    }
    /**
    Finds and returns the index of a direct child, if exists, else -1.
    @param {string|HTMLElement|tp.tpElement} Child The child to find the index
    @returns {number} Returns the index of a direct child, if exists, else -1.
    */
    IndexOfChild(Child) {
        if (this.Handle) {
            let el = null;

            if (Child instanceof HTMLElement) {
                el = Child;
            } else if (tp.IsString(Child)) {
                el = tp.Select(Child);
            } else if (Child instanceof tp.tpElement) {
                el = Child.Handle;
            }

            if (el) {
                let List = this.GetChildren();
                for (let i = 0, ln = List.length; i < ln; i++) {
                    if (el === List[i])
                        return i;
                }
            }
        }

        return -1;
    }
    /**
     Retuns true if a specified element is a direct child of this element.
     @param {string|HTMLElement|tp.tpElement} Child The child to check
     @return {boolean} Retuns true if a specified element is a direct child of this element.
     */
    IsChild(Child) { return this.IndexOfChild(Child) !== -1; }
    /**
    Adds a child HTMLElement to this instance and returns the child.
    @param {string|HTMLElement} Child - Could be any of the tp.tpElement.StandardNodeTypes, i.e. div, span, etc. or a HTMLElement
    @returns {HTMLElement} Returns the newly added child HTMLElement
    */
    AddChild(Child) {
        return this.InsertChild(this.Count, Child);
    }
    /**
    Inserts a child HTMLElement to this instance, at a specified child position, and returns the child.
    @param {number|Node} IndexOrNode - A number indicating the child position or an already existing child element
    @param {string|HTMLElement} Child - Could be any of the tp.tpElement.StandardNodeTypes, i.e. div, span, etc. or a tp.tpElement
    @returns {HTMLElement} Returns the newly added child HTMLElement
    */
    InsertChild(IndexOrNode, Child) {
        if (this.Handle) {

            if (tp.IsString(Child) && tp.ListContainsText(tp.tpElement.StandardNodeTypes, Child)) {
                Child = this.Handle.ownerDocument.createElement(Child);
            }



            if (tp.IsHTMLElement(Child)) {

                let Count = this.Count;
                let ChildrenList = this.GetChildren();
                let beforeElement = null;

                if (Count === 0) {
                    this.Handle.appendChild(Child);
                    return Child;
                }                

                if (tp.IsNumber(IndexOrNode)) {
                    if (IndexOrNode >= Count || IndexOrNode < 0) {
                        this.Handle.appendChild(Child);
                        return Child;
                    }
                    else if (IndexOrNode >= 0 && IndexOrNode <= ChildrenList.length - 1)  
                            beforeElement = ChildrenList[IndexOrNode];
                }
                else if (tp.IsHTMLElement(IndexOrNode) && ChildrenList.indexOf(IndexOrNode) !== -1) {
                    beforeElement = IndexOrNode;
                }

                if (beforeElement) {
                    this.Handle.insertBefore(Child, beforeElement);
                    return Child;
                }
            }
 
        }

        return null;
    }

    /**
    Removes a direct child
    @param {string|HTMLElement} Child The child to remove
    */
    RemoveChild(Child) {
        let Index = this.IndexOfChild(Child);
        if (Index !== -1) {
            let List = this.GetChildren();
            this.Handle.removeChild(List[Index]);
        }
    }
    /**
    Removes a direct child by index
    @param {number} Index The index of the child to remove
    */
    RemoveChildAt(Index) {
        let Child = this.ChildAt(Index);
        if (Child)
            this.Handle.removeChild(Child);
    }
    /**
    Removes all children
   */
    ClearChildren() {
        if (this.Handle) {
            tp.RemoveChildren(this.Handle);
        }
    }

    /* miscs */
    /**
    Adds a div to this instance, creates a tp.tpElement on that div and returns the tp.tpElement
    @returns {tp.tpElement} Returns a tp.tpElement.
    */
    AddDiv() { return this.AddControl('div'); }
    /**
    Adds a span to this instance, creates a tp.tpElement on that span and returns the tp.tpElement
    @returns {tp.tpElement} Returns a tp.tpElement.
    */
    AddSpan() { return this.AddControl('span'); }



    /**
    Attaches this element as a direct child to document.body
    */
    ToBody() {
        if (this.Handle) {
            this.Handle.ownerDocument.body.appendChild(this.Handle);
        }
    }

};

// ------------------------------------------------------------
/* treat them as read-only  class fields (static) */

/* protected */
/** The creation parameters.
CreateParams can be passed to the constructor, or defined in the html markup as
    data-setup = "{ Prop0: Value, PropN: Value}"
or both.
Finally the two CreateParams are merged into one and the values are passed to the properties of this instance, as long as property names match.
@type {object|tp.CreateParams}
*/
tp.tpElement.prototype.CreateParams = {};

/** The css display property. By default, either the css defined or block 
 * @type {string}
 * */
tp.tpElement.prototype.fDisplayType = '';
/** The element node type, e.g. div, etc.
 * @type {string}
 * */
tp.tpElement.prototype.fElementType = '';
/** The element node sub-type of input elements, e.g. text, date, etc.
 * @type {string}
 * */
tp.tpElement.prototype.fElementSubType = '';
/** Default css classes.
 * @type {string}
 * */
tp.tpElement.prototype.fDefaultCssClasses = '';
/** When true the constructor does NOT create the handle (dom element)
 * @type {boolean}
 * */
tp.tpElement.prototype.fDeferHandleCreation = '';
/** When true then the element gets an id attribute value automatically.
 * @type {boolean}
 * */
tp.tpElement.prototype.fAutoId = false;

// ------------------------------------------------------------
/* instance fields */
/** The {@link HTMLElement} this instance represents. */
tp.tpElement.prototype.fHandle = null;
/** True when this element is enabled.
 * @type {boolean}
 * */
tp.tpElement.prototype.fEnabled = true;                     
/** True when the handle of this instance comes from the html markup, i.e. the dom element exists in markup and it is not dynamically created by code.
 * @type {boolean}
 * */
tp.tpElement.prototype.fFromMarkup = false;
/** Any tp.Element instances created from markup and having this instance as container (e.g. when this is an accordion item or a tab page, panel etc.)
 * @type {array}
 * */
tp.tpElement.prototype.fChildMarkupControls = [];
/** A list of DOM events this instance handles. 
 * Could be DOM event names, e.g. click or keydown, or one of the tp.Events constants.
 * @type {string[]|null}
 * */
tp.tpElement.prototype.fHookedEvents = null;
/** Detects size changes in an HTMLElement and sends notifications to a listener function.
 * @type {tp.ResizeDetector}
 * */
tp.tpElement.prototype.fResizeDetector = null;
/** The instance of a listener or null.
 * @type {tp.Listener}
 * */
tp.tpElement.prototype.fScreenResizeListener = null;         

/* fields */
/** The {@link Document} this element is part of.
 * @type {Document}
 */
tp.tpElement.prototype.fDocument = tp.Doc;
/** When true then this instance informs siblings when its size changes 
 * @type {boolean}
 * */
tp.tpElement.prototype.InformSiblings = false;               
/** True after the Dispose() is called.
* @type {boolean}
* */
tp.tpElement.prototype.fIsDisposed = false;

/** HTMLElement - label element, just before a control, with label text.
 * NOTE: It is an HTMLSpanElement in checkbox
 * @type {HTMLElement}
 * */
tp.tpElement.prototype.elText = null;                      
/** HTMLSpanElement - span element, right after a control, with a required mark
 * @type {HTMLSpanElement}
 * */
tp.tpElement.prototype.elRequiredMark = null;            

/** Node type names array, used internally */
tp.tpElement.StandardNodeTypes = [
    'main',
    'aside',
    'article',
    'section',
    'header',
    'footer',
    'nav',

    'iframe',

    'div',
    'span',
    'fieldset',

    'a',
    'form',
    'table',

    'label',
    'button',
    'input',
    'select',
    'option',
    'ul',
    'ol',
    'li',
    'img',
    'textarea',
    'progress',
    'video'
];


/**
Creates, sets-up and returns a textarea tp.tpElement
@param {HTMLElement|tp.tpElement} [Parent=null] - Optional.
@returns {tp.tpElement} Returns a tp.tpElement.
*/
tp.tpElement.CreateMemo = function (Parent = null) {
    let CP = new tp.CreateParams();
    CP.Parent = Parent;
    CP.CssClasses = 'tp-Memo';

    let Result = new tp.tpElement('textarea', CP);

    Result.SetAttributes({
        cols: 10,
        rows: 5
    });
    Result.SetStyle({
        width: 'calc(100% - 6px)',
        height: 'calc(100% - 6px)',
        fontFamily: 'monospace',
        'white-space': 'pre',
        'overflow': 'auto',
        'border': 'none',
        'outline': 'none',
        'resize': 'none'
    });

    return Result;
};


// tp.tpElement association ---------------------------------------------------------------
/**
Returns the Tripous script object associated to a DOM element, if any or null.  
NOTE: When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class.
Also a new property, named tpObject, is created and attached to the DOM element object, pointing to the Tripous object.
@param {string|Node} el - The element to get the associated tripous script object from.
@returns {tp.tpElement} Returns the Tripous script object associated to a DOM element, if any or null.
*/
tp.GetScriptObject = function (el) {
    let o = tp.GetObject(el);
    return o instanceof tp.tpElement ? o : null;
};
/**
Returns true if a specified DOM element is associated to a tp.tpElement.
NOTE: When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class.
Also a new property, named tpObject, is created and attached to the DOM element object, pointing to the Tripous object.
@param {string|Node} el The element to check
@returns {boolean} Returns true if a specified DOM element is associated to a tp.tpElement
*/
tp.HasScriptObject = function (el) { return tp.GetScriptObject(el) !== null; };
/**
Returns an array with all tp.Element objects constructed/existing up on child DOM elements of a specified parent element or the entire document.  
NOTE: When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class.
Also a new property, named tpObject, is created and attached to the DOM element object, pointing to the Tripous object.
@param {string|Node} ParentElementOrSelector - String or Element. Defaults to document. The container of controls. If null/undefined/empty the document is used
@returns {tp.tpElement[]} Returns an array with tp.Element objects constructed up on elements of a parent element
*/
tp.GetScriptObjects = function (ParentElementOrSelector) {
    let List = tp.GetObjects(ParentElementOrSelector);
    let Result = [];        // tp.tpElement[]

    for (let i = 0, ln = List.length; i < ln; i++) {
        if (List[i] instanceof tp.tpElement)
            Result.push(List[i]);
    }

    return Result;
};


/**
 Finds and returns a tp.Elemement contained by a container, by Id. If not found, then null is returned
 @param {string} Id - The Id to match
 @param {string|Node} [ParentElementOrSelector=null] - The container of controls. If null/undefined/empty the document is used.
 @returns {tp.tpElement} Returns the tp.tpElement or null.
 */
tp.FindControlById = function (Id, ParentElementOrSelector = null) {
    ParentElementOrSelector = ParentElementOrSelector || tp.Doc.body;
    var List = tp.GetScriptObjects(ParentElementOrSelector);

    for (var i = 0, ln = List.length; i < ln; i++) {
        if (tp.IsSameText(List[i].Id, Id))
            return List[i];
    }
    return null;
};
/**
 Finds and return a tp.Elemement contained by a container, by a property. If not found, then null is returned 
 @param {string} PropName - The property to match
 @param {any} PropValue - The value to match
 @param {string|Node} [ParentElementOrSelector=null] - The container of controls. If null/undefined/empty the document is used.
 @returns {tp.tpElement} Returns the tp.tpElement or null.
 */
tp.FindControlByProp = function (PropName, PropValue, ParentElementOrSelector = null) {
    ParentElementOrSelector = ParentElementOrSelector || tp.Doc.body;
    var List = tp.GetScriptObjects(ParentElementOrSelector);

    for (var i = 0, ln = List.length; i < ln; i++) {
        if (List[i][PropName] === PropValue)
            return List[i];
    }
    return null;
};


// __tpInfo association ---------------------------------------------------------------
/**
In some cases Tripous script defines a __tpInfo property to some DOM elements and stores some additional information to it.
This function returns that associated property.
@param {string|Node} el - The element to get the associated __tpInfo object from.
@param {string} [InfoName='__tpInfo'] Optional. The name of the property to operate on. By default is '__tpInfo'
@returns {object} Returns the associated __tpInfo object from a DOM element, if any or null.
*/
tp.GetElementInfo = function (el, InfoName = '__tpInfo') {
    if (tp.IsString(el))
        el = tp.Select(el);

    if (tp.IsElement(el)) {
        if (InfoName in el)
            return el[InfoName];
    }

    return null;
};
/**
In some cases Tripous script defines a __tpInfo property to some DOM elements and stores some additional information to it.
This function creates a __tpInfo property to a DOM element, if not already there, and assigns the specified information to it.
@param {string|Node} el - The element to create/assign the __tpInfo object to.
@param {object}  v - The __tpInfo data.
@param {string} [InfoName='__tpInfo'] Optional. The name of the property to operate on. By default is '__tpInfo'
*/
tp.SetElementInfo = function (el, v, InfoName = '__tpInfo') {
    if (tp.IsString(el))
        el = tp.Select(el);

    if (tp.IsElement(el))
        el[InfoName] = v;
};
/**
In some cases Tripous script defines a __tpInfo property to some DOM elements and stores some additional information to it.
This function returns true if a specified DOM element has defined a __tpInfo property.
@param {string|Node} el - The element to create/assign the __tpInfo object to.
@param {string} [InfoName='__tpInfo'] Optional. The name of the property to operate on. By default is '__tpInfo'
@returns {boolean} Returns true if a specified DOM element has defined a __tpInfo property..
*/
tp.HasElementInfo = function (el, InfoName = '__tpInfo') { return tp.GetElementInfo(el, InfoName) !== null; };



// Command association -----------------------------------------------------------------


/** Returns a command name out of a specified value, if any, else empty string.
 * @param {HTMLElement|Event|string|tp.EventArgs} v The value to get the command from. Could be:
 * 1. HTMLElement
 * 2. Event
 * 3. a string selector
 * 4. tp.EventArgs
 * @returns {string} Returns a command name out of a specified value, if any, else empty string.
 */
tp.GetCommand = function (v) {
    let Args = null;            // tp.EventArgs
    let e = null;               // Event
    let el = null;              // HTMLElement
    let i, ln,
        Result = '';


    var FindCommand = function (Instance) {
        if (!tp.IsEmpty(Instance) && 'Command' in Instance && !tp.IsBlank(Instance.Command))
            return Instance.Command;
        return '';
    };
    var FindContainerCommand = function (el, ElementClass) {
        var C = tp.GetContainerByClass(el, ElementClass);
        if (!tp.IsEmpty(C) && 'Command' in C && !tp.IsBlank(C['Command']))
            return C['Command'];
        return '';
    };

    if (v instanceof tp.EventArgs) {
        Args = v;
        e = Args.e;
    } else if (v instanceof Event) {
        e = v;
    } else if (v instanceof HTMLElement) {
        el = v;
    } else if (tp.IsString(v)) {
        el = tp.Select(v);
    }

    if (e instanceof Event && e.target instanceof HTMLElement) {
        el = e.target;
    }

    Result = FindCommand(Args);



    if (Args && !tp.IsBlank(Args.Command))
        Result = Args.Command;

    if (tp.IsBlank(Result) && Args && !tp.IsEmpty(Args.Sender) && 'Command' in Args.Sender && !tp.IsBlank(Args.Sender['Command'])) {
        Result = Args.Sender['Command'];
    }

    if (tp.IsBlank(Result) && Args && !tp.IsEmpty(Args.Button) && !tp.IsBlank(Args.Button.Command)) {
        Result = Args.Button.Command;
    }

    if (tp.IsBlank(Result) && Args && !tp.IsEmpty(Args.MenuItem) && !tp.IsBlank(Args.MenuItem.Command)) {
        Result = Args.MenuItem.Command;
    }

    if (tp.IsBlank(Result) && el) {
        Result = tp.Data(el, 'command');

        if (tp.IsBlank(Result) && tp.HasObject(el)) {
            let o = tp.GetObject(el);
            if ('Command' in o && !tp.IsBlank(o['Command'])) {
                Result = o['Command'];
            }
        }

        // search for a parent of type button or similar
        if (tp.IsBlank(Result)) {
            var ButtonClasses = tp.ButtonClasses;               // var ButtonClasses = [tp.ToolButton, tp.Button, tp.ButtonStripItem];
            for (i = 0, ln = ButtonClasses.length; i < ln; i++) {
                Result = FindContainerCommand(el, ButtonClasses[i]);
                if (!tp.IsBlank(Result)) {
                    break;
                }
            }
        }
    }

    return Result || '';
};



//#endregion

//---------------------------------------------------------------------------------------
// Element drag and resize, and resize detection
//---------------------------------------------------------------------------------------

//#region  tp.ResizeDetector
/** Detects size changes in an HTMLElement and sends notifications to a listener function.  <br />
 * Uses the {@link https://developer.mozilla.org/en-US/docs/Web/API/Resize_Observer_API|ResizeObserver} API.
 */
tp.ResizeDetector = class {

    /**
     * Constructor.
     * @param {string|HTMLElement} SelectorOrElement Required. The element to observe.
     * @param {function} OnResizeFunc Required. A call-back that accepting an object of type <c>{Width: boolean, Height: boolean}</c>.
     * @param {object} Context Optional.The context to be used when calling the call-back function 
     * @param {boolean} ImmediateStart Optional. When true, the default, the observation starts immediately. Otherwise a call to <c>Start()</c> is required.
     */
    constructor(SelectorOrElement, OnResizeFunc, Context = null, ImmediateStart = true) {
        this.Element = tp(SelectorOrElement);
        this.OnResizeFunc = OnResizeFunc;
        this.Context = Context;
        this.Observer = new ResizeObserver((Entries, Observer) => {
            this.ObserverCallback(Entries, Observer);
        });

        if (ImmediateStart === true) {
            this.Start();
        }
    }


    /** The observer
     * @type {ResizeObserver}
     */
    Observer = null;
    /** The element to observe
     * @type {HTMLElement}
     */
    Element = null;
    /** The call-back function to call on size changes
     * @type {function}
     */
    OnResizeFunc = null;
    /** The context to be used when calling the call-back function 
     * @type{object}
     */
    Context = null;
    /** Internal
     * @type {integer}
     */
    Width = 0;
    /** Internal
     * @type {integer}
     */
    Height = 0;

    /** Starts the observation. */
    Start() {
        this.Width = this.Element.offsetWidth;
        this.Height = this.Element.offsetHeight;

        let Options = {
            box: "border-box"
        };
        this.Observer.observe(this.Element, Options);
    }
    /** Stops the observation */
    Stop() {
        this.Observer.unobserve(this.Element);
    }
    /** The call-back called by the observer.
     * SEE: {@link https://developer.mozilla.org/en-US/docs/Web/API/ResizeObserver/ResizeObserver| ResizeObserver constructor}
     * @param {ResizeObserverEntry[]} Entries An array of ResizeObserverEntry objects that can be used to access the new dimensions of the element after each change.
     * @param {ResizeObserver} Observer A reference to the ResizeObserver itself
     */
    ObserverCallback(Entries, Observer) {
        if (Entries && Entries.length > 0) {
            /** @type {ResizeObserverEntry} */
            let Entry = Entries[0];
            if (Entry.borderBoxSize && Entry.borderBoxSize.length > 0) {
                let Size = Entry.borderBoxSize[0];
                let w = Size.inlineSize;
                let h = Size.blockSize;

                if (w !== this.Width || h !== this.Height) {

                    let ResizeInfo = {
                        Width: w !== this.Width,
                        Height: h !== this.Height
                    };

                    this.Width = w;
                    this.Height = h;

                    tp.Call(this.OnResizeFunc, this.Context, ResizeInfo);

                }
            }
        }

    }
};
//#endregion

//#region  tp.WrapObserver
/**
Observes a container and when its elements are wrapped it assigns proper css classes to those elements.
@class
*/
tp.WrapObserver = (function () {

    let Initialized = false;
    let Containers = [];            // HTMLElement[]
    let WrappedClass = '';          // css class specified by the initializer code
    let WrapFirstClass = '';        // css class specified by the initializer code, marks the first element
    let WrapLastClass = '';         // css class specified by the initializer code, marks the last element

    let ProcessContainer = function (Container) {
        var IsWrapped = false;
        var S = WrapFirstClass + ' ' + WrapLastClass;

        var el0, // HTMLElement,
            el1, // HTMLElement,
            el2, // HTMLElement,
            R,   // ClientRect,
            R2   // ClientRect
                = null;

        var i, ln,
            List = Container.children;

        for (i = 0, ln = List.length; i < ln; i++) {
            if (List[i] instanceof HTMLElement) {

                if (i === 0)
                    el0 = List[i];

                el1 = List[i];
                tp.RemoveClasses(el1, S);
                R = el1.getBoundingClientRect();

                if (R2 && R2.top < R.top) {
                    tp.AddClass(el2, WrapLastClass);
                    tp.AddClass(el1, WrapFirstClass);
                    if (i === List.length - 1)
                        tp.AddClass(el1, WrapLastClass);
                    IsWrapped = true;
                }

                el2 = el1;
                R2 = R;
            }

        }

        if (IsWrapped) {
            tp.AddClass(Container, WrappedClass);
            if (el0)
                tp.AddClass(el0, WrapFirstClass);
        } else {
            tp.RemoveClass(Container, WrappedClass);
            if (el0)
                tp.RemoveClass(el0, WrapFirstClass);
        }
    };
    let OnWindowResizeFunc = function (e /* Event */) {
        var i, ln;
        for (i = 0, ln = Containers.length; i < ln; i++) {
            ProcessContainer(Containers[i]);
        }
    };

    return {
        /**
         * Initializes this class
         * @param {string} Wrapped The Wrapped css class
         * @param {string} WrapFirst The WrapFirst css class
         * @param {string} WrapLast  The WrapLast css class
         * @memberof tp.WrapObserver
         * @static
         */
        Initialize: function (Wrapped, WrapFirst, WrapLast) {
            if (!Initialized) {
                Initialized = true;

                WrappedClass = Wrapped;
                WrapFirstClass = WrapFirst;
                WrapLastClass = WrapLast;

                window.addEventListener('resize', OnWindowResizeFunc, true);
            }
        },
        /**
         * Registers one or more container elements.
         * @param {...Element} elements The element or elements to register.
         * @memberof tp.WrapObserver
         * @static
         */
        AddContainer: function (...elements) {
            arguments.forEach((el) => {
                if (!tp.ListContains(Containers, el)) {
                    Containers.push(el);
                    ProcessContainer(el);
                }
            });
        },
        /**
         * Unregisters one or more container elements
         * @param {...Element} elements The element or elements to unregister.
         * @memberof tp.WrapObserver
         * @static
         */
        RemoveContainer: function (...elements) {
            arguments.forEach((el) => {
                tp.ListRemove(Containers, el);
            });
        }
    };

})();

//#endregion

//#region tp.DraggerMode

/** Enum-like class.  
 * Indicates the active operations of a dragger 
 @class
@enum {number}
 * */
tp.DraggerMode = {
    Drag: 1,
    Resize: 2,
    Both: 1 | 2
};
Object.freeze(tp.DraggerMode);
//#endregion

//#region tp.IDragContextListener
/**
A tp.DragContext listener (just for reference and the intellisense)
@interface
*/
tp.IDragContextListener = class {

    constructor() { }

    /**
    Called by the tp.DragContext. The listener returns true for a dragging operation to start. 
    The may called either on mouse-down, or on mouse-move while the mouse button is down.
    @param {MouseEvent} e A mouse event
    @returns {boolean} Returning true starts the dragging operation.
    */
    IsDragStart(e) { return false; }

    /**
    Called by the tp.DragContext.
    @param {MouseEvent} e A mouse event
    */
    DragStart(e) { }
    /**
    Called by the tp.DragContext.
    @param {MouseEvent} e A mouse event
    */
    DragMove(e) { }
    /**
    Called by the tp.DragContext.
    @param {MouseEvent} e A mouse event
    */
    DragEnd(e) { }
};
//#endregion

//#region tp.DragContext
/**
A drag (and resize) context
*/
tp.DragContext = class {
    /**
     * Constructor
     * @param {string|Element} ElementOrSelector The element of this context.
     * @param {tp.IDragContextListener} Listener The listener of this context.
     */
    constructor(ElementOrSelector, Listener) {
        this.fListener = Listener;
        this.fElement = tp.Select(ElementOrSelector);

        this.fIsMouseDown = false;
        this.fElement.addEventListener('mousedown', this);
        this.fElement.ownerDocument.addEventListener('mousemove', this, true);
        this.fElement.ownerDocument.addEventListener('mouseup', this, true);
    }

    /* properties */
    /**
    Returns true while dragging
    @type {boolean}
    */
    get Dragging() { return this.fDragging; }
    /**
    Returns mouse information regarding the last mouse event handled by this instance. Valid only while dragging.
    @type {tp.MouseInfo}
    */
    get MouseInfo() { return this.fMouseInfo; }
    /**
    Returns true after the Dispose() is called indicating that this instance is no longer usable.
    @type {boolean}
    */
    get IsDisposed() { return this.fIsDisposed; }

    /* private */
    /**
     * Event handler
     * @param {MouseEvent} e The mouse event
     */
    OnMouseDown(e) {
        if (tp.Mouse.IsLeft(e)) {

            if (tp.IsEmpty(this.fMouseInfo))
                this.fMouseInfo = new tp.MouseInfo(e);
            else
                this.fMouseInfo.Update(e);

            this.fIsMouseDown = true;
            this.fDragging = this.fListener.IsDragStart(e);

            if (this.Dragging) {
                this.fListener.DragStart(e);
            }

        }
    }
    /**
     * Event handler
     * @param {MouseEvent} e The mouse event
     */
    OnMouseMove(e) {
        if (this.fIsMouseDown) {
            this.fMouseInfo.Update(e);

            if (!this.Dragging) {
                this.fDragging = this.fListener.IsDragStart(e);

                if (this.Dragging) {
                    this.fListener.DragStart(e);
                }
            }

            if (this.Dragging) {
                this.fListener.DragMove(e);
            }
        }
    }
    /**
     * Event handler
     * @param {MouseEvent} e The mouse event
     */
    OnMouseUp(e) {
        this.fIsMouseDown = false;
        if (this.Dragging) {
            this.fMouseInfo.Update(e);
            this.fDragging = false;
            this.fListener.DragEnd(e);
        }
    }

    /* public */
    /**
     * Event handler
     * @param {Event} e The event
     */
    handleEvent(e) {
        if (tp.IsSameText('mousedown', e.type)) {
            this.OnMouseDown(e);
        } else if (tp.IsSameText('mousemove', e.type)) {
            this.OnMouseMove(e);
        } else if (tp.IsSameText('mouseup', e.type)) {
            this.OnMouseUp(e);
        }
    }
    /**
    Disposes off the system resources used by this instance. After this call this instance is no more usable.
    */
    Dispose() {
        if (this.fIsDisposed === false) {
            this.fElement.removeEventListener('mousedown', this);
            this.fElement = null;
            this.fIsDisposed = true;
        }
    }

};

tp.DragContext.prototype.fIsMouseDown = false;
tp.DragContext.prototype.fDragging = false;
tp.DragContext.prototype.fListener = null;              // : tp.IDragContextListener
tp.DragContext.prototype.fElement = null;               // Element
tp.DragContext.prototype.fMouseInfo = null;             // tp.MouseInfo 
tp.DragContext.prototype.fIsDisposed = false;
//#endregion

//#region  tp.Dragger
/**
Element mover and resizer  class. <br />
@example
let Dragger = new tp.Dragger(tp.DraggerMode.Both '.Box', '.Caption');
*/
tp.Dragger = class extends tp.tpObject {

    /**
    Constructor
    @param {tp.DraggerMode} Mode - Indicates the active operations of the dragger. A bit-field of tp.DraggerMode constants
    @param {string|Element} ElementOrSelector - The element to drag and/or resize
    @param {string|Element} [DragElementOrSelector=null] - Optional. The element to be used as the drag (move) handle, i.e. a header in a window. If not specified the main element is used.
    */
    constructor(Mode, ElementOrSelector, DragElementOrSelector = null) {
        super();

        this.fMode = Mode;

        let el = tp.Select(ElementOrSelector);
        this.fHandle = el;

        if (this.IsDraggable) {
            el = tp.Select(DragElementOrSelector);
            this.fDragHandle = el instanceof HTMLElement ? el : (this.IsDraggable ? this.Handle : null);
        }

        this.fOldCursor = tp.Mouse.Cursor;
        this.Active = true;
    }


    /* properties */
    /**
    Gets or sets a value indicationg whether this instance is active. Defaults to true.
    @type {boolean}
    */
    get Active() { return this.fActive; }
    set Active(v) { this.SetActive(v); }
    /**
    Indicates the active operations of a dragger. A bit-field of tp.DraggerMode constants
    @type {tp.DraggerMode}
    */
    get Mode() { return this.fMode; }
    /** 
     Returns true if this is a drag mode dragger
     @type {boolean}
    */
    get IsDraggable() { return tp.Bf.In(tp.DraggerMode.Drag, this.Mode); }
    /**
    Returns true if this is a resize mode dragger
    @type {boolean}
    */
    get IsResizable() { return tp.Bf.In(tp.DraggerMode.Resize, this.Mode); }
    /**
    Returns true while resizing
    @type {boolean}
    */
    get Resizing() { return this.fResizing; }
    /**
    Returns true while dragging
    @type {boolean}
    */
    get Dragging() { return this.fDragging; }
    /**
    Returns the element to drag and/or resize
    @type {HTMLElement}
    */
    get Handle() { return this.fHandle; }
    /**
    Returns the element that is used as the drag (move) handle, i.e. a header in a window.
    @type {HTMLElement}
    */
    get DragHandle() { return this.fDragHandle; }


    /* private methods */
    /**
     * True if the number passes the check
     * @param {number} v The number to check
     * @return {boolean} True if the number passes the check
     */
    IsValidWidth(v) {
        return v >= this.MinWidth && v <= this.MaxWidth;
    }
    /**
     * True if the number passes the check
     * @param {number} v The number to check
     * @return {boolean} True if the number passes the check
     */
    IsValidHeight(v) {
        return v >= this.MinHeight && v <= this.MaxHeight;
    }
    /**
     * Sets the cursor of the handle of this instance
     * @param {tp.Cursors} Cursor A string denoting the cursor.
     */
    SetCursor(Cursor) {
        if (this.Handle instanceof HTMLElement) {
            this.Handle.ownerDocument.body.style.cursor = Cursor;
            this.Handle.style.cursor = Cursor;

            if (this.Handle.parentNode instanceof HTMLElement) {
                this.Handle.parentNode.style.cursor = Cursor;
            }
        }
    }
    /**
     * Activates and de-activates the dragger
     * @param {boolean} v A boolean value
     */
    SetActive(v) {
        v = v === true;
        if (v !== this.Active) {
            if (v) {
                this.fHandle.addEventListener('scroll', this, true);
                this.fHandle.addEventListener('mousedown', this, true);
                this.fHandle.addEventListener('mouseout', this, true);
                this.fHandle.ownerDocument.addEventListener('mousemove', this, true);
                //this.fHandle.ownerDocument.addEventListener('mouseup', this, true);
            } else {
                this.fHandle.removeEventListener('scroll', this, true);
                this.fHandle.removeEventListener('mousedown', this, true);
                this.fHandle.removeEventListener('mouseout', this, true);
                this.fHandle.ownerDocument.removeEventListener('mousemove', this, true);
                //this.fHandle.ownerDocument.removeEventListener('mouseup', this, true);
            }

            this.fActive = v;
        }
    }

    /**
     * Event handler
     * @param {MouseEvent} e The mouse event.
     */
    DragStart(e) {
        if (this.Dragging === true || this.Resizing === true) {
            var Mouse = tp.Mouse.ToElement(e, this.fHandle.parentNode);
            var Style = tp.GetComputedStyle(this.Handle);
            var X = Mouse.X - tp.ExtractNumber(Style.left);
            var Y = Mouse.Y - tp.ExtractNumber(Style.top);
            this.fDelta = new tp.Point(X, Y);

            this.OnDragStart(e);
        }

        //this.OnDragStart(e);
    }
    /**
     * Event handler
     * @param {MouseEvent} e The mouse event.
     */
    DragMove(e) {

        let L, T, W, H, Mouse;

        if (this.IsDraggable && this.Dragging) {
            Mouse = tp.Mouse.ToElement(e, this.fHandle.parentNode);

            L = Mouse.X - this.fDelta.X;
            T = Mouse.Y - this.fDelta.Y;

            this.fHandle.style.left = L + 'px';
            this.fHandle.style.top = T + 'px';

            this.OnDragMove(e);
        } else if (this.IsResizable && this.Resizing) {

            let Style = tp.GetComputedStyle(this.fHandle);
            L = tp.ExtractNumber(Style.left);
            T = tp.ExtractNumber(Style.top);
            W = tp.ExtractNumber(Style.width);
            H = tp.ExtractNumber(Style.height);

            let Dif = 0;


            let BR = this.fHandle.getBoundingClientRect();

            if (e.clientX < BR.left) {
                L = L - (BR.left - e.clientX);
                W = W + (BR.left - e.clientX);
            } else if (e.clientX > BR.left && e.clientX < BR.right) {
                if (tp.Edge.IsLeft(this.fEdge)) {
                    L = L + (e.clientX - BR.left);
                    W = W - (e.clientX - BR.left);
                } else if (tp.Edge.IsRight(this.fEdge)) {
                    W = W - (BR.right - e.clientX);
                }
            } else if (e.clientX > BR.right) {
                W = W + (e.clientX - BR.right);
            }


            if (e.clientY < BR.top) {
                T = T - (BR.top - e.clientY);
                H = H + (BR.top - e.clientY);
            } else if (e.clientY > BR.top && e.clientY < BR.bottom) {
                if (tp.Edge.IsTop(this.fEdge)) {
                    T = T + (e.clientY - BR.top);
                    H = H - (e.clientY - BR.top);
                } else if (tp.Edge.IsBottom(this.fEdge)) {
                    H = H - (BR.bottom - e.clientY);
                }
            } else if (e.clientY > BR.bottom) {
                H = H + (e.clientY - BR.bottom);
            }

            if (this.IsValidWidth(W) && this.IsValidHeight(H)) {
                this.fHandle.style.left = L + 'px';
                this.fHandle.style.width = W + 'px';
                this.fHandle.style.top = T + 'px';
                this.fHandle.style.height = H + 'px';
            }


            this.OnDragMove(e);
        }

    }
    /**
     * Event handler
     * @param {MouseEvent} e The mouse event.
     */
    DragEnd(e) {
        this.SetCursor(this.fOldCursor);
        this.OnDragEnd(e);
    }

    /* public */
    /**
     * Event handler
     * @param {Event} e The event.
     */
    handleEvent(e) {
        if (this.Active === true) {

            let Flag = false; // for cancelling the event - NOT USED
            let Edge;

            if (tp.IsSameText('scroll', e.type) && (this.Resizing || this.Dragging)) {
                tp.CancelEvent(e);
                e.preventDefault();
                return;
            }

            if (tp.IsSameText('mousedown', e.type)) {
                if (tp.Mouse.IsLeft(e)) {

                    Edge = tp.Edge.ResizeHitTest(e, this.fHandle, this.HandleSize);
                    if (this.IsDraggable && (Edge === tp.Edge.None) && tp.ContainsEventTarget(this.fDragHandle, e.target)) {
                        this.fDragging = true;
                        this.SetCursor(tp.Cursors.Move);
                        Flag = true;
                        this.DragStart(e);
                    } else if (this.IsResizable) {
                        if (tp.Bf.In(Edge, this.Edges)) {
                            this.fResizing = true;
                            this.fEdge = Edge;
                            this.SetCursor(tp.Edge.ToCursor(Edge));
                            Flag = true;
                            this.DragStart(e);
                        }
                    }

                    if (this.Dragging === true || this.Resizing === true) {
                        this.fMouseInfo = new tp.MouseInfo(e);
                        this.fHandle.ownerDocument.addEventListener('mouseup', this, true);
                    }

                }
            } else if (tp.IsSameText('mousemove', e.type)) {
                if (!this.InMove) {
                    this.InMove = true;
                    try {
                        if (tp.Mouse.IsLeft(e) && (this.fDragging || this.fResizing)) {
                            let Dif = this.fMouseInfo.Dif(e);
                            if (!(Math.abs(Dif.X) > 5 || Math.abs(Dif.Y) > 5))
                                return;

                            Flag = true;
                            this.DragMove(e);
                        } else if (this.IsResizable) {
                            Edge = tp.Edge.ResizeHitTest(e, this.fHandle, this.HandleSize);
                            if (tp.Bf.In(Edge, this.Edges)) {
                                this.SetCursor(tp.Edge.ToCursor(Edge));
                            } else {
                                this.SetCursor(this.fOldCursor);
                            }
                        }
                    } finally {
                        this.InMove = false;
                    }
                }

            } else if (tp.IsSameText('mouseout', e.type)) {
                if (!this.fResizing) {
                    this.SetCursor(this.fOldCursor);
                }
            } else if (tp.IsSameText('mouseup', e.type)) {
                Flag = this.fDragging || this.fResizing;

                if (Flag) {
                    this.fHandle.ownerDocument.removeEventListener('mouseup', this, true);
                    this.DragEnd(e);
                }

                this.fDragging = false;
                this.fResizing = false;
                this.fEdge = tp.Edge.None;

                this.fMouseInfo = null;
            }
        }
    }

    /* event triggers */
    /**
    Event trigger
     @param {MouseEvent} e The mouse event.
    */
    OnDragStart(e) { this.Trigger(tp.Events.DragStart, { e: e }); }
    /**
    Event trigger
     @param {MouseEvent} e The mouse event.
    */
    OnDragMove(e) { this.Trigger(tp.Events.DragOver, { e: e }); }
    /**
    Event trigger
     @param {MouseEvent} e The mouse event.
    */
    OnDragEnd(e) { this.Trigger(tp.Events.DragEnd, { e: e }); }



};

/* private */
tp.Dragger.prototype.fActive = false;                      // when false no resizing/moving takes place
tp.Dragger.prototype.fMode = tp.DraggerMode.Both;          // tp.DraggerMode
tp.Dragger.prototype.fDragging = false;
tp.Dragger.prototype.fResizing = false;

tp.Dragger.prototype.fHandle = null;                        // HTMLElement
tp.Dragger.prototype.fDragHandle = null;                    // HTMLElement

tp.Dragger.prototype.fOldCursor = '';                       // tp.Cursors constant
tp.Dragger.prototype.fEdge = tp.Edge.None;                  // resize edge
tp.Dragger.prototype.fDelta = null;                         // tp.Point

tp.Dragger.prototype.fMouseInfo = null;                     // tp.MouseInfo;

tp.Dragger.prototype.InMove = false;

/* public */

/** Bit-field, the edges to be used as valid resize handlers. */
tp.Dragger.prototype.Edges = tp.Edge.All;

/** Resize limit */
tp.Dragger.prototype.MinWidth = 50;
/** Resize limit */
tp.Dragger.prototype.MaxWidth = 6000;
/** Resize limit */
tp.Dragger.prototype.MinHeight = 50;
/** Resize limit */
tp.Dragger.prototype.MaxHeight = 6000;

/** The size (width and heigth) of a resize handler */
tp.Dragger.prototype.HandleSize = 8;

//#endregion


//---------------------------------------------------------------------------------------
// Window classes
//---------------------------------------------------------------------------------------

//#region tp.WindowSettings

tp.WindowSettings = {
    BackColor: 'white',
    Border: '1px solid #CCC',
    CaptionHeight: '30px',
    TextPadding: '2px 4px'
};
//#endregion

//#region  tp.DialogResult
/** Enum-like object for modal window result. 
 @class
 @enum {number}
 */
tp.DialogResult = {
    None: 0,
    OK: 1,
    Cancel: 2,
    Abort: 3,
    Retry: 4,
    Ignore: 5,
    Yes: 6,
    No: 7
};
Object.freeze(tp.DialogResult);
//#endregion

//#region tp.WindowArgs
/**
Arguments for the tp.tpWindow constructor
@class
@extends tp.CreateParams
*/
tp.WindowArgs = class extends tp.CreateParams {
    /**
    Constructor.
    @param {object} [SourceArgs=null] Optional. The source arguments to copy from.
    */
    constructor(SourceArgs = null) {
        super(SourceArgs);
    }

    /** Returns the dialog result, one of the constants of {@link tp.DialogResult}, after a modal dialog box closes.
     * @type {number} 
     */
    get DialogResult() {
        return this.Window instanceof tp.tpWindow ? this.Window.DialogResult : tp.DialogResult.None;
    }
};

/* properties */

/** Window initial left position. Ignored with small screens. */
tp.WindowArgs.prototype.X = 100;
/** Window initial top position. Ignored with small screens. */
tp.WindowArgs.prototype.Y = 200;
/** Window initial width. Ignored with small screens. */
tp.WindowArgs.prototype.Width = 800;
/** Window initial height. Ignored with small screens. */
tp.WindowArgs.prototype.Height = 600;
/** When true the window is initially centered in the viewport. */
tp.WindowArgs.prototype.CenterScreen = true;
/** Window caption text */
tp.WindowArgs.prototype.Text = 'Window';
/** True makes the window header visible */
tp.WindowArgs.prototype.ShowHeader = true;
/** True makes the window footer visible */
tp.WindowArgs.prototype.ShowFooter = true;

/** When true, then the window provides a close button in the upper right corner */
tp.WindowArgs.prototype.CloseBox = true;

/** The edges to be used as resize handlers for the window. Setting to tp.Edge.None makes a non-resizable window
 @type {tp.Edge}
*/
tp.WindowArgs.prototype.ResizeEdges = tp.Edge.All;                                  // tp.Edge.SW | tp.Edge.SE | tp.Edge.S;
/** True indicates a movable (draggable) window */
tp.WindowArgs.prototype.Movable = true;

/** An object. Context for the CloseFunc */
tp.WindowArgs.prototype.Creator = null;
/** Callback to call when the window closes. A function as function(Args: tp.WindowArgs) */
tp.WindowArgs.prototype.CloseFunc = null;

/** The tp.tpWindow window after the creation.
@type {tp.tpWindow}
*/
tp.WindowArgs.prototype.Window = null;
/** Element or selector with html content. An element that becomes the content of the window. 
@type {string|HTMLElement}
*/
tp.WindowArgs.prototype.Content = null;

/** Modal indication flag for the inheritors. For the window to function as a modal one the ShowDialog() must be used. */
tp.WindowArgs.prototype.AsModal = false;

/** What dialog result to returns when the close button in the upper left corner is clicked or the escape is pressed.
@type {tp.DialogResult}
*/
tp.WindowArgs.prototype.DefaultDialogResult = tp.DialogResult.Cancel;
//#endregion

//#region tp.tpWindow (🗖 🗗 🗕🗙      × _ □ )

/**
The ultimate ancestor of all windows and dialog boxes
@class
*/
tp.tpWindow = class extends tp.tpElement {
    /**
    Constructor
    @param {tp.WindowArgs} Args - Setup options
    */
    constructor(Args) {
        Args.Parent = tp.Doc.body;

        Args.CssText = `
box-sizing: border-box;
position: absolute;
display: flex;
flex-direction: column;
align-items: stretch;
background-color: ${tp.WindowSettings.BackColor};
border: ${tp.WindowSettings.Border};
overflow: hidden;
outline: none;
`;

        Args.Id = tp.SafeId('tp-Window');
        super(null, Args);
    }

    /**
    Shows or hides the element by setting the display style property.
     @type {boolean}
    */
    get Visible() { return this.Handle ? this.Handle.style.display === 'flex' : false; }
    set Visible(v) {
        if (this.Handle) {
            v = v === true;
            if (this.Visible !== v) {
                this.Handle.style.display = v ? 'flex' : 'none';
                this.OnVisibleChanged();
            }
        }
    }

    /* properties */
    /**
    Returns the setup options object.
    @type {tp.WindowArgs}
    */
    get Args() { return this.CreateParams; }
    /**
    Gets or sets the title text
    @type {string}
    */
    get Text() { return this.HeaderText ? this.HeaderText.innerHTML : ''; }
    set Text(v) {
        if (this.HeaderText) {
            this.HeaderText.innerHTML = v;
        }
    }
    /**
    Returns true when this is a modal dialog
    @type {string}
    */
    get Modal() { return this.fModal; }
    /**
    Returns true when in maximized state.
    @type {string}
    */
    get IsMaximized() { return this.fMaximized; }
    /**
    Gets or sets the dialog result of a modal window. Setting dialog result to any number value != 0, closes the modal window.
    @type {tp.DialogResult}
    */
    get DialogResult() { return this.fDialogResult; }
    set DialogResult(v) {
        if (this.Modal === true
            && tp.IsNumber(v)
            && v !== this.DialogResult
            && v !== tp.DialogResult.None) {
            this.fDialogResult = v;
            this.Close();
        }
    }


    /* overrides */
    /**
    Override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.tpWindow';
        this.fDefaultCssClasses = 'tp-Window';
    }
    /**
    Notification  
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnHandleCreated() {
        super.OnHandleCreated();
        this.CreateOverlay();
        this.IsElementResizeListener = true;
        this.HookEvent(tp.Events.KeyDown);
        this.Handle.tabIndex = -1;
    }
    /**
    Initializes fields and properties just before applying the create params.        
    */
    InitializeFields() {
        super.InitializeFields();
        this.fModal = false;

    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Notification
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization and options (CreateParams) processing <br />
    Notification
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnCreateParamsProcessed() {
        super.OnCreateParamsProcessed();

        this.Args.Window = this;
        this.ProcessInitInfo();
    }
    /**
    Override
    */
    Dispose() {
        this.DisposeOverlay();
        super.Dispose();
    }
    /**
    Override
    @returns {number} Returns the current z-index.
    */
    BringToFront() {
        var zIndex = this.ZIndex;
        var Result = 0;
        var v;
        for (var i = 0, ln = tp.tpWindow.Windows.length; i < ln; i++) {
            if ((tp.tpWindow.Windows[i] !== this) && !tp.tpWindow.Windows[i].Modal) {
                v = tp.tpWindow.Windows[i].ZIndex;
                Result = Math.max(Result, v);
            }
        }

        if (Result > zIndex) {
            Result++;
            this.ZIndex = Result;
        }

        return Result;
    }
    /**
    Handles any DOM event
    @param {Event} e The event
    */
    OnAnyDOMEvent(e) {
        if (e instanceof KeyboardEvent) {
            if (tp.IsSameText('keydown', e.type)) {
                var Key = e.keyCode;
                if (tp.Keys.Escape === Key) {
                    this.EscapePressed(e);
                }
            }
        }
    }


    /* overridables */
    ProcessInitInfo() { }
    SetupPositionAndSize() {
        //var vpSize = tp.Viewport.GetSize();

        if (tp.Viewport.IsXSmall || tp.Viewport.IsSmall) {
            this.X = 1;
            this.Y = 1;
            this.Width = this.Document.documentElement.clientWidth - 3;     // vpSize.Width - 40;
            this.Height = this.Document.documentElement.clientHeight - 3;   // vpSize.Height - 40; 
        } else {
            this.X = this.Args.X;
            this.Y = this.Args.Y;
            this.Width = this.Args.Width;
            this.Height = this.Args.Height;

            if (this.Args.CenterScreen === true) {
                this.CenterInScreen();
            }
        }

    }
    CenterInScreen() {
        var vpSize = tp.Viewport.GetSize();
        var Size = this.OffsetSize;

        this.X = Math.round(((vpSize.Width - Size.Width) / 2));
        this.Y = Math.round(((vpSize.Height - Size.Height) / 2));
    }

    SetupDragger() {
        if (!tp.Viewport.IsSmall) {
            if ((this.Args.ResizeEdges !== tp.Edge.None) || (this.Args.Movable === true)) {
                var DragHandle = this.Args.Movable === true ? this.HeaderText : null;
                var DragOnly = !tp.IsEmpty(DragHandle) && (this.Args.ResizeEdges === tp.Edge.None);
                let Mode = DragOnly ? tp.DraggerMode.Drag : tp.DraggerMode.Both;

                this.fDragger = new tp.Dragger(Mode, this.Handle, DragHandle);
                this.fDragger.Edges = this.Args.ResizeEdges;
                this.fDragger.MinHeight = 100;
                this.fDragger.MinWidth = 100;
            }
        }
    }
    CreateOverlay() {
        this.fOverlay = new tp.ScreenOverlay();
        this.ZIndex = this.fOverlay.ZIndex + 1;
        this.fOverlay.Visible = false;

        this.fOverlay.Handle.appendChild(this.Handle);
    }
    DisposeOverlay() {
        if (this.fOverlay) {
            this.fOverlay.Dispose();
            this.fOverlay = null;
        }
    }

    /**
     * Creates and appends a button in the footer element. Returns the tp.tpElement button.
     * @param {string} Command The command of the button, if any, else null.
     * @param {string} Title The text of the button
     * @param {tp.DialogResult} [DialogResult=tp.DialogResult.None] Optional. For modal windows only. The dialog result to return when the button is clicked. One of the tp.DialogResult constants.
     * @param {boolean} [ToLeft=false] Optional. True places the button to the left of the footer, false to the right.
     * @returns {tp.tpElement} Returns the tp.tpElement button.
     */
    CreateFooterButton(Command, Title, DialogResult = tp.DialogResult.None, ToLeft = false) {
        let Style = `
position: relative;
text-decoration: none;
padding: 0 15px;
line-height: inherit;
`;

        let CP = new tp.CreateParams();
        CP.Id = tp.SafeId('tp-Window-FooterButton');
        CP.Text = Title;
        CP.Width = 70;
        CP.CssClasses = 'tp-Button';
        CP.CssText = Style;

        var Result = new tp.tpElement('button', CP);

        Result['Command'] = Command;
        Result['DialogResult'] = DialogResult;
        Result.On(tp.Events.Click, this.AnyClick, this);

        if (ToLeft === true)
            Result.InsertAt(0, this.Footer);
        else
            Result.AppendTo(this.Footer);


        return Result;
    }
    /**
     * Creates the content element.
     * @returns {tp.tpElement} Returns the tp.tpElement content element.
     * */
    CreateContentElement() {
        if (tp.IsEmpty(this.Content)) {
            let Style = `
position: absolute;
display: block;
left: 0;
top: 0;
width: 100%;
height: 100%;
`;
            let CP = new tp.CreateParams();
            CP.Id = tp.SafeId('tp-Window-Content');
            CP.Parent = this.ContentWrapper;
            CP.CssText = Style;

            // content
            this.Content = new tp.tpElement(null, CP); 


        }
        return this.Content;
    }

    /**
     * INTERNAL - Creates and adds a button (HTMLImageElement) to the header.
     * @param {string} Command The button command
     * @param {string} Src The url data representing the button image.
     * @returns {HTMLImageElement} Returns the button (HTMLImageElement)
     */
    AddHeaderButton(Command) {
        let Style = `
position: relative;
display: flex;
align-items: center;
user-select: none;
`;

        // 🗖 🗖 🗗 🗕🗙 × _ □ 
        let Type = '🗙'; // 'Close';
        if (Command === 'Maximize')
            Type = '🗖';
        else if (Command === 'Restore')
            Type = '🗗';

        var Result = this.Document.createElement('span');
        this.HeaderButtonBar.appendChild(Result);

        Result.style.cssText = Style;    
        Result.innerHTML = Type;

        Result['Command'] = Command;
        Result.title = Command;

        tp.On(Result, tp.Events.Click, this.FuncBind(this.WindowAnyClick), true);

        return Result;
    }
    /** 
     * Creates all controls of this window. 
     * */
    CreateControls() {

        let Style;

        this.fMaximizeButton = null;
        this.fRestoreButton = null;
        this.fDragger = null;

        // header
        let CP = new tp.CreateParams();

        this.Header = this.Document.createElement('div');
        this.Header.id = tp.SafeId('tp-Window-Header');
        this.Handle.appendChild(this.Header);
        Style = `
position: relative;
display: flex;
align-items: stretch;
height: ${tp.WindowSettings.CaptionHeight};
border-bottom: ${tp.WindowSettings.Border};
padding-right: 3px;
user-select: none;
`;
        this.Header.style.cssText = Style;        

        // header - text
        this.HeaderText = this.Document.createElement('span');
        this.Header.appendChild(this.HeaderText);
        Style = `
position: relative;
display: flex;
flex-direction: column;
justify-content: center;
flex-grow: 1;
padding: ${tp.WindowSettings.TextPadding};
font-weight: bold;
user-select: none;
`;
        this.HeaderText.style.cssText = Style;  
        this.HeaderText.innerHTML = this.Args.Text;

        tp.On(this.HeaderText, tp.Events.DoubleClick, this.FuncBind(this.WindowAnyClick), true);

        // header - button-bar
        this.HeaderButtonBar = this.Document.createElement('div');
        this.HeaderButtonBar.id = tp.SafeId('tp-Window-Header-ButtonBar');
        this.Header.appendChild(this.HeaderButtonBar);
        Style = `
position: relative;
display: flex;
align-items: center;
height: auto;
user-select: none;
gap: 4px;
`;
        this.HeaderButtonBar.style.cssText = Style; 

        if (!tp.Viewport.IsSmall && (this.Args.ResizeEdges !== tp.Edge.None)) {
            this.fMaximizeButton = this.AddHeaderButton('Maximize');
            this.fRestoreButton = this.AddHeaderButton('Restore');
            this.fRestoreButton.style.display = 'none';
        }

        if (this.Args.CloseBox === true) {
            this.fCloseButton = this.AddHeaderButton('Close');
        }

        if (this.Args.ShowHeader === false) {
            this.Header.style.display = 'none';
        }

        // content-wrapper
        CP = new tp.CreateParams();
        CP.Id = tp.SafeId('tp-Window-Content-Wrapper');
        CP.Parent = this;
        CP.CssText = `
position: relative;
display: block;
flex-grow: 1;
`;
        this.ContentWrapper = new tp.tpElement(null, CP);  
        this.ContentWrapper.IsElementResizeListener = true;
        this.ContentWrapper.On('Resized', this.ContentResized, this);

        // footer
        CP = new tp.CreateParams();
        CP.Parent = this;

        this.Footer = this.Document.createElement('div');
        this.Footer.id = tp.SafeId('tp-Window-Footer');
        this.Handle.appendChild(this.Footer);
        Style = `
position: relative;
height: auto;
display: flex;
align-items: center;
min-height: ${tp.WindowSettings.CaptionHeight};
border-top: ${tp.WindowSettings.Border};
user-select: none;
padding: 0.15em;
gap: 0.15em;
`;

 
        this.Footer.style.cssText = Style;  

        // 
        let divElement = this.Document.createElement('div');
        this.Footer.appendChild(divElement);
        divElement.style.cssText = 'position: relative; flex-grow: 1;';

        if (this.Args.ShowFooter === false) {
            this.Footer.style.display = 'none';
        }
    }

    /**
     * Used with modal windows.
     * Passes the results back to the caller code. Results could an object, an array or anything.
     * */
    PassBackResult() {
        tp.Call(this.Args.CloseFunc, this.Args.Creator, this.Args);
    }
    /**
     * Maximizes the window.
     * */
    Maximize() {
        if (!tp.Viewport.IsSmall && !this.IsMaximized) {
            this.fLastRect = this.OffsetRect;  // save current position/size to restore later

            var h = window.innerHeight;
            if (this.Document.documentElement.scrollWidth > this.Document.documentElement.clientWidth) {
                h = h - tp.Environment.ScrollbarSize.Height;
            }

            this.X = 1;
            this.Y = 1;
            this.Width = this.Document.documentElement.clientWidth - 3;
            this.Height = h - 3;


            if (this.fMaximizeButton) {
                this.fMaximizeButton.style.display = 'none';
                this.fRestoreButton.style.display = '';
            }

            if (this.fDragger) {
                this.fDragger.Active = false;
            }

            this.fMaximized = true;
        }
    }
    /** 
     *  Restores the window 
     * */
    Restore() {
        if (!tp.Viewport.IsSmall && this.IsMaximized) {
            this.X = this.fLastRect.X;
            this.Y = this.fLastRect.Y;
            this.Width = this.fLastRect.Width;
            this.Height = this.fLastRect.Height;

            if (this.fMaximizeButton) {
                this.fMaximizeButton.style.display = '';
                this.fRestoreButton.style.display = 'none';
            }

            if (this.fDragger) {
                this.fDragger.Active = true;
            }


            this.fMaximized = false;
        }
    }
    /**
     * Event handler. Handles the escape key.
     * @param {KeyboardEvent} e A KeyboardEvent object
     */
    EscapePressed(e) {
        if (this.Modal) {
            this.DialogResult = this.Args.DefaultDialogResult;
        }
    }
    /**
     * Called when the window shows up.
     * */
    OnShow() { }

    /* public */
    /**
    Shows the window
    */
    Show() {
        if (this.Modal) {
            this.fOverlay.Visible = true;
        }
        this.Visible = true;

        if (tp.IsEmpty(this.Header)) {
            this.CreateControls();
            this.SetupDragger();
            this.SetupPositionAndSize();
        }

        this.Handle.focus();
        this.OnShow();
    }
    /**
    Hides the window
    */
    Hide() {
        this.Visible = false;
        if (this.Modal) {
            this.fOverlay.Visible = true;
        }
    }
    /**
    Shows the window as modal dialog
    */
    ShowModal() {
        this.fDialogResult = tp.DialogResult.None;
        this.fModal = true;
        this.Show();
    }
    /**
    Closes and disposes this instance.
    */
    Close() {
        this.PassBackResult();
        this.Hide();
        this.Dispose();
        tp.ListRemove(tp.tpWindow.Windows, this);
    }

    /* events */
    /**
     * Called when the content wrapper is resized.
     * It triggers the ContentResized event.
     * @param {tp.EventArgs} Args A tp.EventArgs object.
     */
    ContentResized(Args) {
        this.ResizeControls();
        this.Trigger('ContentResized', {});
    }
    /**
     * Event handler for all "standard" clicks on the window, such as the Maximize, Close and Restore.
     * @param {Event} e The event object.
     */
    WindowAnyClick(e) {
        var Command, Type = tp.Events.ToTripous(e.type);

        if (tp.IsSameText(tp.Events.DoubleClick, Type)) {
            if (this.IsMaximized) {
                this.Restore();
            }
            else {
                this.Maximize();
            }
        } else if (!tp.IsBlank(e.target['Command'])) {
            Command = e.target['Command'];
            if (tp.IsSameText('Maximize', Command)) {
                this.Maximize();
            } else if (tp.IsSameText('Restore', Command)) {
                this.Restore();
            } else if (tp.IsSameText('Close', Command)) {
                if (this.Modal) {
                    this.DialogResult = this.Args.DefaultDialogResult;
                }
            }
        }
    }
    /**
    Event handler for buttons added with CreateFooterButton().  
    It does nothing here though. Inheritors should override this method and provide the functionality for their added buttons.
    @param {tp.EventArgs} Args A tp.EventArgs object
    */
    AnyClick(Args) {
        if (this.Modal === true) {
            var Button = Args.Sender;
            if (tp.IsNumber(Button['DialogResult']))
                this.DialogResult = Button['DialogResult'];
        }
    }


};

/* static */
tp.tpWindow.Windows = [];                         // tp.tpWindow[]

/* protected */
tp.tpWindow.prototype.fOverlay = null;            // tp.ScreenOverlay;
tp.tpWindow.prototype.fDragger = null;            // tp.Dragger;

tp.tpWindow.prototype.fMaximizeButton = null;     // HTMLImageElement;
tp.tpWindow.prototype.fRestoreButton = null;      // HTMLImageElement;
tp.tpWindow.prototype.fCloseButton = null;        // HTMLImageElement;

tp.tpWindow.prototype.fLastRect = new tp.Rect(0, 0, 0, 0);
tp.tpWindow.prototype.fMaximized = false;

tp.tpWindow.prototype.fModal = false;
tp.tpWindow.prototype.fDialogResult = tp.DialogResult.None;

//tp.tpWindow.prototype.fArgs = null;                // tp.WindowArgs;

tp.tpWindow.prototype.Header = null;              // HTMLElement;
tp.tpWindow.prototype.HeaderText = null;          // HTMLSpanElement;
tp.tpWindow.prototype.HeaderButtonBar = null;     // HTMLElement;
tp.tpWindow.prototype.Footer = null;              // HTMLElement;

/** It always created by this base class */
tp.tpWindow.prototype.ContentWrapper = null;      // tp.tpElement
/** It may be created after a specific call to CreateContentElement() */
tp.tpWindow.prototype.Content = null;             // tp.tpElement

/** 
 The "three-lines" icon as url-data.
 @static
 @type {string}
 */
tp.tpWindow.ICON_ThreeLines = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA8AAAANCAYAAAB2HjRBAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAadEVYdFNvZnR3YXJlAFBhaW50Lk5FVCB2My41LjEwMPRyoQAAACxJREFUOE9jcHBw+E8uZiBXI0gfZZr////PQC4mWyPIQsqcPRpgpKW2gQttALaGnxXL5WQ1AAAAAElFTkSuQmCC';
//#endregion

//#region  tp.ContentWindow
/**
A <code>tp.tpWindow</code> for displaying any type of html content. <br />
Use the <code>tp.ContentWindow.Show()</code>, or <code>ShowModal()</code> in order to pass the root element with the content to display.
@extends tp.tpWindow
*/
tp.ContentWindow = class extends tp.tpWindow {
    /**
    Constructor 
    @param {tp.WindowArgs} Args - Setup options.
    */
    constructor(Args) {
        super(Args);
    }

    /* overrides */
    /**
    * @override
    * @protected
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ContentWindow';
        this.fDefaultCssClasses = tp.tpWindow.CSS_CLASS_Window;
    }
    /**
    @override
    @protected
    */
    Dispose() {
        if (tp.IsElement(this.Args.ContentParent)) {
            this.Args.ContentParent.appendChild(this.Args.Content);
        }
        super.Dispose();
    }
    /** Creates all controls of this window.
    @override
    @protected
    */
    CreateControls() {
        super.CreateControls();

        if (this.Args.AsModal === true) {
            this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK);
            this.CreateFooterButton('Cancel', 'Cancel', tp.DialogResult.Cancel);
        }

        this.SetContent(this.Args.Content);
    }
    /**
     * Sets the content of the dialog box.
     * @override
     * @protected
     * @param {string|HTMLElement} Content The html text or element to use as content.
     */
    SetContent(Content) {
        Content = tp.Select(Content);
        if (tp.IsElement(Content)) {
            this.Args.Content = Content;
            this.Args.ContentParent = Content.parentNode;
            this.ContentWrapper.Handle.appendChild(Content);
        }
    }
};
/* static */
/**
Displays a content window, either as modal or as non-modal.
@static
@param {boolean} Modal Flag
@param {string} Text - The caption title of the window.
@param {string|HTMLElement} Content - Element or selector with html content.
@param {function} [CloseFunc=null] - Optional. Called when the window closes. A function as <code>function (Args: tp.WindowArgs): void</code>.
@param {object} [Creator=null] - Optional. The context (this) for the callback function.
@returns {tp.ContentWindow} Returns the <code>tp.ContentWindow</code> dialog box
*/
tp.ContentWindow.Show = function (Modal, Text, Content, CloseFunc = null, Creator = null) {
    var Args = new tp.WindowArgs();
    Args.Creator = Creator;
    Args.CloseFunc = CloseFunc;
    Args.Text = Text;
    Args.Width = 800;
    Args.Height = 600;
    Args.ShowFooter = Modal;
    Args.Content = Content;

    Args.AsModal = Modal;
    Args.DefaultDialogResult = tp.DialogResult.Cancel;

    var Result = new tp.ContentWindow(Args);
    if (Modal)
        Result.ShowModal();
    else
        Result.Show();
    return Result;
};
/**
Displays a content window, either as modal or as non-modal, and returns a Promise.
@static
@param {boolean} Modal Flag
@param {string} Text - The caption title of the window
@param {string|HTMLElement} Content - Element or selector with html content
@returns {Promise} Returns a Promise with the modal window Args (<code>tp.WindowArgs</code>)
*/
tp.ContentWindow.ShowAsync = async function (Modal, Text, Content) {
    return new Promise((Resolve, Reject) => {
        tp.ContentWindow.Show(Modal, Text, Content, (Args) => {
            Resolve(Args);
        });
    });
};
/**
Displays a content window, as modal, and returns a Promise.
@static
@param {string} Text - The caption title of the window
@param {string|HTMLElement} Content - Element or selector with html content
@returns {Promise} Returns a Promise with the modal window Args (<code>tp.WindowArgs</code>)
*/
tp.ContentWindow.ShowModalAsync = async function (Text, Content) {
    return tp.ContentWindow.ShowAsync(true, Text, Content);
};
//#endregion


//---------------------------------------------------------------------------------------
// dialog boxes
//---------------------------------------------------------------------------------------


//#region  tp.MessageDialog (internal class)
/**
Internal class
*/
tp.MessageDialog = class extends tp.tpWindow {

    /**
     * Constructor
     * @param {tp.WindowArgs} Args The window args
     */
    constructor(Args) {
        super(Args);
    }


    /* overrides */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.MessageDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();
        this.BoxType = this.Args['BoxType'] || '';
        this.MessageText = this.Args['MessageText'] || '';
    }
    CreateControls() {
        super.CreateControls();


        // title text - buttons
        switch (this.BoxType) {
            case 'Information':
                this.Text = 'Information';
                this.CreateFooterButton('Close', 'Close', tp.DialogResult.Cancel);
                this.Args.DefaultDialogResult = tp.DialogResult.Cancel;
                break;
            case 'Error':
                this.Text = 'Error';
                this.CreateFooterButton('Close', 'Close', tp.DialogResult.Cancel);
                this.Args.DefaultDialogResult = tp.DialogResult.Cancel;
                break;
            case 'Question':
                this.Text = 'Question';
                this.CreateFooterButton('Yes', 'Yes', tp.DialogResult.Yes);
                this.CreateFooterButton('No', 'No', tp.DialogResult.No);
                this.Args.DefaultDialogResult = tp.DialogResult.No;
                break;
        }


        // memo
        let CP = new tp.CreateParams();
        CP.Parent = this.CreateContentElement(); // content
        CP.CssClasses = 'tp-Memo';
        CP.CssText = `
width: calc(100% - 6px);
height: calc(100% - 6px);
font-family: monospace;
white-space: pre;
overflow: auto;
border: none;
outline: none;
resize: none;
padding: 4px;
`;
        CP.SpellCheck = false;
        this.edtMemo = new tp.tpElement('textarea', CP);

        this.edtMemo.SetAttributes({
            cols: 10,
            rows: 5
        });




        this.edtMemo.Text = this.MessageText;

        this.edtMemo.Focus();
    }



};

tp.MessageDialog.prototype.BoxType = '';
tp.MessageDialog.prototype.MessageText = '';
tp.MessageDialog.prototype.edtMemo = null;      // tp.tpElement

/* static */
/**
 * Shows the dialog modally.
 * @param {string} MessageText The text to display
 * @param {string} BoxType The "type" of the dialog, i.e. Information, Error and Question.
 * @param {function} [CloseFunc=null] Optional. A callback function as function(Args: tp.WindowArgs): void to be called when the dialog closes.
 * @param {object} [Creator=null] Optional. A context (this) to be used when calling the callback function.
 * @returns {tp.MessageDialog} Returns the tp.MessageDialog dialog instance.
 */
tp.MessageDialog.Show = function (MessageText, BoxType, CloseFunc = null, Creator = null) {
    let Args = new tp.WindowArgs();
    Args.Width = 500;
    Args.Height = 300;
    Args.CloseFunc = CloseFunc;
    Args.Creator = Creator;
    Args.BoxType = BoxType;
    Args.MessageText = MessageText;
    //Args.fModal = true;

    var Result = new tp.MessageDialog(Args);
    Result.ShowModal();
    return Result;
};
//#endregion

//#region  Dialog boxes
/**
Displays an information modal dialog.
* @param {string} MessageText The text to display
* @param {function} [CloseFunc=null] Optional. A callback function as function(Args: tp.WindowArgs): void to be called when the dialog closes.
* @param {object} [Creator=null] Optional. A context (this) to be used when calling the callback function.
* @returns {tp.MessageDialog} Returns the tp.MessageDialog dialog instance.
*/
tp.InfoBox = function (MessageText, CloseFunc = null, Creator = null) {
    return tp.MessageDialog.Show(MessageText, 'Information', CloseFunc, Creator);
};
/**
Displays an error modal dialog.
* @param {string} MessageText The text to display
* @param {function} [CloseFunc=null] Optional. A callback function as function(Args: tp.WindowArgs): void to be called when the dialog closes.
* @param {object} [Creator=null] Optional. A context (this) to be used when calling the callback function.
* @returns {tp.MessageDialog} Returns the tp.MessageDialog dialog instance.
*/
tp.ErrorBox = function (MessageText, CloseFunc = null, Creator = null) {
    if (tp.IsHtml(MessageText))
        return tp.FrameBox('Error', MessageText, CloseFunc, Creator);
    else
        return tp.MessageDialog.Show(MessageText, 'Error', CloseFunc, Creator);
};
/**
Displays a yes-no modal dialog.
* @param {string} MessageText The text to display
* @param {function} [CloseFunc=null] Optional. A callback function as function(Args: tp.WindowArgs): void to be called when the dialog closes.
* @param {object} [Creator=null] Optional. A context (this) to be used when calling the callback function.
* @returns {tp.MessageDialog} Returns the tp.MessageDialog dialog instance.
*/
tp.YesNoBox = function (MessageText, CloseFunc = null, Creator = null) {
    return tp.MessageDialog.Show(MessageText, 'Question', CloseFunc, Creator);
};


/**
Displays an information dialog and returns a promise.
@param {string} MessageText - The message to display to the user.
@returns {tp.WindowArgs} Returns a {@link Promise} with the modal window {@link tp.WindowArgs} Args.
*/
tp.InfoBoxAsync = async function (MessageText) {
    return new Promise((Resolve, Reject) => {
        tp.InfoBox(MessageText, (Args) => {
            Resolve(Args);
        });
    });
};
/**
Displays an error modal dialog and returns a promise.
@param {string} MessageText - The message to display to the user.
@returns {tp.WindowArgs} Returns a {@link Promise} with the modal window {@link tp.WindowArgs} Args.
*/
tp.ErrorBoxAsync = async function (MessageText) {
    return new Promise((Resolve, Reject) => {
        tp.ErrorBox(MessageText, (Args) => {
            Resolve(Args);
        });
    });
};
/**
Displays a yes-no modal dialog and returns a promise.
@param {string} MessageText - The message to display to the user.
@returns {boolean} Returns a {@link Promise} with a boolean value, where true indicates that the Yes button has been clicked.
*/
tp.YesNoBoxAsync = async function (MessageText) {
    return new Promise((Resolve, Reject) => {
        tp.YesNoBox(MessageText, (Args) => {
            let DialogResult = Args.Window.DialogResult;
            Resolve(DialogResult === tp.DialogResult.Yes);
        });
    });
};


//#endregion

//#region  FrameBox functions
/**
Displays a modal window with an iframe element
@param {string} Text - The caption title of the window
@param {string} UrlOrHtmlContent - The url or the html content (text) to display
@param {function} [CloseFunc=null] - Optional. Called when the window closes. A function as function (Args: tp.WindowArgs): void.
@param {Object} [Creator=null] - Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns a tp.tpWindow window.
*/
tp.FrameBox = function (Text, UrlOrHtmlContent, CloseFunc = null, Creator = null) {
    let Args = new tp.WindowArgs();
    Args.Creator = Creator;
    Args.CloseFunc = CloseFunc;
    Args.Text = Text;
    Args.Width = 800;
    Args.Height = 600;

    let box = new tp.tpWindow(Args);
    box.ShowModal();


    let frame = box.Document.createElement('iframe');
    box.ContentWrapper.Handle.appendChild(frame);
    tp.SetStyle(frame, {
        position: 'absolute',
        left: 0,
        top: 0
    });
    frame.width = '100%';
    frame.height = '100%';

    tp.FrameRemoveBorder(frame);

    tp.ShowSpinner(true);

    let IsContentLoaded = false;
    function FrameLoaded() {
        if (!IsContentLoaded) {
            tp.ForceHideSpinner();
            box.Header.focus();
            IsContentLoaded = true;
        }
    }

    tp.On(frame, tp.Events.Load, () => {
        FrameLoaded();
    });

    // stop loading iframe after 30 seconds
    setTimeout(FrameLoaded, 1000 * 30);

    if (tp.IsHtml(UrlOrHtmlContent)) {
        if ('srcdoc' in frame) {
            frame['srcdoc'] = UrlOrHtmlContent;
        } else {
            let Doc = frame.contentDocument;
            if (Doc) {
                Doc.open();
                Doc.write(UrlOrHtmlContent);
                Doc.close();
            }
        }
    } else {
        frame.src = UrlOrHtmlContent;
    }


    let btn = box.CreateFooterButton('Close', 'Close', tp.DialogResult.Cancel);

    btn.On(tp.Events.Click, (Args) => {
        var Button = Args.Sender;
        if (tp.IsNumber(Button['DialogResult']))
            box.DialogResult = Button['DialogResult'];
    });

    return box;
};
/**
Displays a modal window with an iframe element and returns a promise
@param {string} Text - The caption title of the window
@param {string} UrlOrHtmlContent - The url or the html content (text) to display
@returns {Promise} Returns a promise with the modal window Args (tp.WindowArgs)
*/
tp.FrameBoxAsync = function (Text, UrlOrHtmlContent) {
    return new Promise((Resolve, Reject) => {
        tp.FrameBox(Text, UrlOrHtmlContent, (Args) => {
            Resolve(Args);
        });
    });
};

//#endregion


//---------------------------------------------------------------------------------------
// tp.StaticFiles
//---------------------------------------------------------------------------------------

/** Used in loading/unloading js and css files to head element dynamically. */
tp.StaticFiles = { 
};

/** A list with the dynamically loaded modules (javascript files). */
tp.StaticFiles.JavascriptFiles = [];
/** A list with the dynamically loaded css files. */
tp.StaticFiles.CssFiles = [];

/**
 * Loads a javascript file dynamically.
 * NOTE: Files loaded dynamically are referenced counted. When ref-counting drops to zero the element is deleted from DOM.
 * @param {string} Url The url path of the file.
 * @returns {Promise} Returns a promise.
 */
tp.StaticFiles.LoadJavascriptFile = async function (Url) {
 
    let FileUrl = Url.toLowerCase();
    let oFile = tp.StaticFiles.JavascriptFiles.find(item => { return item.FileUrl === FileUrl; });

    if (!tp.IsValid(oFile)) {
        let ExecutorFunc = (Resolve, Reject) => {
            try {
                let el = tp.Doc.createElement("script");
                el.src = Url;
                el.onload = function () {
                    oFile = {
                        FileUrl: FileUrl,
                        Counter: 1,
                        Element: el
                    };
                    tp.StaticFiles.JavascriptFiles.push(oFile);

                    Resolve();
                };
                el.onerror = function (e) {
                    Reject(e);
                };
                let Head = tp('head');
                Head.appendChild(el);
            } catch (e) {
                Reject(e);
            }
        };

        let Result = new Promise(ExecutorFunc);
        return Result;
    }
    else {
        oFile.Counter += 1;
    }

    return Promise.resolve();
};
/**
 * Unloads a dynamically loaded javascript file.
 * @param {string} Url The url path of the file.
 */
tp.StaticFiles.UnLoadJavascriptFile = function (Url) {
    let FileUrl = Url.toLowerCase();
    let oFile = tp.StaticFiles.JavascriptFiles.find(item => { return item.FileUrl === FileUrl; });

    if (tp.IsValid(oFile)) {
        oFile.Counter -= 1;

        if (oFile.Counter <= 0) {
            let Head = tp('head');
            Head.removeChild(oFile.Element);
            tp.ListRemove(tp.StaticFiles.JavascriptFiles, oFile);
        }
    }
};

/**
 * Loads a list of javascript files dynamically.
 * @param {string[]} UrlList An array with the url paths of the files.
 */
tp.StaticFiles.LoadJavascriptFiles = async function (UrlList) {
    if (tp.IsArray(UrlList)) {
        let i, ln;

        for (i = 0, ln = UrlList.length; i < ln; i++) {
            await tp.StaticFiles.LoadJavascriptFile(UrlList[i]);
        }
    }
};
/**
 * Unloads a dynamically loaded list of javascript files 
 * @param {string[]} UrlList An array with the url paths of the files.
 */
tp.StaticFiles.UnLoadJavascriptFiles = function (UrlList) {
    if (tp.IsArray(UrlList)) {
        let i, ln;

        for (i = 0, ln = UrlList.length; i < ln; i++) {
            tp.StaticFiles.UnLoadJavascriptFile(UrlList[i]);
        }
    }
};

/**
 * Loads a css file dynamically.
 * NOTE: Files loaded dynamically are referenced counted. When ref-counting drops to zero the element is deleted from DOM.
 * @param {string} Url The url path of the file.
 * @returns {Promise} Returns a promise.
 */
tp.StaticFiles.LoadCssFile = async function (Url) {

    let FileUrl = Url.toLowerCase();
    let oFile = tp.StaticFiles.CssFiles.find(item => { return item.FileUrl === FileUrl; });

    if (!tp.IsValid(oFile)) {
        let ExecutorFunc = (Resolve, Reject) => {
            try {
                let el = tp.Doc.createElement("link");
                el.href = Url;
                el.rel = 'stylesheet';
                el.type = 'text/css';

                el.onload = function () {
                    oFile = {
                        FileUrl: FileUrl,
                        Counter: 1,
                        Element: el
                    };
                    tp.StaticFiles.CssFiles.push(oFile);

                    Resolve();
                };
                el.onerror = function (e) {
                    Reject(e);
                };
                let Head = tp('head');
                Head.appendChild(el);
            } catch (e) {
                Reject(e);
            }
        };

        let Result = new Promise(ExecutorFunc);
        return Result;
    }
    else {
        oFile.Counter += 1;
    }

    return Promise.resolve();
};
/**
 * Unloads a dynamically loaded css file.
 * @param {string} Url The url path of the file.
 */
tp.StaticFiles.UnLoadCssFile = function (Url) {
    let FileUrl = Url.toLowerCase();
    let oFile = tp.StaticFiles.CssFiles.find(item => { return item.FileUrl === FileUrl; });

    if (tp.IsValid(oFile)) {
        oFile.Counter -= 1;

        if (oFile.Counter <= 0) {
            let Head = tp('head');
            Head.removeChild(oFile.Element);
            tp.ListRemove(tp.StaticFiles.CssFiles, oFile);
        }
    }
};

/**
 * Loads a list of css files dynamically.
 * @param {string[]} UrlList An array with the url paths of the files.
 */
tp.StaticFiles.LoadCssFiles = async function (UrlList) {
    if (tp.IsArray(UrlList)) {
        let i, ln;

        for (i = 0, ln = UrlList.length; i < ln; i++) {
            await tp.StaticFiles.LoadCssFile(UrlList[i]);
        }
    }
};
/**
 * Unloads a dynamically loaded list of css files
 * @param {string[]} UrlList An array with the url paths of the files.
 */
tp.StaticFiles.UnLoadCssFiles = function (UrlList) {
    if (tp.IsArray(UrlList)) {
        let i, ln;

        for (i = 0, ln = UrlList.length; i < ln; i++) {
            tp.StaticFiles.UnLoadCssFile(UrlList[i]);
        }
    }
};

//---------------------------------------------------------------------------------------
// NotificationBox
//---------------------------------------------------------------------------------------

//#region  NotificationBox

/** NotificationBox. Displays a notification message to the user.
 */
NotificationBox = class extends tp.tpElement {
    /**
     * Constructor
     * @param {string} Message The message
     * @param {tp.NotificationType} Type The type
     */
    constructor(Message, Type) {
        /*
            <div class='tp-NotificationBox tp-Error'>
                <div class='tp-Caption'>
                    <div class="tp-Close tp-UnSelectable">✖</div>
                </div>
                <div class="tp-Content">
                    <textarea> </textarea>
                </div>
            </div>
        */

        // create the box and append it to body

        let Setup = tp.NotificationBoxSetup[tp.EnumNameOf(tp.NotificationType, Type)];
        let Title = Setup.Title;
        let BackColor = Setup.BackColor;
        let BorderColor = Setup.BorderColor;

        let elBox = tp.Doc.createElement('div');
        elBox.id = tp.SafeId('tp-NotificationBox');
        let S = tp.Format('tp-NotificationBox tp-{0}', tp.EnumNameOf(tp.NotificationType, Type)); // tp.NotificationType[Type]);
        elBox.className = S;

        let CaptionId = tp.SafeId('tp-Note-Caption');
        let btnCloseId = tp.SafeId('tp-Note-Close');
        let ContentId = tp.SafeId('tp-Note-Content');
        let edtMessageId = tp.SafeId('tp-Note-Text');

        let Html = `
        <div id='${CaptionId}'>
            <div>${Title}</div>
            <div style='flex-grow: 1;' ></div>
            <div id='${btnCloseId}' ">✖</div> 
        </div>
        <div id='${ContentId}' >
            <textarea id='${edtMessageId}' > </textarea> 
        </div>
`;
        elBox.innerHTML = Html;

        // append to the body
        tp.Doc.body.appendChild(elBox);


        // call the super constructor
        super(elBox, null);

        this.Type = Type;


        // further initialize the box

        let CssStyle = {
            'position': 'fixed',
            'display': 'flex',
            'flex-direction': 'column',
            'background-color': BackColor,
            'height': tp.px(tp.NotificationBoxSetup.Height),
            'margin': '4px 4px',
            'right': '10px',
            'border': '1px solid ' + BorderColor,
            'border-left-width': '6px',
            'opacity': '0',
            'transition': tp.Format('opacity {0}s ease-in 0s', tp.NotificationBoxSetup.DurationSecs),  //   // google : css transition fade out
            'user-select': 'none'
        };

        if (tp.Viewport.IsXSmall) {
            CssStyle['left'] = '2px';
            CssStyle['right'] = '2px';
        } else {
            CssStyle['width'] = tp.px(tp.NotificationBoxSetup.Width);
        }
        this.SetStyle(CssStyle);



        // Caption setup  // justify-content: flex-end;
        let CssText = `
position: relative;
display: flex;
align-items: center;
padding: 0 6px;
outline: none;
border-bottom: 1px solid ${BorderColor};
user-select: none;
`;

        this.divCaption = tp('#' + CaptionId);
        this.divCaption.style.cssText = CssText;
        this.divCaption.tabIndex = 0;

        // Close button setup
        CssText = `
padding: 2px 0;
cursor: default;
font-family: monospace;
font-size: 12px;
padding: 0 2px;
user-select: none;
`;
        this.divClose = tp('#' + btnCloseId);
        this.divClose.style.cssText = CssText;

        // Content setup
        CssText = `
position: relative;
flex-grow: 1;
display: flex; 
padding: 4px;
`;
        this.divContent = tp('#' + ContentId);
        this.divContent.style.cssText = CssText;
        this.divContent.tabIndex = 0;

        // edtMessage textarea setup
        CssText = `
position: relative;
display: block;
width: 100%;
height: 100%;
font-family: monospace;
font-size: 13px;
white-space: pre;
overflow: auto;
border: none;
outline: none;
resize: none;
padding: 4px;
box-sizing: border-box;
background-color: ${BorderColor};
`;
        this.edtMessage = tp('#' + edtMessageId);
        this.edtMessage.style.cssText = CssText;
        this.edtMessage.cols = 10;
        this.edtMessage.rows = 2;
        this.edtMessage.value = Message;

        // add to the notification boxes
        tp.NotificationBoxes.Place(this.Handle);
        tp.NotificationBoxes.Add(this.Handle);

        // set a close timeout
        setTimeout(function (box) {
            if (!box.Clicked) {
                box.Dispose();
            }
        }, tp.NotificationBoxSetup.DurationSecs * 1000, this);

        // Dragger setup
        this.Dragger = new tp.Dragger(tp.DraggerMode.Both, this.Handle, this.divCaption);
        this.Dragger.On(tp.Events.DragStart, this.OnAnyEvent, this);

        // events
        this.HookEvent(tp.Events.Click);
        this.HookEvent(tp.Events.KeyDown);
        this.HookEvent(tp.Events.MouseDown);

        this.BringToFront();
    }

    /** Returns a string indicating the box type.
     @type {string}
     */
    get TypeText() { return tp.EnumNameOf(tp.NotificationType, this.Type); }
    /* overrides */

    /**
     * Initializes the 'static' and 'read-only' class fields
     * @protected 
     * @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.NotificationBox';
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event}  e The Event object
    */
    OnAnyDOMEvent(e) {
        let Type = tp.Events.ToTripous(e.type);

        if (tp.IsSameText(tp.Events.Click, Type)) {
            if (this.Clicked !== true) {
                this.Clicked = true;
                this.Handle.style.transition = 'none';
                this.Handle.style.opacity = '1';
            }

            if (e.target === this.divClose) {
                this.Dispose();
            }
        } else if (e instanceof KeyboardEvent) {
            if (tp.IsSameText(tp.Events.KeyDown, Type)) {
                var Key = e.keyCode;
                if (tp.Keys.Escape === Key) {
                    this.Dispose();
                }
            }
        } else if (tp.IsSameText(tp.Events.MouseDown, Type)) {
            this.BringToFront();
            //log(this.TypeText + ': ' + this.ZIndex.toString());
        }

        if (!this.IsDisposed)
            super.OnAnyDOMEvent(e);
    }
    /**
    Handles any event. Even DOM events are send in this method.
    @protected
    @override
    @param {tp.EventArgs} Args The tp.EventArgs object
    */
    OnAnyEvent(Args) {
        if (tp.IsSameText(tp.Events.DragStart, Args.EventName)) {
            if (this.Dragger) {
                tp.NotificationBoxes.Remove(this.Handle);

                let X = this.Handle.offsetLeft - 5;
                let Y = this.Handle.offsetTop - 5;

                requestAnimationFrame(() => {
                    this.X = X;
                    this.Y = Y;
                    this.Position = 'absolute';
                });
            }
        }
    }
    /**        
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    @protected
    @override
    */
    Dispose() {
        tp.NotificationBoxes.Remove(this.Handle);
        super.Dispose();
    }
};
/** Field
 * 
 * @type {tp.NotificationType}
 * */
NotificationBox.prototype.Type = tp.NotificationType.Information;
/** Field
 * @protected
 * @type {HTMLElement}
 * */
NotificationBox.prototype.divCaption = null;    // HTMLElement;
/** Field
 * @protected
 * @type {HTMLElement}
 * */
NotificationBox.prototype.divClose = null;      // HTMLElement;
/** Field
 * @protected
 * @type {HTMLElement}
 * */
NotificationBox.prototype.divContent = null;    // HTMLElement;
/** Field
 * @protected
 * @type {HTMLTextAreaElement}
 * */
NotificationBox.prototype.edtMessage = null;    // HTMLTextAreaElement;

/** Field
 * @protected
 * @type {boolean}
 * */
NotificationBox.prototype.Clicked = false;
/** Field
 * @protected
 * @type {tp.Dragger}
 * */
NotificationBox.prototype.Dragger = null;       // tp.Dragger = null;


// 
/** Replace the above tp.NotifyFunc <br />
Displays a notification message to the user.
@param {string} Message The notification message
@param {NotificationType} Type The type of notification, i.e. Warning, Error, etc
 */
tp.NotifyFunc = (Message, Type) => {
    let Box = new NotificationBox(Message, Type);
};

//#endregion



//---------------------------------------------------------------------------------------
// tp Properties and constants
//---------------------------------------------------------------------------------------

//#region tp Properties and constants

// NOTE: some properties or constants may declared twice, just for the intellisense to work.
/** Line Break    
 * WARNING: .Net line break = \r\n */
tp.LB = '\n';
tp.SPACE = ' ';
tp.NO_NAME = 'no-name';
tp.NONE = "[none]";

tp.NULL = "___null___";

tp.FromField = "_FROM";
tp.ToField = "_TO";


/** The undefined constant as a tp constant.
 * 
 @type {undefined}
 */
tp.Undefined = void 0;
Object.defineProperty(tp, 'Undefined', {
    get() { return void 0; }
});
//tp.Constant('Undefined', tp, void 0); // http://stackoverflow.com/questions/7452341/what-does-void-0-mean





/** The document the script operates on */
tp.Doc = window.frameElement ? window.top.document : window.document;


/** The currently active element 
 @type {Element}
 */
tp.ActiveElement = null;
Object.defineProperty(tp, 'ActiveElement', {
    get() { return tp.Doc.activeElement; }
});


tp.Prefix = 'tp-';

tp.ButtonClasses = [];

/** A global object for keeping the urls used by a javascript application in ajax and other calls. */
tp.Urls = {};
tp.Urls.Language = '/App/Language';
tp.Urls.StringResource = '/App/StringResource';
tp.Urls.StringResourceList = '/App/StringResourceList';
tp.Urls.Culture = '/App/Culture';

tp.IcoChars = {
    Insert: '+',    // '➕',
    Delete: '-',    // '➖',
    Edit: '*',      // '✱',
    Find:  '&#x1F50E;&#xFE0E;', // '🔎︎'  '🔍',
    LargeButtonDown: '&#9660;'
};

// (🗖 🗗 🗕🗙      × _ □ )


/**
Control create params.
Create params are placed here as 
    tp.GlobalCreateParams[Id] = { };

@class
*/
tp.GlobalCreateParams = {};

/**
The system configuration global object
@class
*/
tp.SysConfig = {};
tp.SysConfig.CompanyFieldName = 'CompanyId';
tp.SysConfig.VariablesPrefix = ':@';
tp.SysConfig.LocatorShowDropDownRowCountLimit = 200;
tp.SysConfig.UseServerStringResources = false;
tp.SysConfig.UseServerCultures = false;
tp.SysConfig.DefaultConnection = "DEFAULT";
tp.SysConfig.GlobalErrorHandling = false;


//#endregion


//---------------------------------------------------------------------------------------
// Languages - string resources
//---------------------------------------------------------------------------------------

//#region tp.Language
/**
A class for storing string resources of a certain language <br />
A language is identified by a two-letter language code. <br />
SEE: https://en.wikipedia.org/wiki/ISO_639-1  <br />
SEE: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes <br />
SEE: https://www.ietf.org/rfc/bcp/bcp47.txt
*/
tp.Language = class {

    /**
    Constructor
    @param {string} Name - The name of the language
    @param {string} Code - The two letter code of the language, e.g en, el, it, fr, etc.
    @param {string} CultureCode -  The culture code associated to this language, e.g.  e.g en-US, el-GR, etc.
    */
    constructor(Name, Code, CultureCode) {
        this.fName = Name;
        this.fCode = Code;
        this.fCultureCode = CultureCode;
        this.fItems = new tp.Dictionary();
    }



    /* properties */
    /**
    The two letter code of the language, e.g en, el, it, fr, etc.
    @type {string}
    */
    get Code() { return this.fCode; }
    /**
    The name of the language
     @type {string}
    */
    get Name() { return this.fName; }
    /**
    The culture code associated to this language, e.g.  e.g en-US, el-GR, etc.
     @type {string}
    */
    get CultureCode() { return this.fCultureCode; }
    /**
    The resource string. A string/string  dictionary with the resources of the language
    @type {tp.Dictionary}
    */
    get Items() { return this.fItems; }

    /* public */
    /**
    Adds a source string list to this instance
    @param {object} Source The source object to copy strings from
    */
    AddStringList(Source) {
        if (Source) {
            for (var Key in Source) {
                this.Items.Set(Key, Source[Key]);
            }
        }
    }
    /**
    Returns a string representation of this instance
    @returns {string} Returns a string representation of this instance
    */
    toString() {
        return tp.Format('{0} - {1}', this.Code, this.Name);
    }
};

/* private */
/** Field. The two letter code of the language, e.g en, el, it, fr, etc.
 * @private
 * @type {string}
 */
tp.Language.prototype.fCode = '';
/** Field. The name of the language
 * @private
 * @type {string}
 */
tp.Language.prototype.fName = '';
/** Field. The culture code associated to this language, e.g.  e.g en-US, el-GR, etc.
 * @private
 * @type {string}
 */
tp.Language.prototype.fCultureCode = '';
/** Field. A string/string dictionary
 * @private
 * @type {tp.Dictionary}
 */
tp.Language.prototype.fItems = null; // tp.Dictionary = null;

//#endregion

//#region tp.Languages
/**
A static helper class with a list of languages (Language), where each language is a list of string resources of that certain language
@static
*/
tp.Languages = {

    /**
    Finds and returns a {@link tp.Language} language by its code, if any, else null
    @param {string} Code - The two letter code of the language, e.g en, el, it, fr, etc.
    @returns {tp.Language} Returns a {@link tp.Language} language
    */
    Find: function (Code) {
        return tp.FirstOrDefault(tp.Languages.Items, function (item) {
            return tp.IsSameText(Code, item.Code);
        });
    },
    /**
    Returns true if a {@link tp.Language} language exists, by its code
    @param {string} Code - The code of the language. The two letter code of the language, e.g en, el, it, fr, etc.
    @returns {boolean} Returns true if a {@link tp.Language}  language exists, by its code
    */
    Exists: function (Code) {
        return tp.Languages.Find(Code) !== null;
    },
    /**
    Adds and returns a new {@link tp.Language} language.
   
    @param {string} Name - The name of the language
    @param {string} Code - The two letter code of the language, e.g en, el, it, fr, etc.
    @param {string} CultureCode - The culture code associated to this language, e.g.  e.g en-US, el-GR, etc.
    @returns {tp.Language} Returns a new {@link tp.Language} language.
    */
    Add: function (Name, Code, CultureCode) {
        var Result = tp.Languages.Find(Code);
        if (tp.IsEmpty(Result)) {
            Result = new tp.Language(Name, Code, CultureCode);
            tp.Languages.Items.push(Result);
        }
        return Result;
    },

    get Current() {
        if (this.fCurrent instanceof tp.Language)
            return this.fCurrent;

        if (tp.IsValid(this.Items) && this.Items.length > 0)
            return this.Items[0];

        return null;
    },
    set Current(v) {
        if (v instanceof tp.Language && v !== tp.Languages.fCurrent) {
            tp.Languages.fCurrent = v;

            if (tp.SysConfig.UseServerStringResources === true) {
                let Data = {
                    LanguageCode: v
                };
                tp.Ajax.PostAsync(tp.Urls.Language, Data, (Args) => {
                    // nothing to do
                });
            }
        }
    },
    /**
    The list of resource languages.
    @private
    @type {tp.Language[]}
    */
    Items: [], // tp.Language[];

    /** 
    The current language.
    @type {tp.Language}
    */
    fCurrent: null,
    /** 
    The english language.
    @type {tp.Language}
    */
    En: null,
    /** 
    The greek language.
    @type {tp.Language}
    */
    Gr: null
};




//#endregion

//#region tp.Res
/**
A static class for getting back string resources
@static
*/
tp.Res = {

    // TODO: /App/StringResourceList

    /**
    GS = GetString. <br />
    Finds a string resource, for the current language, by a key. The string resource is passed back to the caller by calling a callback function.
    In case of failure a specified default string is passed back. <br />
    The current language is defined in the Languages class. <br />
    @param {string} Key - The key for the string resource
    @param {Function} ResultFunc - A callback function <code>(Value: string, UserTag?: any) => void </code> provided by the caller, to be called in order to return back the string resource
    @param {string} [Default] - Optional. A default string value, in case the key is not found or the operation fails
    @param {Object} [Context] - Optional. The context (this) to be used when calling the callback function
    @param {any} [UserTag] - Optional. A user defined value
     */
    GS: function (Key, ResultFunc, Default = '', Context = null, UserTag = null) {
        // GS = GetString.
        if (tp.IsString(Key) && tp.IsFunction(ResultFunc)) {
            UserTag = UserTag || Key;
            var Value;
            if (tp.Languages.Current.Items.ContainsKey(Key)) {
                Value = tp.Languages.Current.Items.ValueOf(Key, Default);
                ResultFunc.call(Context, Value, UserTag);
            } else if (tp.SysConfig.UseServerStringResources === true) {
                tp.Ajax.GetAsync(tp.Urls.StringResource, (Args) => {
                    Value = Args.ResponseData.Packet;
                    tp.Languages.Current.Items.Set(Key, Value);
                    ResultFunc.call(Context, Value, UserTag);
                });
            } else {
                ResultFunc.call(Context, Default, UserTag);
            }
        }
    },
    /**
     Finds a string resource, for the current language, by a key. The string resource is passed back to the caller by calling a callback function.
     In case of failure a specified default string is passed back. <br />
     The current language is defined in the Languages class. <br />
     This function first looks up for the key in the current language, and if not there, it uses a provider and tries a communication with the server.
    @param {string} Key - The key for the string resource
    @param {Function} ResultFunc - A callback function <code>(Value: string, UserTag?: any) => void </code> provided by the caller, to be called in order to return back the string resource
    @param {string} [Default] - Optional. A default string value, in case the key is not found or the operation fails
    @param {Object} [Context] - Optional. The context (this) to be used when calling the callback function
    @param {any} [UserTag] - Optional. A user defined value
     */
    GetString: function (Key, ResultFunc, Default = '', Context = null, UserTag = null) {
        tp.Res.GS(Key, ResultFunc, Default, Context, UserTag);
    }
};
//#endregion


//---------------------------------------------------------------------------------------
// Cultures
//---------------------------------------------------------------------------------------

//#region tp.Culture
/**
A culture class. Provides information about a certain culture (locale), i.e. the 'en-US', regarding date and time patterns and separators, day and month names, numeric separators, etc.
*/
tp.Culture = class {

    /**
    Constructor
    @param {object} [Source=null] Optional. A source object to copy property values from.
    */
    constructor(Source = null) {
        if (Source) {
            tp.MergeQuick(this, Source);
        }
    }



};
/** Country name, in English.
 @type {string}
 */
tp.Culture.prototype.Country = '';

/** Culture name, in English.
 @type {string}
 */
tp.Culture.prototype.Name = '';
/** Culture code, i.e. 'en-US'
 @type {string}
 */
tp.Culture.prototype.Code = '';

/** Full date time format pattern.
 @type {string}
 */
tp.Culture.prototype.FullDateTimeFormat = '';

/** Date format pattern.
 @type {string}
 */
tp.Culture.prototype.DateFormat = '';
/** Date separator.
 @type {string}
 */
tp.Culture.prototype.DateSeparator = '';
/** The name of the first day of the week, in English.
 @type {string}
 */
tp.Culture.prototype.FirstDayOfWeek = '';

/** Time format pattern.
 @type {string}
 */
tp.Culture.prototype.TimeFormat = '';
/** Time separator.
 @type {string}
 */
tp.Culture.prototype.TimeSeparator = '';
/** The PM designator.
 @type {string}
 */
tp.Culture.prototype.PM = '';
/** The AM designator.
 @type {string}
 */
tp.Culture.prototype.AM = '';

/** Array of day names
 @type {string[]}
 */
tp.Culture.prototype.DayNames = [];
/** Array of abbreviated day names.
 @type {string[]}
 */
tp.Culture.prototype.AbbreviatedDayNames = [];

/** Array of month names.
 @type {string[]}
 */
tp.Culture.prototype.MonthNames = [];
/** Array of abbreviated month names.
 @type {string[]}
 */
tp.Culture.prototype.AbbreviatedMonthNames = [];

/** Decimal separator.
 @type {string}
 */
tp.Culture.prototype.DecimalSeparator = '';
/** Thousand separator
 @type {string}
 */
tp.Culture.prototype.ThousandSeparator = '';

/** Currency name, in English
 @type {string}
 */
tp.Culture.prototype.CurrencyName = '';
/** Currency code, i.e. USD or EUR.
 @type {string}
 */
tp.Culture.prototype.CurrencyCode = '';
/** Currency symbol, i.e. $ or €
 @type {string}
 */
tp.Culture.prototype.CurrencySymbol = '';

/** Default decimal places for a currency value.
 @type {string}
 */
tp.Culture.prototype.CurrencyDecimals = 0;
//#endregion

//#region tp.Cultures
/**
A static helper class with a list of cultures ({@link tp.Culture}) 
@static
*/
tp.Cultures = {
    /**
     * Finds and returns a {@link tp.Culture} by a specified culture code, i.e. en-US, if any, else null.
     * @param {string} Code The culture code, i.e. en-US
     * @returns {tp.Culture} Returns a {@link tp.Culture}, if any, else null.
     */
    Find: function (Code) {
        return tp.FirstOrDefault(this.Items, function (item) {
            return tp.IsSameText(Code, item.Code);
        });
    },
    /**
     * Adds a {@link tp.Culture} instance in the internal cultures list.
     * @param {tp.Culture} Culture The {tp.Culture} to add.
     */
    Add: function (Culture) {
        if (Culture instanceof tp.Culture && !this.Find(Culture.Code)) {
            this.Items.push(Culture);
        }
    },
    /**
     * Sorts the culture list by a specified property.
     * @param {string} [PropName='Code'] Optional. Defaults to 'Code'. The property name to use in sorting the culture list.
     */
    Sort: function (PropName = 'Code') {
        tp.ListSort(Items, [{ Prop: PropName, Reverse: false }]);
    },

    /** Gets or sets the current {@link tp.Culture} culture.
     * @type {tp.Culture}
     */
    get Current() {
        if (this.fCurrent instanceof tp.Culture)
            return this.fCurrent;

        if (tp.IsValid(this.Items) && this.Items.length > 0)
            return this.Items[0];

        return null;
    },
    set Current(v) {
        if (v instanceof tp.Culture && v !== this.fCurrent) {
            this.fCurrent = v;
        }
    },

    fCurrent: null,
    Items: []

};



//#endregion

//#region Culture related methods

/**
 * Returns the decimal separator of a specified culture, i.e. en-US
 * @param {string} [CultureCode] The culture code, i.e. en-US. If not specified then the current culture is used.
 * @return {string} Returns the decimal separator of a specified culture, i.e. en-US
 */
tp.GetDecimalSeparator = (CultureCode) => {
    if (!tp.IsString(CultureCode) || tp.IsBlank(CultureCode))
        CultureCode = tp.CultureCode;

    let n = 1.1;
    let Result = n.toLocaleString(CultureCode).substring(1, 2);
    return Result;
};
/**
 * Returns the thousand separator of a specified culture, i.e. en-US
 * @param {string} [CultureCode] The culture code, i.e. en-US. If not specified then the current culture is used.
 * @return {string} Returns the thousand separator of a specified culture, i.e. en-US
 */
tp.GetThousandSeparator = (CultureCode) => {
    if (!tp.IsString(CultureCode) || tp.IsBlank(CultureCode))
        CultureCode = tp.CultureCode;

    let n = 1000;
    let Result = n.toLocaleString(CultureCode).substring(1, 2);
    return Result;
};
/**
 * Returns the date separator of a specified culture, i.e. en-US
 * @param {string} [CultureCode] The culture code, i.e. en-US. If not specified then the current culture is used.
 * @returns {string} Returns the date separator of a specified culture, i.e. en-US
 */
tp.GetDateSeparator = (CultureCode) => {
    if (!tp.IsString(CultureCode) || tp.IsBlank(CultureCode))
        CultureCode = tp.CultureCode;

    let S = new Date().toLocaleDateString(CultureCode);

    if (S.indexOf('/') !== -1) {
        return '/';
    } else if (S.indexOf('.') !== -1) {
        return '.';
    }

    return '-';
};
/**
 * Returns the date format, i.e. dd/MM/yyyy or MM/dd/YYYY, of a specified culture, i.e. en-US
 * @param {string} [CultureCode] The culture code, i.e. en-US. If not specified then the current culture is used.
 * @returns {string} Returns the date format, i.e. dd/MM/yyyy or MM/dd/YYYY, of a specified culture, i.e. en-US
 */
tp.GetDateFormat = (CultureCode) => {
    if (CultureCode === 'ISO')
        return tp.DateFormatISO;

    if (!tp.IsString(CultureCode) || tp.IsBlank(CultureCode))
        CultureCode = tp.CultureCode;

    let i, ln;
    let DateSeparator = tp.GetDateSeparator(CultureCode);

    let DT = new Date('2000-10-15');
    let S = DT.toLocaleDateString(CultureCode, { year: 'numeric', month: '2-digit', day: '2-digit' });

    let Parts = S.split(DateSeparator);

    for (i = 0, ln = Parts.length; i < ln; i++)
        Parts[i] = Parts[i].trim();

    for (i = 0, ln = Parts.length; i < ln; i++) {
        if (Parts[i] === '2000') {
            Parts[i] = 'yyyy';
        }
        if (Parts[i] === '10') {
            Parts[i] = 'MM';
        }
        if (Parts[i] === '15') {
            Parts[i] = 'dd';
        }
    }

    return Parts.join(DateSeparator);

};
/**
 *  Returns a {@link tp.DatePattern} constant value by analyzing the DateFormat.
 * @param {string} DateFormat The date format string to analyze, e.g. yyyy-MM-dd or MM/dd/YYYY
 * @returns {number} Returns a {@link tp.DatePattern} constant value by analyzing the DateFormat.
 */
tp.GetDatePattern = function (DateFormat) {
    let Result = tp.DatePattern.DMY;
    DateFormat = DateFormat.trim();
    let C = DateFormat.charAt(0).toUpperCase();

    if (C === 'Y')
        Result = tp.DatePattern.YMD;
    else if (C === 'M')
        Result = tp.DatePattern.MDY;
    else
        Result = tp.DatePattern.DMY;

    return Result;
};
 

//#endregion

//#region Culture related properties and variables 


/** The currency symbol. Defaults to dollar sign. According to current culture (locale)
 @type {string}
 */
tp.CurrencySymbol = null; // '$'; // '€'; 
Object.defineProperty(tp, 'CurrencySymbol', {
    get() { return tp.Cultures.Current.CurrencySymbol; }
});
/** The current decimal separator. According to current culture (locale)
 @type {string}
 */
tp.DecimalSeparator = null; // '.';
Object.defineProperty(tp, 'DecimalSeparator', {
    get() { return tp.Cultures.Current.DecimalSeparator; }
});

/** The current thousand separator. According to current culture (locale)
 @type {string}
 */
tp.ThousandSeparator = null // ',';
Object.defineProperty(tp, 'ThousandSeparator', {
    get() { return tp.Cultures.Current.ThousandSeparator; }
});

/** The current date separator. According to current culture (locale)
 @type {string}
 */
tp.DateSeparator = null; // '/';
Object.defineProperty(tp, 'DateSeparator', {
    get() { return tp.Cultures.Current.DateSeparator; }
});
/** The current date format pattern. According to current culture (locale)
 @type {string}
 */
tp.DateFormat = null; // 'yyyy/MM/dd';
Object.defineProperty(tp, 'DateFormat', {
    get() { return tp.Cultures.Current.DateFormat; }
});
/** The ISO date format pattern
 @type {string}
 */
tp.DateFormatISO = null; // 'yyyy-MM-dd';
Object.defineProperty(tp, 'DateFormatISO', {
    value: 'yyyy-MM-dd',
    writable: false
});
/** Array of day names. According to current culture (locale)
 @type {string[]}
 */
tp.DayNames = null;
Object.defineProperty(tp, 'DayNames', {
    get() { return tp.Cultures.Current.DayNames; }
});
/** Array of monthe names. According to current culture (locale)
 @type {string[]}
 */
tp.MonthNames = null;
Object.defineProperty(tp, 'MonthNames', {
    get() { return tp.Cultures.Current.MonthNames; }
});
//#endregion

//#region tp.CultureCode

/** Gets or sets the current culture, i.e. locale. By default returns 'en-US'. <br />
 * The initial value of this property comes from lang attribute of the html element, e.g. <html lang="en-US"> <br />
@type {string}
*/
tp.CultureCode = null;
Object.defineProperty(tp, 'CultureCode', {
    get() {
        return tp.Cultures.Current.Code;
    },
    set(v) {
        if (v.toUpperCase() !== tp.Cultures.Current.Code.toUpperCase()) {
            let C = tp.Cultures.Find(v);
            if (C) {
                tp.Cultures.Current = C;
            }
            else {
                // is allowed to get this culture from server?
                if (tp.SysConfig.UseServerCultures === true) {
                    let Data = {
                        CultureCode: v
                    };
                    tp.Ajax.PostAsync(tp.Urls.Culture, Data, (Args) => {
                        // TODO
                    });
                }
                else {

                    // create a copy of en-US and set some crucial properties
                    let EnUS = tp.Cultures.Find('en-US');

                    C = new tp.Culture(EnUS);
                    C.Code = v;
                    C.DecimalSeparator = tp.GetDecimalSeparator(v);
                    C.ThousandSeparator = tp.GetThousandSeparator(v);
                    C.DateSeparator = tp.GetDateSeparator(v);
                    C.DateFormat = tp.GetDateFormat(v);

                    tp.Cultures.Add(C);
                    tp.Cultures.Current = C;
                }
            }
        }

    }
});

//#endregion

//---------------------------------------------------------------------------------------
// initialization
//---------------------------------------------------------------------------------------



//#region document ready state


/** Returns true if the document is loaded and ready (readyState === 'complete') */
tp.Property('IsReady', tp, () => tp.Doc.readyState === "complete");
/** For internal use. A list of {@link tp.Listener} objects to be called when the document is loaded and ready. */
tp.ReadyListeners = [];
/**
Adds a listener to the document.onreadystatechange event.
@param {function} Func - The callback function
@param {object} Context - The context (this) of the callback function
*/
tp.AddReadyListener = function (Func, Context = null) {
    var Listener = new tp.Listener(Func, Context);
    tp.ReadyListeners.push(Listener);
};
/**
Executes a specified callback function when the document is loaded and ready.
@param {function} Func - The function to call when document is loaded and ready.
*/
tp.Ready = function (Func) {
    tp.AddReadyListener(Func);
};
/** Just a placeholder. Client code may re-assign this property. 
 * NOTE: It is executed before any ready listeners.
 */
tp.AppInitializeBefore = function () { };
/** Just a placeholder. Client code may re-assign this property.
 * NOTE: It is executed after any ready listeners.
 * */
tp.AppInitializeAfter = function () { };
/** Just a placeholder. Client code may re-assign this property.
 * NOTE: It is executed after the AppInitializeAfter()
 * */
tp.Main = function () { };

/** Just a placeholder for a function that adds languages. */
tp.AddLanguagesFunc = null;

(function () {

    let InitializeLogDiv = function () {
        if (!tp.LogDiv) {
            tp.LogDiv = document.getElementById('LogDiv');
            if (!tp.LogDiv) {
                var div = document.createElement('div');
                div.id = 'LogDiv';
                div.style.cssText = 'padding: 15px';
                tp.LogDiv = div;
                document.body.appendChild(tp.LogDiv);
            }
        }
    };

    let InitializeLanguages = function () {
        tp.Languages.En = new tp.Language('English', 'en', 'en-US');
        tp.Languages.Gr = new tp.Language('Greek', 'el', 'el-GR');
        tp.Languages.Items.push(tp.Languages.En, tp.Languages.Gr);
        tp.Languages.fCurrent = tp.Languages.En;

        tp.Call(tp.AddLanguagesFunc);
    };

    let InitializeCulture = function () {

        // en-US
        let C = new tp.Culture();
        C.Country = 'United States';
        C.Name = 'English (United States)';
        C.Code = 'en-US';
        C.FullDateTimeFormat = 'dddd, MMMM d, yyyy h:mm:ss tt';
        C.DateFormat = '"M/d/yyyy';
        C.DateSeparator = '/';
        C.FirstDayOfWeek = 'Sunday';
        C.TimeFormat = 'h:mm tt';
        C.TimeSeparator = ':';
        C.PM = 'PM';
        C.AM = 'AM';
        C.DayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        C.AbbreviatedDayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        C.MonthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        C.AbbreviatedMonthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        C.DecimalSeparator = '.';
        C.ThousandSeparator = ',';
        C.CurrencyName = 'US Dollar';
        C.CurrencyCode = 'USD';
        C.CurrencySymbol = '$';
        C.CurrencyDecimals = 2;

        tp.Cultures.Add(C);
        tp.Culture.Current = C;

        // el-GR
        C = new tp.Culture();
        C.Country = 'Greece';
        C.Name = 'Greek (Greece)';
        C.Code = 'el-GR';
        C.FullDateTimeFormat = 'dddd, d MMMM yyyy h:mm:ss tt';
        C.DateFormat = 'd/M/yyyy';
        C.DateSeparator = '/';
        C.FirstDayOfWeek = 'Monday';
        C.TimeFormat = 'h:mm tt';
        C.TimeSeparator = ':';
        C.PM = 'μμ';
        C.AM = 'πμ';
        C.DayNames = ["Κυριακή", "Δευτέρα", "Τρίτη", "Τετάρτη", "Πέμπτη", "Παρασκευή", "Σάββατο"];
        C.AbbreviatedDayNames = ["Κυρ", "Δευ", "Τρι", "Τετ", "Πεμ", "Παρ", "Σαβ"];
        C.MonthNames = ["Ιανουάριος", "Φεβρουάριος", "Μάρτιος", "Απρίλιος", "Μάιος", "Ιούνιος", "Ιούλιος", "Αύγουστος", "Σεπτέμβριος", "Οκτώβριος", "Νοέμβριος", "Δεκέμβριος"];
        C.AbbreviatedMonthNames = ["Ιαν", "Φεβ", "Μαρ", "Απρ", "Μαϊ", "Ιουν", "Ιουλ", "Αυγ", "Σεπ", "Οκτ", "Νοε", "Δεκ"];
        C.DecimalSeparator = ',';
        C.ThousandSeparator = '.';
        C.CurrencyName = 'Euro';
        C.CurrencyCode = 'EUR';
        C.CurrencySymbol = '€';
        C.CurrencyDecimals = 2;

        tp.Cultures.Add(C);
        tp.Culture.Current = C;

        let CultureCode = document.querySelector('html').getAttribute('lang');

        if (tp.IsString(CultureCode)) {

            if (tp.IsBlank(CultureCode))
                CultureCode = 'en-US';

            if (CultureCode.length == 2) {
                let o = tp.FirstOrDefault(tp.Cultures.Items, (item) => { return item.Code.startsWith(CultureCode) });
                if (o)
                    CultureCode = o.Code;
            }

            if (CultureCode.length < 5 || CultureCode[2] !== '-')
                tp.Throw(`Invalid culture code: ${CultureCode} \nPlease define a Culture in the html lang attribute. Example lang="en-US"`);

            tp.CultureCode = CultureCode;

            let Parts = CultureCode.split('-');
            let Lang = tp.Languages.Find(Parts[0]);
            if (!Lang) {
                Lang = tp.Languages.Add("unknown", Parts[0], CultureCode);
            }

            tp.Languages.fCurrent = tp.Languages.Lang;
        }

    };

    let ReadyFunc = function () {

        InitializeLogDiv();

        InitializeLanguages();
        InitializeCulture();

        // initialize global objects
        tp.Environment.Initialize();
        tp.Viewport.Initialize();

        if (tp.IsFunction(tp.AppInitializeBefore))
            tp.Call(tp.AppInitializeBefore);

        if (tp.Page) {
            tp.Page.CreatePage();
        }

        // call "ready listeners"
        var list = tp.ReadyListeners;
        var listener;
        for (var i = 0, ln = list.length; i < ln; i++) {
            listener = list[i];
            listener.Func.call(listener.Context);
        }

        if (tp.IsFunction(tp.AppInitializeAfter))
            tp.Call(tp.AppInitializeAfter);

        // call Main()
        if (tp.IsFunction(tp.Main))  
            tp.Call(tp.Main);
 

    };

    if (tp.IsReady) {
        ReadyFunc();
    } else {
        tp.Doc.addEventListener('readystatechange', function (e) {
            if (tp.IsReady) {
                ReadyFunc();
            }
        }, false);
    }

})();






//#endregion