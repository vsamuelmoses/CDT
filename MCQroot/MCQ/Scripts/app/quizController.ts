module Mcq {
    export module Quiz {
        export function quizController($scope, $stateParams, $rootScope, $state, quizService: QuizService, $cookies) {
            if (!$rootScope.selectedTopic)
                return;
            $scope.selectedState = 0;
            $scope.$$phase || $scope.apply();
            quizService.getQuizQuestionsData($rootScope.selectedTopic.id).then((response) => {
                $rootScope.unReadedQuestions = response.data.unreadQuestionsIds;
                $rootScope.flaggedQuestionsIds = response.data.flaggedQuestionsIds;
                //$rootScope.lastAllOrder = null;
                //$rootScope.lastUnreadedOrder = null;
                //$rootScope.lastFlaggedOrder = null;
                $rootScope.$broadcast("initQuestion", 0);
                $scope.selectedState = 0;
                $scope.$$phase || $scope.apply();
            });


            $scope.all = () => {
                $rootScope.$broadcast("initQuestion", 0);
                $cookies[$rootScope.selectedTopic.id + "_last"] = $rootScope.lastAllOrder - 1;
                $scope.selectedState = 0;
                $scope.$$phase || $scope.apply();
            }

            $scope.unread = () => {
                if ($rootScope.unReadedQuestions.length === 0)
                    return;
                $cookies[$rootScope.selectedTopic.id + "_last"] = $rootScope.lastUnreadedOrder - 1;
                $rootScope.$broadcast("initQuestion", 1);
                $scope.selectedState = 1;
                $scope.$$phase || $scope.apply();
            }

            $scope.flagged = () => {
                if ($rootScope.flaggedQuestionsIds.length === 0) {
                    $scope.selectedState = 2;
                    $scope.$$phase || $scope.apply();
                    return;
                }
                    
                $cookies[$rootScope.selectedTopic.id + "_last"] = $rootScope.lastFlaggedOrder - 1;
                $rootScope.$broadcast("initQuestion", 2);
                $scope.selectedState = 2;
                $scope.$$phase || $scope.apply();

            }

            $scope.$on("initTabsData",(data) => {
                quizService.getQuizQuestionsData($rootScope.selectedTopic.id).then((response) => {
                    $rootScope.unReadedQuestions = response.data.unreadQuestionsIds;
                    $rootScope.flaggedQuestionsIds = response.data.flaggedQuestionsIds;
                   
                });
            });

            $scope.$on("changeTab", (data) => {
                $scope.all();
            });

            $scope.$on("questionReaded",(data) => {
                //if (Enumerable.from($scope.unReadedQuestions).any((id) => { return id === data.targetScope.questionViewModel.id })) {
                    //quizService.markQuestionAsRead(data.targetScope.questionViewModel.id).then((response) => {
                    //    if (response.isSucceed)
                    //        $rootScope.unReadedQuestions.splice(Enumerable.from($rootScope.unReadedQuestions).indexOf((x) => { return x === data.targetScope.questionViewModel.id }), 1);
                    //});
               // }
            });
           
           
        }
    }
}

Mcq.Quiz.quizController.$inject = ["$scope", "$stateParams", "$rootScope", "$state", "quizService", "$cookies"];  