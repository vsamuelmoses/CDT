module Mcq {
    export module TopicsMenu {
        import TopicService = Mcq.Home.TopicService;

        export function topicsMenuController($scope: ITopicsMenuScope, $stateParams, $rootScope, $state, topicService: TopicService) {

            var init = () => {
               
                topicService.getTopics().then((response) => {
                    $scope.topics = response.data;
                });
 
            }
            init();

            $scope.$on("userSignedIn", () => {
                init();
            });
            $scope.$on("refreshProgress",() => {
                init();
            });


            //$scope.topics = [
            //    {
            //        topic : {id: 1,
            //        name: "Topic 1"},
            //        progress: 20
            //    }
            //];
            //$rootScope.selectedTopic = $scope.topics[0];

            $scope.selectTopic = (topic) => {
                //if (!$rootScope.loggedUser)
                //    return;
                $rootScope.selectedTopic = topic;
                $rootScope.$$phase || $rootScope.$apply();
                $state.go("topicSummary");
                $rootScope.toggleNav();
            }
        }
    }
}

Mcq.TopicsMenu.topicsMenuController.$inject = ["$scope", "$stateParams", "$rootScope", "$state", "topicService"];  