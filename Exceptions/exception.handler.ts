'use strict';

module app.exceptionOverride {

    angular.module('app.exception')
        .config(['$provide', ($provide: ng.auto.IProvideService) => {
        $provide.decorator('$exceptionHandler', ['$delegate', 'app.services.LoggingService', '$injector',
            ($delegate: ng.IExceptionHandlerService, logger: app.services.ILoggingService, $injector: any) => (exception: any) => {
                var errorMessage = '';
                if (!_.isUndefined(exception) && !_.isNull(exception)) {
                    errorMessage = 'Exception name: ' + exception.name + '\n Exception message: ' + exception.message + '\n';
                } else {
                    errorMessage = 'An exception occured in the application but no usefull information has been caught from the exception itself - check location and current user for more information. \n\n';
                }
                errorMessage += getLocationInfo();

                if (!_.isUndefined(exception) && !_.isUndefined(exception.stack)) {
                    errorMessage += '\n StackTrace: ' + exception.stack;
                }

                logger.logError(errorMessage);
                $delegate(exception);

                function getLocationInfo() {
                    var info = '';
                    $injector.invoke([
                        '$location', ($location: any) => {
                            info = ' Route that error occured: ' + $location.path() + '\n Absolute Path: ' + $location.absUrl() + '\n';
                        }
                    ]);
                    return info;
                }
            }


        ]);
    }]);
}