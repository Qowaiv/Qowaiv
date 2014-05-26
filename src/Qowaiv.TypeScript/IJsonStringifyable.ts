/**
 * To support JSON.stringify()
 */
interface IJsonStringifyable {
    /** 
     * Returns a JSON representation of the object.
     */
    toJSON(): string;

    /**
     * Creates an object from a JSON string.
     * @param {string} s A JSON string representing the object.
     * @return A coverted object if valid, otherwise null.
     */
    fromJSON(s: string): any;
}