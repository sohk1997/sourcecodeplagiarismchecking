Base method: 
public String addUser(User user) throws SQLException {
    String message = "failed";
    boolean check = false;
    try {
        String sql = "insert into tblUsers(userID, firstName, lastName, password, email, roleID, isSendNotification) " + "values(?,?,?,?,?,?,?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, user.getUserID());
        preStm.setString(2, user.getFirstName());
        preStm.setString(3, user.getLastName());
        preStm.setString(4, getEncrypPassword(user.getPassword()));
        preStm.setString(5, user.getEmail());
        preStm.setString(6, user.getRole());
        preStm.setString(7, user.getNotification());
        check = preStm.executeUpdate() > 0;
        if (check)
            message = "success";
    } catch (Exception e) {
        if (e.getMessage().contains("Cannot insert duplicate key")) {
            message = "Duplicate key";
        } else {
            e.printStackTrace();
        }
    } finally {
        closConnection();
    }
    return message;
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
public List<User> searchLikeNameWithRole(String searchName, String searchRole) throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID and u.roleID like ? and \n" + "(u.firstName like ? or u.lastName like ?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, searchRole);
        preStm.setString(2, "%" + searchName + "%");
        preStm.setString(3, "%" + searchName + "%");
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
public List<User> searchLikeNameWithRole(String searchName, String searchRole) throws SQLException {
    List<User> list = new ArrayList<>();
    try {
        String sql = "select u.userID, u.firstName, u.lastName, u.email, r.roleName\n" + "from tblUsers u, tblRoles r\n" + "where u.roleID = r.roleID and u.roleID like ? and \n" + "(u.firstName like ? or u.lastName like ?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, searchRole);
        preStm.setString(2, "%" + searchName + "%");
        preStm.setString(3, "%" + searchName + "%");
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
public String addUser(User user) throws SQLException {
    String message = "failed";
    boolean check = false;
    try {
        String sql = "insert into tblUsers(userID, firstName, lastName, password, email, roleID, isSendNotification) " + "values(?,?,?,?,?,?,?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, user.getUserID());
        preStm.setString(2, user.getFirstName());
        preStm.setString(3, user.getLastName());
        preStm.setString(4, getEncrypPassword(user.getPassword()));
        preStm.setString(5, user.getEmail());
        preStm.setString(6, user.getRole());
        preStm.setString(7, user.getNotification());
        check = preStm.executeUpdate() > 0;
        if (check)
            message = "success";
    } catch (Exception e) {
        if (e.getMessage().contains("Cannot insert duplicate key")) {
            message = "Duplicate key";
        } else {
            e.printStackTrace();
        }
    } finally {
        closConnection();
    }
    return message;
}
Next method
public String addUser(User user) throws SQLException {
    String message = "failed";
    boolean check = false;
    try {
        String sql = "insert into tblUsers(userID, firstName, lastName, password, email, roleID, isSendNotification) " + "values(?,?,?,?,?,?,?)";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        preStm.setString(1, user.getUserID());
        preStm.setString(2, user.getFirstName());
        preStm.setString(3, user.getLastName());
        preStm.setString(4, getEncrypPassword(user.getPassword()));
        preStm.setString(5, user.getEmail());
        preStm.setString(6, user.getRole());
        preStm.setString(7, user.getNotification());
        check = preStm.executeUpdate() > 0;
        if (check)
            message = "success";
    } catch (Exception e) {
        if (e.getMessage().contains("Cannot insert duplicate key")) {
            message = "Duplicate key";
        } else {
            e.printStackTrace();
        }
    } finally {
        closConnection();
    }
    return message;
}
