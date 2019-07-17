private static void basicAuth() throws IOException, InterruptedException {
    var VARIABLE_9 = VARIABLE_8.newHttpClient();
    var VARIABLE_7 = VARIABLE_6.newBuilder().uri(VARIABLE_5.create("https://postman-echo.com/basic-auth")).build();
    var VARIABLE_4 = VARIABLE_9.send(VARIABLE_7, VARIABLE_3.BodyHandlers.ofString());
    // 401
    VARIABLE_2.out.println(VARIABLE_4.statusCode());
    var VARIABLE_1 = VARIABLE_8.newBuilder().authenticator(new Authenticator() {

        @Override
        protected PasswordAuthentication getPasswordAuthentication() {
            return new PasswordAuthentication("postman", "password".toCharArray());
        }
    }).build();
    var request2 = HttpRequest.newBuilder().uri(URI.create("https://postman-echo.com/basic-auth")).build();
    var response2 = authClient.send(request2, HttpResponse.BodyHandlers.ofString());
    // 200
    System.out.println(response2.statusCode());
}