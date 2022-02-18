 function StringifyDateTime (DT) {
     var timezoneOffsetInHours = -(DT.getTimezoneOffset() / 60); //UTC minus local time
    var sign = timezoneOffsetInHours >= 0 ? '+' : '-';
    var leadingZero = (Math.abs(timezoneOffsetInHours) < 10) ? '0' : '';

    //It's a bit unfortunate that we need to construct a new Date instance 
    //(we don't want _this_ Date instance to be modified)
     var correctedDate = new Date(DT.getFullYear(), DT.getMonth(),
         DT.getDate(), DT.getHours(), DT.getMinutes(), DT.getSeconds(),
         DT.getMilliseconds());
     correctedDate.setHours(DT.getHours() + timezoneOffsetInHours);
    var iso = correctedDate.toISOString().replace('Z', '');

    return iso + sign + leadingZero + Math.abs(timezoneOffsetInHours).toString() + ':00';
}


let DateTest = () => {
    let DT = new Date();
    DT = tp.ClearTime(DT);
    log(DT);                                // Mon Jun 11 2018 10:14:33 GMT+0430 (Iran Daylight Time)

    let JsonText = JSON.stringify(DT);
    log(JsonText);

    
    JsonText = StringifyDateTime(DT);   
    log(JsonText);

    DT = new Date(JsonText);
    log(DT);

    DT = tp.ClearTime(DT);
    log(DT);

    JsonText = JSON.stringify(DT);
    log(JsonText);                         

    let S = JSON.parse(JsonText);
    log(S);                                

    DT = new Date(S);
    log(DT);                                // Mon Jun 11 2018 10:14:33 GMT+0430 (Iran Daylight Time)

};
 

