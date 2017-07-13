import { LinksTaskPage } from './app.po';

describe('links-task App', () => {
  let page: LinksTaskPage;

  beforeEach(() => {
    page = new LinksTaskPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
