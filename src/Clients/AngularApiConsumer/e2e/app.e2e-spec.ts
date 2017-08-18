import { NgApiConsumerPage } from './app.po';

describe('ng-api-consumer App', () => {
  let page: NgApiConsumerPage;

  beforeEach(() => {
    page = new NgApiConsumerPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
