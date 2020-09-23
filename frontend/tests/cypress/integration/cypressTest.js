describe("My First Test", () => {
  it("Click Home button", () => {
    cy.visit("/");

    cy.contains("Home").click();
  });
});
