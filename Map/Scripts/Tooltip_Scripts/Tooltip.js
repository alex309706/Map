//всплывающая подсказка (информация о штабе)

var svg = document.getElementById("tooltip-svg");

//Функция вывода координат в консоль(для удобства определения координат местополодения штабов)
function Write_Coordinates(evt) {
    var x = evt.clientX;
    var y = evt.clientY;
    console.log("x=" + x + " y=" + y);
}
svg.addEventListener('click', Write_Coordinates);

//группа нужна для SVG (g в svg)
function Create_Group()
{
    var g = document.createElementNS("http://www.w3.org/2000/svg", 'g');
    g.setAttribute('id', 'tooltip');
    g.setAttribute('visibility', 'hidden');
    svg.appendChild(g);
}
Create_Group();

//Создание фона и надписей для группы в svg(для тултипа)
function Create_Text_Elements_And_Background() {

    var g = document.getElementById("tooltip");

    var rectangle_background = document.createElementNS("http://www.w3.org/2000/svg", "rect");
    rectangle_background.setAttribute("id", "rectangle-background");
    rectangle_background.setAttribute("width", "0");
    rectangle_background.setAttribute("height", "0");
    rectangle_background.setAttribute("style", "fill:grey;stroke:black;stroke-width:5;");
    rectangle_background.setAttribute("x", "0");
    rectangle_background.setAttribute("y", "0");
    g.appendChild(rectangle_background);

    var Location = document.createElementNS("http://www.w3.org/2000/svg","text");
    Location.setAttribute("id", "Location");
    Location.setAttribute("class", "Changing");
    Location.setAttribute("x", "5");
    Location.setAttribute("y", "16");
    Location.textContent = "Location";
    g.appendChild(Location);

    var Name = document.createElementNS("http://www.w3.org/2000/svg", "text");
    Name.setAttribute("id", "Name");
    Name.setAttribute("class", "Changing");
    Name.setAttribute("x", "5");
    Name.setAttribute("y", "32");
    Name.textContent = "Name";
    g.appendChild(Name);

    var Composition = document.createElementNS("http://www.w3.org/2000/svg", "text");
    Composition.setAttribute("id", "Composition");
    Composition.setAttribute("class", "Changing");
    Composition.setAttribute("x", "5");
    Composition.setAttribute("y", "48");
    Composition.textContent = "Composition";
    g.appendChild(Composition);

    var Strength = document.createElementNS("http://www.w3.org/2000/svg", "text");
    Strength.setAttribute("id", "Strength");
    Strength.setAttribute("class", "Changing");
    Strength.setAttribute("x", "5");
    Strength.setAttribute("y", "64");
    Strength.textContent = "Strength";
    g.appendChild(Strength);

    var Document = document.createElementNS("http://www.w3.org/2000/svg", "text");
    Document.setAttribute("id", "Document");
    Document.setAttribute("class", "Changing");
    Document.setAttribute("x", "5");
    Document.setAttribute("y", "80");
    Document.textContent = "Document";
    g.appendChild(Document);
}
Create_Text_Elements_And_Background();

//скрываем тултип
function hideTooltip()
{
    setTimeout(() => { tooltip.setAttributeNS(null, "visibility", "hidden");},3000);
}

//вешаем обрааботчики события на действия mousemove и mouseout
var tooltip = document.getElementById('tooltip');
var triggers = document.getElementsByClassName('tooltip-trigger');
for (var i = 0; i < triggers.length; i++)
{
    triggers[i].addEventListener('mousemove', showTooltip);
    triggers[i].addEventListener('mouseout', hideTooltip);
}

//показываем тултип
function showTooltip(evt)
{
    var CTM = svg.getScreenCTM();
    var mouseX = (evt.clientX - CTM.e) / CTM.a;
    var mouseY = (evt.clientY - CTM.f) / CTM.d;
    tooltip.setAttributeNS(null, "x", mouseX + 60 / CTM.a);
    tooltip.setAttributeNS(null, "y", mouseY + 20 / CTM.d);
    tooltip.setAttributeNS(null, "visibility", "visible");
    var x = (evt.clientX - CTM.e + 10) / CTM.a;
    var y = (evt.clientY - CTM.f + 10) / CTM.d;
    tooltip.setAttributeNS(null, "transform", "translate(" + x + " " + y + ")");
    var tooltipText = tooltip.getElementsByClassName('Changing');
    for (var i = 0; i < tooltipText.length; i++)
    {
        var attribute_name = tooltipText[i].id;
        var translation_of_attribute;
        var data = evt.target.getAttributeNS(null, attribute_name);
        if (data == " "|| data=="")
        {
            data = "Нет данных";
        }
        switch (attribute_name)
        {
        case "Location": translation_of_attribute = "Расположение штаба";      break;
        case "Name": translation_of_attribute = "Название армии";              break;
        case "Composition": translation_of_attribute = "Состав";               break;
        case "Strength": translation_of_attribute = "Численность";             break;
        case "Document": translation_of_attribute = "Удостоверяющий документ"; break;
        case "Commander": translation_of_attribute = "Командир";               break;
        default:
        }
        tooltipText[i].firstChild.data = translation_of_attribute+" : "+data;
    }
    var tooltipRect = document.getElementById('rectangle-background');
    tooltipRect.setAttributeNS(null, "width", Counting_width(tooltipText));
    tooltipRect.setAttributeNS(null, "height", Counting_heigt(tooltipText));
}

//Определение ширины тултипа
function Counting_width(mass)
{
    var max_count_of_letters_in_str;
    max_count_of_letters_in_str = mass[0].firstChild.length;
    for (var i = 1; i < mass.length; i++)
    {
        if (max_count_of_letters_in_str < mass[i].firstChild.length)
        {
        max_count_of_letters_in_str = mass[i].firstChild.length;
        }
}
return max_count_of_letters_in_str*7+20;
}

//Определение высоты(магичесвое число 17)
function Counting_heigt(mass)
{
    return mass.length*17;
}