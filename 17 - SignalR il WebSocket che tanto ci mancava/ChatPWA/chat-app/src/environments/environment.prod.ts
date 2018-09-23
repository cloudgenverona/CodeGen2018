export const environment = {
  production: true,
  baseUrl: 'https://codegensignalr.azurewebsites.net/api/',
  baseHubs: 'https://codegensignalr.azurewebsites.net',
  controllers: {
    Auth : 'Auth',
    Groups: 'Groups',
    JoinGroups : 'JoinGroups',
    UserStats: 'UserStats'
  }
};

/*
export const environment = {
  production: false,
  baseUrl: 'https://localhost:44341/api/',
  baseHubs: 'https://localhost:44341',
  controllers: {
    Auth : 'Auth',
    Groups: 'Groups',
    JoinGroups : 'JoinGroups',
    UserStats: 'UserStats'
  }
};
*/
