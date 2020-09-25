function Set_Year() {
    let date = new Date();
    document.getElementById('year').value = date.getFullYear();
}
function Set_Month() {
    let date = new Date();
    document.getElementById('month').value = date.getMonth() + 1;
}
function Set_Day() {
    let date = new Date();
    document.getElementById('day').value = date.getDate();
}
function Year_Increase() {
    let current = document.getElementById('year').value;
    let increased = +current + 1;
    document.getElementById('year').value = increased;
}
function Year_Decrease() {
    let current = document.getElementById('year').value;
    let decreased = +current - 1;

    document.getElementById('year').value = decreased;
}

function Month_Increase() {
    let current = document.getElementById('month').value;
    let output;
    if (current > 11) {
        output = 1;
        Year_Increase();
    }
    else {
        output = +current + 1;
    }
    document.getElementById('month').value = output;

}
function Month_Decrease() {
    let current = +document.getElementById('month').value;
    let output;
    if (current < 2) {
        Year_Decrease();
        output = 12;
    }
    else {
        output = +current - 1;
    }
    document.getElementById('month').value = output;
}

function Day_Increase() {
    let current = +document.getElementById('day').value;
    let output;
    if (current > 30) {
        output = 1;
        Month_Increase();
    }
    else {
        output = current + 1;
    }
    document.getElementById('day').value = output;
}
function Day_Decrease() {
    let current = +document.getElementById('day').value;
    let output;
    if (current < 2) {
        output = 31;
        Month_Decrease();
    }
    else {
        output = current - 1;
    }

    document.getElementById('day').value = output;
}
Set_Year();
Set_Month();
Set_Day();