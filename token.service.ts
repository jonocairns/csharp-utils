'use strict';
module app.services {

    export interface ITokenService {
        validate(): app.token.IToken;
        refresh(): void;
        login(): void;
        get(): app.token.IToken;
        logout(): void;
        clearClientCache(): void;
    }

    export class TokenService implements ITokenService {
        constructor(private $cookies: any, private $injector: any,
            private $location: ng.ILocationService, private apiEndpoint: app.common.IApiEndpointConfig,
            private $window: ng.IWindowService, private cache: ICacheService) {
        }

        public get(): app.token.IToken {
            var cookie = this.$cookies['ignite.token'];
            if (_.isUndefined(cookie)) {
                return app.token.Token.empty();
            }
            return new app.token.Token(cookie);
        }

        public validate(): app.token.IToken {
            var token = this.get();
            if (token.isEmpty()) {
                this.refresh();
            } else {
                // let the root scope know that the user has a token and the page can be displayed - this is picked up in the common.page.class.controller
                this.$injector.invoke([
                    '$rootScope', ($rootScope: IIgniteRootScope) => {
                        $rootScope.igniteAuthenticationLoaderFlag = false;
                    }
                ]);
            }
            return token;
        }

        public refresh(): void {
            this.$injector.invoke(['$http', '$rootScope', '$location', ($http: ng.IHttpService, $rootScope: IIgniteRootScope, $location: ng.ILocationService) => {
                var ret = $location.absUrl();
                var encoded = btoa(ret);

                $http.get(this.apiEndpoint.baseUrl + '/auth/request?request=' + encoded).then((urlRedirect: any) => {
                    this.$window.location.href = urlRedirect.data.RedirectAddress;
                },(resp: any) => {
                        $rootScope.igniteAuthenticationLoaderFlag = false;
                        if (resp.status !== 0) {
                            $location.path('/error');
                        }
                    });
            }]);
        }

        public logout(): void {
            this.$injector.invoke(['$http', '$timeout', ($http: ng.IHttpService, $timeout: ng.ITimeoutService) => {
                $http.get(this.apiEndpoint.baseUrl + '/auth/logout').then((urlRedirect: any) => {
                    this.clearClientCache();
                    $timeout(() => {
                        this.$window.location.href = urlRedirect.data.RedirectAddress;
                    });
                },() => {
                        this.clearClientCache();
                    });
            }]);
        }

        public clearClientCache(): void {
            this.removeCookie();
            this.cache.clearAll();
            this.$location.path('/');
            this.$window.location.reload();
        }

        public login(): void {
            var currentPage = this.$location.absUrl(); // TODO: AM to replace split with something more concrete.
            var encoded = btoa(currentPage.split('#')[0] + '#/home');

            this.$injector.invoke(['$http', ($http: ng.IHttpService) => {
                $http.get(this.apiEndpoint.baseUrl + '/auth/request?request=' + encoded).then((urlRedirect: any) => {
                    this.$window.location.href = urlRedirect.data.RedirectAddress;
                },() => {
                        this.$window.location.href = currentPage + 'home';
                        this.$window.location.reload();
                    });
            }]);
        }

        public removeCookie() {
            this.$cookies['ignite.token'] = '';
            delete this.$cookies['ignite.token'];
            document.cookie = 'ignite.token=; domain=.' + this.$location.host() + ';expires=Wed, 31 Oct 2012 08:50:17 GMT;';
        }
    }

function factory($cookies: any, $injector: any, $location: ng.ILocationService, apiEndpoint: app.common.IApiEndpointConfig,
    $window: ng.IWindowService, cache: ICacheService): ITokenService {
    return new TokenService($cookies, $injector, $location, apiEndpoint, $window, cache);
}
factory.$inject = [
    '$cookies',
    '$injector',
    '$location',
    'app.common.ApiEndpoint',
    '$window',
    'app.services.CacheService'
];
angular
    .module('app.services')
    .factory('app.services.TokenService', factory);
} 