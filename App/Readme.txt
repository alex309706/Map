Проект "Map" построен по типу MVC. Возможны ненужные зависимости. Какие-то части оказались не нужными и не использовались при разработке.(Infrastructure.Business, Services...)
Для наполнения приложения данными нужно переписать путь к таблице Excel в классе HomeController.
readonly ISubdivisionRepository repo = new ExcelSubdivisionRepository(Ваш путь к файлу\Книга_с_группировкой_(2).xlsx")


