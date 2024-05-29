﻿using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.Utilities
{
    public class HttpClientService : IHttpClientService
    {
        private HttpClient _client = new HttpClient();

        public HttpClientService(IIdentityProvider identityService)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, identityService.Token);
            _client.DefaultRequestHeaders.Add("x-timezone-offset", identityService.TimezoneOffset.ToString());
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
           // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6IkFNSU4iLCJwcm9maWxlIjp7ImZpcnN0bmFtZSI6IkFNSU4iLCJsYXN0bmFtZSI6IkJVS0FOIFJBSVoiLCJnZW5kZXIiOiJtYWxlIiwiZG9iIjpudWxsLCJlbWFpbCI6bnVsbH0sInBlcm1pc3Npb24iOnsiVDE5IjoxLCJOMTgiOjEsIk4xOSI6MSwiTjIwIjoxLCJOMjEiOjEsIk4yMiI6MSwiTjIzIjoxLCJOMjQiOjEsIk4yNSI6MSwiTjI2IjoxLCJOMjciOjEsIk4xNyI6MSwiTjI4IjoxLCJOMzAiOjEsIk4zMSI6MSwiTjMyIjoxLCJOMzMiOjEsIk4zNCI6MSwiTjM1IjoxLCJOMzYiOjEsIk4zNyI6MSwiTjM4IjoxLCJOMzkiOjEsIk4yOSI6MSwiTjE2IjoxLCJOMTUiOjEsIk4xNCI6MSwiTTI1IjoxLCJNMjYiOjEsIk0yNyI6MSwiTTI4IjoxLCJNMjkiOjEsIk0zMCI6MSwiTTMxIjoxLCJNMzIiOjEsIk0zMyI6MSwiTTM0IjoxLCJOMSI6MSwiTjIiOjEsIk4zIjoxLCJONCI6MSwiTjUiOjEsIk42IjoxLCJONyI6MSwiTjgiOjEsIk45IjoxLCJOMTAiOjEsIk4xMSI6MSwiTjEyIjoxLCJOMTMiOjEsIk40MCI6MSwiTjQxIjoxLCJONDIiOjEsIk40MyI6MSwiTzI4IjoxLCJPMjkiOjEsIk8zMCI6MSwiTzMxIjoxLCJPMzIiOjEsIk8zMyI6MSwiTzM0IjoxLCJPMzUiOjEsIk8zNiI6MSwiTzM3IjoxLCJPMzgiOjEsIk8zOSI6MSwiTzQwIjoxLCJPNDEiOjEsIk80MiI6MSwiTzQzIjoxLCJPNDQiOjEsIk80NSI6MSwiTzQ2IjoxLCJPNDciOjEsIk80OCI6MSwiTzQ5IjoxLCJPNTAiOjEsIk8yNyI6MSwiTTI0IjoxLCJPMjYiOjEsIk8yNCI6MSwiTzEiOjEsIk8yIjoxLCJPMyI6MSwiTzQiOjEsIk81IjoxLCJPNiI6MSwiTzciOjEsIk84IjoxLCJPOSI6MSwiTzEwIjoxLCJPMTEiOjEsIk8xMiI6MSwiTzEzIjoxLCJPMTQiOjEsIk8xNSI6MSwiTzE2IjoxLCJPMTciOjEsIk8xOCI6MSwiTzE5IjoxLCJPMjAiOjEsIk8yMSI6MSwiTzIyIjoxLCJPMjMiOjEsIk8yNSI6MSwiTzUxIjoxLCJNMjMiOjEsIk0yMSI6MSwiTDQiOjEsIkw1IjoxLCJMNiI6MSwiTDciOjEsIkw4IjoxLCJMOSI6MSwiTDEwIjoxLCJMMTEiOjEsIkwxMiI6MSwiTDEzIjoxLCJMMyI6MSwiTDE0IjoxLCJMMTYiOjEsIkwxNyI6MSwiTDE4IjoxLCJMMTkiOjEsIkwyMCI6MSwiTDIxIjoxLCJMMjIiOjEsIkwyMyI6MSwiTDI0IjoxLCJMMjUiOjEsIkwxNSI6MSwiTDIiOjEsIkwxIjoxLCJLNjIiOjEsIkszOSI6MSwiSzQwIjoxLCJLNDEiOjEsIks0MiI6MSwiSzQzIjoxLCJLNDQiOjEsIks0NSI6MSwiSzQ2IjoxLCJLNDciOjEsIks0OCI6MSwiSzQ5IjoxLCJLNTAiOjEsIks1MSI6MSwiSzUyIjoxLCJLNTMiOjEsIks1NCI6MSwiSzU1IjoxLCJLNTYiOjEsIks1NyI6MSwiSzU4IjoxLCJLNTkiOjEsIks2MCI6MSwiSzYxIjoxLCJMMjYiOjEsIkwyNyI6MSwiTDI4IjoxLCJMMjkiOjEsIkw1NyI6MSwiTDU4IjoxLCJMNTkiOjEsIk0xIjoxLCJNMiI6MSwiTTMiOjEsIk00IjoxLCJNNSI6MSwiTTYiOjEsIk03IjoxLCJNOCI6MSwiTTkiOjEsIk0xMCI6MSwiTTExIjoxLCJNMTIiOjEsIk0xMyI6MSwiTTE0IjoxLCJNMTUiOjEsIk0xNiI6MSwiTTE3IjoxLCJNMTgiOjEsIk0xOSI6MSwiTTIwIjoxLCJMNTYiOjEsIk0yMiI6MSwiTDU1IjoxLCJMNTMiOjEsIkwzMCI6MSwiTDMxIjoxLCJMMzIiOjEsIkwzMyI6MSwiTDM0IjoxLCJMMzUiOjEsIkwzNiI6MSwiTDM3IjoxLCJMMzgiOjEsIkwzOSI6MSwiTDQwIjoxLCJMNDEiOjEsIkw0MiI6MSwiTDQzIjoxLCJMNDQiOjEsIkw0NSI6MSwiTDQ2IjoxLCJMNDciOjEsIkw0OCI6MSwiTDQ5IjoxLCJMNTAiOjEsIkw1MSI6MSwiTDUyIjoxLCJMNTQiOjEsIkszOCI6MSwiTzUyIjoxLCJPNTQiOjEsIlIxOSI6MSwiUjIwIjoxLCJSMjEiOjEsIlIyMiI6MSwiUjIzIjoxLCJSMjQiOjEsIlIyNSI6MSwiUjI2IjoxLCJSMjciOjEsIlIyOCI6MSwiUjE4IjoxLCJSMjkiOjEsIlIzMSI6MSwiUjMyIjoxLCJSMzMiOjEsIlIzNCI6MSwiUjM1IjoxLCJSMzYiOjEsIlIzNyI6MSwiUzEiOjEsIlMyIjoxLCJTMyI6MSwiUjMwIjoxLCJSMTciOjEsIlIxNiI6MSwiUjE1IjoxLCJRNCI6MSwiUTUiOjEsIlE2IjoxLCJRNyI6MSwiUTgiOjEsIlE5IjoxLCJRMTAiOjEsIlExMSI6MSwiUTEyIjoxLCJSMSI6MSwiUjIiOjEsIlIzIjoxLCJSNCI6MSwiUjUiOjEsIlI2IjoxLCJSNyI6MSwiUjgiOjEsIlI5IjoxLCJSMTAiOjEsIlIxMSI6MSwiUjEyIjoxLCJSMTMiOjEsIlIxNCI6MSwiUzQiOjEsIlM1IjoxLCJTNiI6MSwiUzciOjEsIlMzNSI6MSwiUzM2IjoxLCJTMzciOjEsIlMzOCI6MSwiVDEiOjEsIlQyIjoxLCJUMyI6MSwiVDQiOjEsIlQ1IjoxLCJUNiI6MSwiVDciOjEsIlQ4IjoxLCJUOSI6MSwiVDEwIjoxLCJUMTEiOjEsIlQxMiI6MSwiVDEzIjoxLCJUMTQiOjEsIlQxNSI6MSwiVDE2IjoxLCJUMTciOjEsIlQxOCI6MSwiUzM0IjoxLCJRMyI6MSwiUzMzIjoxLCJTMzEiOjEsIlM4IjoxLCJTOSI6MSwiUzEwIjoxLCJTMTEiOjEsIlMxMiI6MSwiUzEzIjoxLCJTMTQiOjEsIlMxNSI6MSwiUzE2IjoxLCJTMTciOjEsIlMxOCI6MSwiUzE5IjoxLCJTMjAiOjEsIlMyMSI6MSwiUzIyIjoxLCJTMjMiOjEsIlMyNCI6MSwiUzI1IjoxLCJTMjYiOjEsIlMyNyI6MSwiUzI4IjoxLCJTMjkiOjEsIlMzMCI6MSwiUzMyIjoxLCJPNTMiOjEsIlEyIjoxLCJQODIiOjEsIlA2IjoxLCJQNyI6MSwiUDgiOjEsIlA5IjoxLCJQMTAiOjEsIlAxMSI6MSwiUDEyIjoxLCJQMTMiOjEsIlAxNCI6MSwiUDE1IjoxLCJQNSI6MSwiUDE2IjoxLCJQMTgiOjEsIlAxOSI6MSwiUDIwIjoxLCJQMjEiOjEsIlAyMiI6MSwiUDIzIjoxLCJQMjQiOjEsIlAyNSI6MSwiUDI2IjoxLCJQMjciOjEsIlAxNyI6MSwiUDQiOjEsIlAzIjoxLCJQMiI6MSwiTzU1IjoxLCJPNTYiOjEsIk81NyI6MSwiTzU4IjoxLCJPNTkiOjEsIk82MCI6MSwiTzYxIjoxLCJPNjIiOjEsIk82MyI6MSwiTzY0IjoxLCJPNjUiOjEsIk82NiI6MSwiTzY3IjoxLCJPNjgiOjEsIk82OSI6MSwiTzcwIjoxLCJPNzEiOjEsIk83MiI6MSwiTzczIjoxLCJPNzQiOjEsIk83NSI6MSwiTzc2IjoxLCJQMSI6MSwiUDI4IjoxLCJQMjkiOjEsIlAzMCI6MSwiUDMxIjoxLCJQNTkiOjEsIlA2MCI6MSwiUDYxIjoxLCJQNjIiOjEsIlA2MyI6MSwiUDY0IjoxLCJQNjUiOjEsIlA2NiI6MSwiUDY3IjoxLCJQNjgiOjEsIlA2OSI6MSwiUDcwIjoxLCJQNzEiOjEsIlA3MiI6MSwiUDczIjoxLCJQNzQiOjEsIlA3NSI6MSwiUDc2IjoxLCJQNzciOjEsIlA3OCI6MSwiUDc5IjoxLCJQODAiOjEsIlA4MSI6MSwiUDU4IjoxLCJRMSI6MSwiUDU3IjoxLCJQNTUiOjEsIlAzMiI6MSwiUDMzIjoxLCJQMzQiOjEsIlAzNSI6MSwiUDM2IjoxLCJQMzciOjEsIlAzOCI6MSwiUDM5IjoxLCJQNDAiOjEsIlA0MSI6MSwiUDQyIjoxLCJQNDMiOjEsIlA0NCI6MSwiUDQ1IjoxLCJQNDYiOjEsIlA0NyI6MSwiUDQ4IjoxLCJQNDkiOjEsIlA1MCI6MSwiUDUxIjoxLCJQNTIiOjEsIlA1MyI6MSwiUDU0IjoxLCJQNTYiOjEsIkszNyI6MSwiVjEiOjEsIkszNSI6MSwiRDQxIjoxLCJFMSI6MSwiRTIiOjEsIkUzIjoxLCJFNCI6MSwiRTUiOjEsIkU2IjoxLCJFNyI6MSwiRTgiOjEsIkU5IjoxLCJENDAiOjEsIkUxMCI6MSwiRTEyIjoxLCJFMTMiOjEsIkUxNCI6MSwiRTE1IjoxLCJFMTYiOjEsIkUxNyI6MSwiRTE4IjoxLCJFMTkiOjEsIkUyMCI6MSwiRTIxIjoxLCJFMTEiOjEsIkQzOSI6MSwiRDM4IjoxLCJEMzciOjEsIkQxNCI6MSwiRDE1IjoxLCJEMTYiOjEsIkQxNyI6MSwiRDE4IjoxLCJEMTkiOjEsIkQyMCI6MSwiRDIxIjoxLCJEMjIiOjEsIkQyMyI6MSwiRDI0IjoxLCJEMjUiOjEsIkQyNiI6MSwiRDI3IjoxLCJEMjgiOjEsIkQyOSI6MSwiRDMwIjoxLCJEMzEiOjEsIkQzMiI6MSwiRDMzIjoxLCJEMzQiOjEsIkQzNSI6MSwiRDM2IjoxLCJFMjIiOjEsIkUyMyI6MSwiRTI0IjoxLCJFMjUiOjEsIkYxNCI6MSwiRjE1IjoxLCJGMTYiOjEsIkYxNyI6MSwiRjE4IjoxLCJGMTkiOjEsIkYyMCI6MSwiRjIxIjoxLCJGMjIiOjEsIkYyMyI6MSwiRjI0IjoxLCJGMjUiOjEsIkYyNiI6MSwiRjI3IjoxLCJGMjgiOjEsIkYyOSI6MSwiRjMwIjoxLCJGMzEiOjEsIkYzMiI6MSwiRjMzIjoxLCJGMzQiOjEsIkcxIjoxLCJHMiI6MSwiRjEzIjoxLCJEMTMiOjEsIkYxMiI6MSwiRjEwIjoxLCJFMjYiOjEsIkUyNyI6MSwiRTI4IjoxLCJFMjkiOjEsIkUzMCI6MSwiRTMxIjoxLCJFMzIiOjEsIkUzMyI6MSwiRTM0IjoxLCJFMzUiOjEsIkUzNiI6MSwiRTM3IjoxLCJFMzgiOjEsIkUzOSI6MSwiRjEiOjEsIkYyIjoxLCJGMyI6MSwiRjQiOjEsIkY1IjoxLCJGNiI6MSwiRjciOjEsIkY4IjoxLCJGOSI6MSwiRjExIjoxLCJEMTIiOjEsIkQxMSI6MSwiRDEwIjoxLCJCMjQiOjEsIkIyNSI6MSwiQjI2IjoxLCJCMjciOjEsIkIyOCI6MSwiQjI5IjoxLCJCMzAiOjEsIkIzMSI6MSwiQjMyIjoxLCJCMzMiOjEsIkIzNCI6MSwiQjM1IjoxLCJCMzYiOjEsIkIzNyI6MSwiQjM4IjoxLCJCMzkiOjEsIkI0MCI6MSwiQjQxIjoxLCJCNDIiOjEsIkI0MyI6MSwiQjQ0IjoxLCJCNDUiOjEsIkI0NiI6MSwiQjIzIjoxLCJCNDciOjEsIkIyMiI6MSwiQjIwIjoxLCJVMSI6MSwiQTEiOjEsIkEyIjoxLCJBMyI6MSwiQjEiOjEsIkIyIjoxLCJCMyI6MSwiQjQiOjEsIkI1IjoxLCJCNiI6MSwiQjciOjEsIkI4IjoxLCJCOSI6MSwiQjEwIjoxLCJCMTEiOjEsIkIxMiI6MSwiQjEzIjoxLCJCMTQiOjEsIkIxNSI6MSwiQjE2IjoxLCJCMTciOjEsIkIxOCI6MSwiQjE5IjoxLCJCMjEiOjEsIkczIjoxLCJCNDgiOjEsIkI1MCI6MSwiQzIwIjoxLCJDMjEiOjEsIkMyMiI6MSwiQzIzIjoxLCJDMjQiOjEsIkMyNSI6MSwiQzI2IjoxLCJDMjciOjEsIkMyOCI6MSwiQzI5IjoxLCJDMzAiOjEsIkMzMSI6MSwiQzMyIjoxLCJDMzMiOjEsIkQxIjoxLCJEMiI6MSwiRDMiOjEsIkQ0IjoxLCJENSI6MSwiRDYiOjEsIkQ3IjoxLCJEOCI6MSwiRDkiOjEsIkMxOSI6MSwiQjQ5IjoxLCJDMTgiOjEsIkMxNiI6MSwiQjUxIjoxLCJCNTIiOjEsIkI1MyI6MSwiQjU0IjoxLCJCNTUiOjEsIkI1NiI6MSwiQjU3IjoxLCJCNTgiOjEsIkMxIjoxLCJDMiI6MSwiQzMiOjEsIkM0IjoxLCJDNSI6MSwiQzYiOjEsIkM3IjoxLCJDOCI6MSwiQzkiOjEsIkMxMCI6MSwiQzExIjoxLCJDMTIiOjEsIkMxMyI6MSwiQzE0IjoxLCJDMTUiOjEsIkMxNyI6MSwiSzM2IjoxLCJHNCI6MSwiRzYiOjEsIko1IjoxLCJKNiI6MSwiSjciOjEsIko4IjoxLCJKOSI6MSwiSjEwIjoxLCJKMTEiOjEsIkoxMiI6MSwiSjEzIjoxLCJKMTQiOjEsIko0IjoxLCJKMTUiOjEsIkoxNyI6MSwiSjE4IjoxLCJKMTkiOjEsIkoyMCI6MSwiSjIxIjoxLCJKMjIiOjEsIkoyMyI6MSwiSjI0IjoxLCJKMjUiOjEsIkoyNiI6MSwiSjE2IjoxLCJKMyI6MSwiSjIiOjEsIkoxIjoxLCJINjEiOjEsIkg2MiI6MSwiSTEiOjEsIkkyIjoxLCJJMyI6MSwiSTQiOjEsIkk1IjoxLCJJNiI6MSwiSTciOjEsIkk4IjoxLCJJOSI6MSwiSTEwIjoxLCJJMTEiOjEsIkkxMiI6MSwiSTEzIjoxLCJJMTQiOjEsIkkxNSI6MSwiSTE2IjoxLCJJMTciOjEsIkkxOCI6MSwiSTE5IjoxLCJJMjAiOjEsIkkyMSI6MSwiSjI3IjoxLCJKMjgiOjEsIkoyOSI6MSwiSjMwIjoxLCJLMTIiOjEsIksxMyI6MSwiSzE0IjoxLCJLMTUiOjEsIksxNiI6MSwiSzE3IjoxLCJLMTgiOjEsIksxOSI6MSwiSzIwIjoxLCJLMjEiOjEsIksyMiI6MSwiSzIzIjoxLCJLMjQiOjEsIksyNSI6MSwiSzI2IjoxLCJLMjciOjEsIksyOCI6MSwiSzI5IjoxLCJLMzAiOjEsIkszMSI6MSwiSzMyIjoxLCJLMzMiOjEsIkszNCI6MSwiSzExIjoxLCJINjAiOjEsIksxMCI6MSwiSzgiOjEsIkozMSI6MSwiSjMyIjoxLCJKMzMiOjEsIkozNCI6MSwiSjM1IjoxLCJKMzYiOjEsIkozNyI6MSwiSjM4IjoxLCJKMzkiOjEsIko0MCI6MSwiSjQxIjoxLCJKNDIiOjEsIko0MyI6MSwiSjQ0IjoxLCJKNDUiOjEsIko0NiI6MSwiSzEiOjEsIksyIjoxLCJLMyI6MSwiSzQiOjEsIks1IjoxLCJLNiI6MSwiSzciOjEsIks5IjoxLCJINTkiOjEsIkg1OCI6MSwiSDU3IjoxLCJHMzQiOjEsIkczNSI6MSwiRzM2IjoxLCJHMzciOjEsIkczOCI6MSwiRzM5IjoxLCJHNDAiOjEsIkc0MSI6MSwiRzQyIjoxLCJHNDMiOjEsIkc0NCI6MSwiRzQ1IjoxLCJHNDYiOjEsIkc0NyI6MSwiRzQ4IjoxLCJHNDkiOjEsIkc1MCI6MSwiRzUxIjoxLCJHNTIiOjEsIkc1MyI6MSwiRzU0IjoxLCJIMSI6MSwiSDIiOjEsIkczMyI6MSwiSDMiOjEsIkczMiI6MSwiRzMwIjoxLCJHNyI6MSwiRzgiOjEsIkc5IjoxLCJHMTAiOjEsIkcxMSI6MSwiRzEyIjoxLCJHMTMiOjEsIkcxNCI6MSwiRzE1IjoxLCJHMTYiOjEsIkcxNyI6MSwiRzE4IjoxLCJHMTkiOjEsIkcyMCI6MSwiRzIxIjoxLCJHMjIiOjEsIkcyMyI6MSwiRzI0IjoxLCJHMjUiOjEsIkcyNiI6MSwiRzI3IjoxLCJHMjgiOjEsIkcyOSI6MSwiRzMxIjoxLCJHNSI6MSwiSDQiOjEsIkg2IjoxLCJIMzQiOjEsIkgzNSI6MSwiSDM2IjoxLCJIMzciOjEsIkgzOCI6MSwiSDM5IjoxLCJINDAiOjEsIkg0MSI6MSwiSDQyIjoxLCJINDMiOjEsIkg0NCI6MSwiSDQ1IjoxLCJINDYiOjEsIkg0NyI6MSwiSDQ4IjoxLCJINDkiOjEsIkg1MCI6MSwiSDUxIjoxLCJINTIiOjEsIkg1MyI6MSwiSDU0IjoxLCJINTUiOjEsIkg1NiI6MSwiSDMzIjoxLCJINSI6MSwiSDMyIjoxLCJIMzAiOjEsIkg3IjoxLCJIOCI6MSwiSDkiOjEsIkgxMCI6MSwiSDExIjoxLCJIMTIiOjEsIkgxMyI6MSwiSDE0IjoxLCJIMTUiOjEsIkgxNiI6MSwiSDE3IjoxLCJIMTgiOjEsIkgxOSI6MSwiSDIwIjoxLCJIMjEiOjEsIkgyMiI6MSwiSDIzIjoxLCJIMjQiOjEsIkgyNSI6MSwiSDI2IjoxLCJIMjciOjEsIkgyOCI6MSwiSDI5IjoxLCJIMzEiOjEsIlM0MCI6MSwiUzQxIjoxLCJTNDIiOjEsIlMzOSI6MSwiUjM4IjoxLCJSMzkiOjEsIlI0MCI6MSwiUjQxIjoxLCJSNDIiOjEsIlM0MyI6MSwiUzQ0IjoxLCJYOCI6MSwiWDE0IjoxLCJYMTMiOjEsIlgxMiI6MSwiWDExIjoxLCJYMTAiOjEsIlg5IjoxLCJYMTciOjEsIlg3IjoxLCJYNiI6MSwiWDUiOjEsIlg0IjoxLCJYMyI6MSwiWDIiOjEsIlgxIjoxLCJYMTYiOjEsIlgxOCI6MSwiWDIwIjoxLCJFNDAiOjEsIlgyMSI6MSwiWDIyIjoxLCJYMjMiOjEsIlgyNSI6MSwiWDI2IjoxLCJYMjciOjEsIlgyOCI6MSwiWDMwIjoxLCJYMzEiOjEsIlgzMiI6MSwiUTEzIjoxLCJYMzMiOjEsIlgzNSI6MSwiVDI1IjoxLCJXMTAiOjEsIkE0IjoxLCJQODMiOjEsIlA4NCI6MSwiSDYzIjoxLCJPNzciOjEsIlA4NSI6MSwiUDg2IjoxLCJQODciOjEsIlQyMCI6MSwiVDIxIjoxLCJUMjIiOjEsIlQyMyI6MSwiVDI0IjoxLCJUMjYiOjEsIlQyNyI6MSwiUDg4IjoxLCJYMzciOjEsIlgzOCI6MSwiWDM5IjoxLCJYNDAiOjEsIlg0MSI6MSwiWDQyIjoxfSwiaWF0IjoxNzA3NzkzNTE0fQ.lIW6C_km3oyiJUdvfMKXWPGV3-Mhj24hwonvj6FvxNA");

            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, HttpContent content)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            return await _client.SendAsync(request);
        }
    }
}
