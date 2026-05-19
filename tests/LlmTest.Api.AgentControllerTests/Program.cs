var tests = new AgentControllerTests();

await tests.Ask_ReturnsOrderAnswer_WhenMessageHasOrderId();
await tests.Ask_ReturnsBadRequest_WhenMessageIsEmpty();
await tests.Ask_ReturnsBadRequest_WhenMessageHasNoOrderId();

Console.WriteLine("AgentController tests passed.");
