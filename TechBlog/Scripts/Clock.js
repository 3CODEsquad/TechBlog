
//Clock Script
setInterval("settime()", 1000);

function settime() {
    var dateTime = new Date();
    var hour = dateTime.getHours();
    var minute = dateTime.getMinutes();
    var second = dateTime.getSeconds();

    if (minute < 10)
        minute = "0" + minute;

    if (second < 10)
        second = "0" + second;

    var time = "" + hour + ":" + minute + ":" + second;

    document.getElementById("clock").value = time;

}