﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Extensions;
using System;
using System.Net;
using System.Net.Mail;

namespace OffLogs.Business.Notifications.Core.Emails.Service
{
    public class EmailSendingService: IEmailSendingService
    {
        private readonly SmtpSettings _smtpSettings;

        private readonly MailAddress _defaultFromAddress;
        private readonly NetworkCredential _credentials;
        private readonly ILogger<EmailSendingService> _Logger;

        private char[] Separators = ";".ToCharArray(); // for splitting lists of emails

        public EmailSendingService(IConfiguration configuration, ILogger<EmailSendingService> Logger)
        {
            _smtpSettings = new SmtpSettings(configuration);
            _defaultFromAddress = new MailAddress(_smtpSettings.EmailFrom, _smtpSettings.UserNameFrom);
            _credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
            _Logger = Logger;
        }

        private void ParseEmails(MailAddressCollection collection, string emails)
        {
            if (string.IsNullOrWhiteSpace(emails))
                return;

            var emailArray = emails.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var em in emailArray)
            {
                collection.Add(em);
            }
        }

        // returns "" on success, or text of SMTP exceptions etc
        public string SendEmail(
            string to,
            EmailBuilder emailBuilder,
            string bcc
        )
        {
            emailBuilder.Build();
            return SendEmail(null, to, emailBuilder.Subject, emailBuilder.Body, null, bcc);
        }

        // returns "" on success, or text of SMTP exceptions etc
        public string SendEmail(
            string to,
            string subject,
            string body,
            string bcc
        )
        {
            return SendEmail(null, to, subject, body, null, bcc);
        }

        // returns "" on success, or text of SMTP exceptions etc
        public string SendEmail(
            string from,
            string to,
            string subject,
            string body,
            string cc,
            string bcc
        )
        {
            string res = string.Empty;
            MailMessage message = new MailMessage();

            message.From = string.IsNullOrWhiteSpace(from) ? _defaultFromAddress : new MailAddress(from);
            ParseEmails(message.ReplyToList, message.From?.Address);

            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("'to' parameter is required");

            ParseEmails(message.To, to);
            ParseEmails(message.CC, cc);
            ParseEmails(message.Bcc, bcc);

            message.Subject = subject.RemoveNewLineSymbols();
            message.IsBodyHtml = true;
            message.Body = body;

            message.Priority = MailPriority.Normal;

            SendViaSmtpClient(message);
            return res;

        }

        private void SendViaSmtpClient(MailMessage message)
        {
            var smtpClient = new SmtpClient(_smtpSettings.Server);
            smtpClient.Port = _smtpSettings.Port;
            smtpClient.Credentials = _credentials;
            smtpClient.EnableSsl = _smtpSettings.EnableSsl;

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                _Logger.LogError(e.Message, e);
            }
        }
    }
}
