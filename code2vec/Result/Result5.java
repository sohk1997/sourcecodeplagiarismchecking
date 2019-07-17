Base method: 
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
private List<String> dfs(Set<String> dict, String s) {
    // calculated, just return
    if (memo.containsKey(s))
        return memo.get(s);
    List<String> rst = new ArrayList<>();
    if (s.length() == 0)
        return rst;
    // total match word
    if (dict.contains(s))
        rst.add(s);
    // loop over form index -> n, find candidates, validate, dfs
    StringBuffer sb = new StringBuffer();
    for (int i = 1; i < s.length(); i++) {
        sb.append(s.charAt(i - 1));
        if (!dict.contains(sb.toString())) {
            continue;
        }
        String suffix = s.substring(i);
        List<String> segments = dfs(dict, suffix);
        for (String segment : segments) {
            rst.add(sb.toString() + " " + segment);
        }
    }
    memo.put(s, rst);
    return rst;
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
