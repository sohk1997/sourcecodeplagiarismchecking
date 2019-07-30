/**
 * Writes the specified message to a servlet log file.
 * @param msg message
 */
public void log(String VARIABLE_3) {
    if (VARIABLE_2 != null) {
        VARIABLE_2.getServletContext().log(VARIABLE_3);
    } else {
        VARIABLE_1.out.print(VARIABLE_3);
    }
}