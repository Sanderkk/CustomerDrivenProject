describe('My First Test', () => {
  it('clicks the link "type"', () => {
    cy.visit('/')

    cy.contains('type').click()
  })
})