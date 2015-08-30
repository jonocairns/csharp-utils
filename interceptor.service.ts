interface IResponse {
    config: IResponseConfig;
    data: string;
    headers: any;
    status: number;
    statusText: string;
}

interface IResponseConfig {
    cache: any;
    headers: Function;
    method: string;
    transformRequest: Array<any>;
    transformResponse: Array<any>;
    url: string;
}

((): void => {
    'use strict';
    angular
        .module('app.services').factory('app.interceptor', [
        '$q', 'app.services.ConectivityService', 'app.services.TokenService', 'app.common.ApiEndpoint', 'app.services.MessagingService',
        ($q: any, connectivityService: app.services.ConectivityService, tokenService: app.services.ITokenService,
            apiEndpointConfig: app.common.IApiEndpointConfig, messagingService: app.services.IMessagingService) => {
            return {
                'request': (config: any): any => {
                    if (app.common.LocationValidator.validatePublicApi(config.url, apiEndpointConfig.baseUrl)) {
                        return config;
                    }
                    var token = tokenService.validate();
                    if (!token.isEmpty()) {
                        config.headers['x-jwt'] = token.value;
                        return config;
                    }
                    return config;
                },
                'response': (response: IResponse): any => {
                    var token: app.token.IToken = tokenService.get();
                    var responseVersion = response.headers('x-ignite-api-version');
                    if (!token.isEmpty() && !_.isUndefined(responseVersion) && responseVersion !== null) {
                        var decodeToken: app.services.IDecodedJwt = app.services.JwtUtilityService.decodeToken(token);
                        if (responseVersion !== decodeToken.version) {
                            tokenService.clearClientCache();
                        }
                    }
                    return response;
                },
                'responseError': (rejection: any) => {
                    if (rejection.status === 0) {
                        connectivityService.setOnlineStatus(false);
                    } else {
                        connectivityService.setOnlineStatus(true);
                    }
                    if (rejection.status === 401) {
                        tokenService.refresh();
                    }
                    if (rejection.status === 429) {
                        messagingService.setErrorMessage(rejection.data, true);
                    }
                    return $q.reject(rejection);
                }
            };
        }
    ]);

    angular
        .module('app.services').config([
        '$httpProvider', ($httpProvider: any) => {
            $httpProvider.interceptors.push('app.interceptor');
        }
    ]);

})();