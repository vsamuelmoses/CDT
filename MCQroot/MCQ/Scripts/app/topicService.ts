module Mcq {
    export module Home {
        export function topicServiceFactory($http: ng.IHttpService, $q: ng.IQService, helpers: Mcq.Helpers.HttpHelpers) {
            return new TopicService($q, $http, helpers);
        }

        export class TopicService {
            constructor(
                private $q: ng.IQService,
                private $http: ng.IHttpService,
                private helpers: Mcq.Helpers.HttpHelpers) {
            }

            getTopics(): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.topictService + "/getTopics") });
            }

            getTopicById(id): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.topictService + "/getTopicById/" + id) });
            }

            getDashboardInfo(): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.topictService + "/getDashboardInfo") });
            }

            getTopicSummary(id: number): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.topictService + "/getTopicInfo/" + id) });
            }

            resetProgressForTopic(topicId: number): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.topictService + "/resetProgressForTopic/" + topicId) });
            }

            resetProgress(): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.topictService + "/resetProgress") });
            }
        }
    }
}

Mcq.Home.topicServiceFactory.$inject = ["$http", "$q", "httpHelpers"];    