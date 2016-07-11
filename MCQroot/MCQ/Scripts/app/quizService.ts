module Mcq {
    export module Quiz {
        export function quizServiceFactory($http: ng.IHttpService, $q: ng.IQService, helpers: Mcq.Helpers.HttpHelpers) {
            return new QuizService($q, $http, helpers);
        }

        export class QuizService {
            constructor(
                private $q: ng.IQService,
                private $http: ng.IHttpService,
                private helpers: Mcq.Helpers.HttpHelpers) {
            }

            getQuizQuestionsData(topicId: number): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.quizService + "/getQuizQuestionsData/" + topicId) });
            }

            getNextQuestionForTopic(topicId: number, prevQuestionId: number, p, s): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.quizService + "/getQuestionForTopic/" + topicId + "/" + prevQuestionId + "/" + p + "/" + s) });
            }

            getPrevQuestionForTopic(topicId: number, prevQuestionId: number, p, s): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.quizService + "/getPrevQuestionForTopic/" + topicId + "/" + prevQuestionId + "/" + p + "/" + s) });
            }

            //markQuestionAsRead(questionId: number): ng.IPromise<ResponseResult> {
            //    return this.helpers.http(() => { return this.$http.get(window["urls"].services.quizService + "/markQuestionAsRead/" + questionId) });
            //}

            changeFlagState(questionId: number): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.quizService + "/changeFlagState/" + questionId) });
            }

            revealAnswer(vm: any): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.post(window["urls"].services.quizService + "/revealAnswer", vm) });
            }

        }
    }
}

Mcq.Quiz.quizServiceFactory.$inject = ["$http", "$q", "httpHelpers"];     