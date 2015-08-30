'use strict';

module app.common {

    export class Guid {

        public static guidPattern: string = '^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$';

        public static newGuid(): string {
            return this.generateGuid();
        }

        public static isGuid(guid: string): boolean {
            return guid.match(this.guidPattern) !== null;
        }

        private static generateGuid(): string {
            return this.generatePart() + this.generatePart() + '-' + this.generatePart() + '-' + this.generatePart() + '-' +
                this.generatePart() + '-' + this.generatePart() + this.generatePart() + this.generatePart();
        }

        private static generatePart(): string {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
    }
} 