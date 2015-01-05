///**
// * Compares the object with an other object.
// * @param {object} other The object to compare with.
// * @return True if the other object equals this object, otherwise false.
// */
//Object.prototype.eq = function (other) {
//    return this === other;
//};
/**
 * Compares the left and right operand on equality.
 * @param {object} l The left operand.
 * @param {object} r The right operand.
 * @return True if the left operand equals the right operand, otherwise false.
 */
function eq(l, r) {
    if (arguments.length !== 2) { throw new Error('Invalid number of arguments.'); }
    if (l !== null && l !== undefined && typeof (l.equals) === 'function') {
    	return l.equals(r);
    }
    if (r !== null && r !== undefined && typeof (r.equals) === 'function') {
    	return r.equals(l);
    }
    return l === r;
} 