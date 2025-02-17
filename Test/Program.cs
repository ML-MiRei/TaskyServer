using MimeKit;

try
{
    MimeMessage message = new MimeMessage();
    message.From.Add(new MailboxAddress("Tasky-info", "tasky.information.i@tasky.inf")); //отправитель сообщения
    message.To.Add(new MailboxAddress("", "prttyerl.com")); //адресат сообщения
    message.Subject = "Сообщение от MailKit"; //тема сообщения
    message.Body = new BodyBuilder() { HtmlBody = "<div style=\"color: green;\">Сообщение от MailKit</div>" }.ToMessageBody(); //тело сообщения (так же в формате HTML)

    using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
    {
        client.Connect("smtp.gmail.com", 465, true); //либо использум порт 465
        client.Authenticate("tasky.information.i@gmail.com", "plni rttn wpab gmac"); //логин-пароль от аккаунта
        var s = client.Send(message);

        client.Disconnect(true);
        Console.WriteLine("Сообщение отправлено успешно!");
    }
}
catch (Exception e)
{
    Console.WriteLine(e.GetBaseException().Message);
}


