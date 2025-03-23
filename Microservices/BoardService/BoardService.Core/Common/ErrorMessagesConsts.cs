namespace BoardService.Core.Common
{
    public static class ErrorMessagesConsts
    {
        public static string INVALID_BOARD_TYPE_ERROR_MESSAGE => "Действие не может быть выполнено для данного типа доски";
        public static string SAVE_ERROR_MESSAGE => "Ошибка сохранения данных";
        public static string DELETE_ERROR_MESSAGE => "Ошибка удаления данных";
        public static string NOT_FOUND_ERROR_MESSAGE => "Объект не найден";
        public static string INVALID_ID_ERROR_MESSAGE => "Неверный id";
        public static string INTERNAL_ERROR_MESSAGE => "Внутренняя ошибка";
    }
}
