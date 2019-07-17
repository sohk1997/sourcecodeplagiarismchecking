Base method: 
public boolean updateRole(String[] userID) throws SQLException {
    boolean check = false;
    List<String> possibleValues = Arrays.asList(userID);
    StringBuilder builder = new StringBuilder();
    for (int i = 0; i < possibleValues.size(); i++) {
        builder.append("?,");
    }
    try {
        String sql = "update tblUsers set roleID = 'AD' where userID in (" + builder.deleteCharAt(builder.length() - 1).toString() + ")";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        int index = 1;
        for (Object o : possibleValues) {
            // or whatever it applies
            preStm.setObject(index++, o);
        }
        check = preStm.executeUpdate() > 0;
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return check;
}
Next method
public String checkLogin(String username, String password) throws SQLException {
    String role = "failed";
    try {
        String sql = "select r.roleName from tblUsers u, tblRoles r where u.userID = ? and u.password = ? and u.roleID = r.roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, username);
        preStm.setString(2, getEncrypPassword(password));
        rs = preStm.executeQuery();
        if (rs.next()) {
            role = rs.getString("roleName");
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return role;
}
Next method
public List<User> searchLikeName(String name) throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID and \n" + "(u.firstName like ? or u.lastName like ?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, "%" + name + "%");
        preStm.setString(2, "%" + name + "%");
        rs = preStm.executeQuery();
        while (rs.next()) {
            String userID = rs.getString("userID");
            String firstName = rs.getString("firstName");
            String lastName = rs.getString("lastName");
            String email = rs.getString("email");
            String role = rs.getString("roleName");
            list.add(new User(userID, firstName, lastName, email, role));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public List<User> searchLikeName(String name) throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID and \n" + "(u.firstName like ? or u.lastName like ?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, "%" + name + "%");
        preStm.setString(2, "%" + name + "%");
        rs = preStm.executeQuery();
        while (rs.next()) {
            String userID = rs.getString("userID");
            String firstName = rs.getString("firstName");
            String lastName = rs.getString("lastName");
            String email = rs.getString("email");
            String role = rs.getString("roleName");
            list.add(new User(userID, firstName, lastName, email, role));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public boolean updateRole(String[] userID) throws SQLException {
    boolean check = false;
    List<String> possibleValues = Arrays.asList(userID);
    StringBuilder builder = new StringBuilder();
    for (int i = 0; i < possibleValues.size(); i++) {
        builder.append("?,");
    }
    try {
        String sql = "update tblUsers set roleID = 'AD' where userID in (" + builder.deleteCharAt(builder.length() - 1).toString() + ")";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        int index = 1;
        for (Object o : possibleValues) {
            // or whatever it applies
            preStm.setObject(index++, o);
        }
        check = preStm.executeUpdate() > 0;
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return check;
}
Next method
public boolean updateRole(String[] userID) throws SQLException {
    boolean check = false;
    List<String> possibleValues = Arrays.asList(userID);
    StringBuilder builder = new StringBuilder();
    for (int i = 0; i < possibleValues.size(); i++) {
        builder.append("?,");
    }
    try {
        String sql = "update tblUsers set roleID = 'AD' where userID in (" + builder.deleteCharAt(builder.length() - 1).toString() + ")";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        int index = 1;
        for (Object o : possibleValues) {
            // or whatever it applies
            preStm.setObject(index++, o);
        }
        check = preStm.executeUpdate() > 0;
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return check;
}
